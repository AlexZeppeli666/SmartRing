using Microsoft.Maui.Devices;

namespace Smart_Ring.Views;

public partial class WearableAccessoryPage : ContentPage
{
    private bool _isConnected = true;

    public WearableAccessoryPage()
    {
        InitializeComponent();
    }

    private async void OnExecuteAlarmClicked(object sender, EventArgs e)
    {
        if (!_isConnected)
        {
            await DisplayAlertAsync("Anillo desvinculado", "Vincula el anillo antes de ejecutar la alarma.", "OK");
            return;
        }

        BtnAlarm.IsEnabled = false;
        int pulseMs = MapIntensityToMilliseconds(SliderVibration.Value);

        bool ok = await PlayAlarmPatternAsync(pulseMs);

        BtnAlarm.IsEnabled = true;

        if (!ok)
        {
            await DisplayAlertAsync(
                "Vibración no disponible",
                "Este dispositivo no soporta el motor de vibración. " +
                "En el anillo físico, esta misma señal activaría su actuador háptico.",
                "Entendido");
        }
    }

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
        await DisplayAlertAsync("Vinculado", "El anillo se vinculó correctamente.", "OK");
    }

    private async void OnUnlinkClicked(object sender, EventArgs e)
    {
        _isConnected = false;
        LabelConnection.Text = "Desconectado";
        ConnectionDot.Fill = (Color)Application.Current!.Resources["DangerSoft"];
        await DisplayAlertAsync("Desvinculado", "El anillo se desvinculó del dispositivo.", "OK");
    }

    private void OnAnySwitchToggled(object sender, ToggledEventArgs e)
    {
        TryVibrate(TimeSpan.FromMilliseconds(40));
    }

    private static int MapIntensityToMilliseconds(double sliderValue)
    {
        const int minMs = 100, maxMs = 500;
        return minMs + (int)(sliderValue * (maxMs - minMs));
    }

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
            // feedback UI secundario
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error de vibración: {ex.Message}");
        }
    }
}