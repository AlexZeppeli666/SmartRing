using Microsoft.Maui.Devices;

namespace Smart_Ring.Views;

public partial class WearableAccessoryPage : ContentPage
{
    private bool _isConnected = true;
    private CancellationTokenSource? _vibrationThrottleCts;

    public WearableAccessoryPage()
    {
        InitializeComponent();
        UpdateDurationLabel(SliderVibration.Value);
    }

    private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
    {
        // 1. Actualizar el texto en pantalla inmediatamente
        UpdateDurationLabel(e.NewValue);

        // 2. Cancelar la vibración pendiente anterior
        _vibrationThrottleCts?.Cancel();
        _vibrationThrottleCts = new CancellationTokenSource();
        var token = _vibrationThrottleCts.Token;

        int ms = (int)Math.Round(e.NewValue);

        // 3. Esperar un instante antes de disparar la vibración física (antirrebote)
        Task.Run(async () =>
        {
            try
            {
                // Espera de 150ms mientras el usuario sigue arrastrando
                await Task.Delay(150, token);

                if (!token.IsCancellationRequested)
                {
                    TryVibrateUniversal(TimeSpan.FromMilliseconds(ms));
                }
            }
            catch (TaskCanceledException)
            {
                // El usuario sigue moviendo el slider, ignoramos esta vibración intermedia
            }
        }, token);
    }

    private void UpdateDurationLabel(double value)
    {
        if (LabelDurationVal != null)
        {
            LabelDurationVal.Text = $"{Math.Round(value)} ms";
        }
    }

    private async void OnExecuteAlarmClicked(object sender, EventArgs e)
    {
        if (!_isConnected)
        {
            await DisplayAlert("Anillo desvinculado", "Vincula el anillo antes de ejecutar la alarma.", "OK");
            return;
        }

        BtnAlarm.IsEnabled = false;

        int ms = (int)Math.Round(SliderVibration.Value);
        bool ok = await PlayAlarmPatternUniversalAsync(ms);

        BtnAlarm.IsEnabled = true;

        if (!ok)
        {
            await DisplayAlert(
                "Función no soportada",
                "Este dispositivo no permite el uso del motor de vibración estándar o carece de permisos.",
                "Entendido");
        }
    }

    private async void OnLinkClicked(object sender, EventArgs e)
    {
        _isConnected = true;
        LabelConnection.Text = "Conectado";
        ConnectionDot.Fill = (Color)Application.Current!.Resources["SuccessSoft"];

        TryVibrateUniversal(TimeSpan.FromMilliseconds(100));
        await DisplayAlert("Vinculado", "El anillo se vinculó correctamente.", "OK");
    }

    private async void OnUnlinkClicked(object sender, EventArgs e)
    {
        _isConnected = false;
        LabelConnection.Text = "Desconectado";
        ConnectionDot.Fill = (Color)Application.Current!.Resources["DangerSoft"];

        TryVibrateUniversal(TimeSpan.FromMilliseconds(200));
        await Task.Delay(250);
        TryVibrateUniversal(TimeSpan.FromMilliseconds(200));

        await DisplayAlert("Desvinculado", "El anillo se desvinculó del dispositivo.", "OK");
    }

    private void OnAnySwitchToggled(object sender, ToggledEventArgs e)
    {
        TryVibrateUniversal(TimeSpan.FromMilliseconds(40));
    }

    private async Task<bool> PlayAlarmPatternUniversalAsync(int durationMs)
    {
        try
        {
            for (int i = 0; i < 3; i++)
            {
                TryVibrateUniversal(TimeSpan.FromMilliseconds(durationMs));
                await Task.Delay(durationMs + 150);
            }
            return true;
        }
        catch (FeatureNotSupportedException)
        {
            return false;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error de vibración: {ex.Message}");
            return false;
        }
    }

    private void TryVibrateUniversal(TimeSpan duration)
    {
        // Evita interferencias y bloqueos en Android moderno utilizando un hilo secundario
        Task.Run(() =>
        {
            try
            {
                Vibration.Default.Vibrate(duration);
            }
            catch (FeatureNotSupportedException)
            {
                System.Diagnostics.Debug.WriteLine("La vibración clásica no está soportada en este hardware.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error de vibración: {ex.Message}");
            }
        });
    }
}