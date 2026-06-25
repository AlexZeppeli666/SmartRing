using Microsoft.Maui.Devices;

namespace Smart_Ring.Views;

public partial class WearableAccessoryPage : ContentPage
{
    private bool _isConnected = true;

    public WearableAccessoryPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Botón principal de vibración: ejecuta la alarma del anillo físico.
    /// La duración se calcula a partir del slider "Suave-Fuerte".
    /// </summary>
    private async void OnExecuteAlarmClicked(object sender, EventArgs e)
    {
        if (!_isConnected)
        {
            await DisplayAlert("Anillo desvinculado", "Vincula el anillo antes de ejecutar la alarma.", "OK");
            return;
        }

        BtnAlarm.IsEnabled = false;
        int pulseMs = MapIntensityToMilliseconds(SliderVibration.Value);

        bool ok = await PlayAlarmPatternAsync(pulseMs);

        BtnAlarm.IsEnabled = true;

        if (!ok)
        {
            await DisplayAlert(
                "Vibración no disponible",
                "Este dispositivo no soporta el motor de vibración. " +
                "En el anillo físico, esta misma señal activaría su actuador háptico.",
                "Entendido");
        }
    }

    /// <summary>Vista previa de intensidad al soltar el slider de vibración.</summary>
    private void OnVibrationSliderDragCompleted(object sender, EventArgs e)
    {
        TryVibrate(TimeSpan.FromMilliseconds(MapIntensityToMilliseconds(SliderVibration.Value)));
    }

    private async void OnLinkClicked(object sender, EventArgs e)
    {
        _isConnected = true;
        LabelConnection.Text = "Conectado";
        ConnectionDot.Fill = (Color)Application.Current!.Resources["SuccessSoft"];
        TryVibrate(TimeSpan.FromMilliseconds(120));
        await DisplayAlert("Vinculado", "El anillo se vinculó correctamente.", "OK");
    }

    private async void OnUnlinkClicked(object sender, EventArgs e)
    {
        _isConnected = false;
        LabelConnection.Text = "Desconectado";
        ConnectionDot.Fill = (Color)Application.Current!.Resources["DangerSoft"];
        await DisplayAlert("Desvinculado", "El anillo se desvinculó del dispositivo.", "OK");
    }

    private void OnAnySwitchToggled(object sender, ToggledEventArgs e)
    {
        // Pequeño feedback háptico al cambiar cualquier interruptor.
        TryVibrate(TimeSpan.FromMilliseconds(40));
    }

    /// <summary>Convierte el valor del slider (0.0 - 1.0) en una duración de pulso (100ms - 500ms).</summary>
    private static int MapIntensityToMilliseconds(double sliderValue)
    {
        const int minMs = 100, maxMs = 500;
        return minMs + (int)(sliderValue * (maxMs - minMs));
    }

    /// <summary>
    /// Simula la alarma del anillo: 3 pulsos cortos con la duración indicada,
    /// igual que haría el actuador háptico físico al recibir la orden.
    /// </summary>
    private async Task<bool> PlayAlarmPatternAsync(int pulseMs)
    {
        try
        {
            for (int i = 0; i < 3; i++)
            {
                Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(pulseMs));
                await Task.Delay(pulseMs + 100);
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

    private void TryVibrate(TimeSpan duration)
    {
        try
        {
            Vibration.Default.Vibrate(duration);
        }
        catch (FeatureNotSupportedException)
        {
            // Silenciosamente ignorado: no es crítico para feedback de UI.
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error de vibración: {ex.Message}");
        }
    }
}
