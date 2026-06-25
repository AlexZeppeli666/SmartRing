using System.Diagnostics;
using Microsoft.Maui.Devices;
using Smart_Ring.Graphics;

namespace Smart_Ring.Views;

public partial class AlertIndexPage : ContentPage
{
    private enum TestState { Idle, Waiting, ReadyToTap }

    private readonly AlertGaugeDrawable _gauge = new() { Percentage = 0.88 };
    private TestState _state = TestState.Idle;
    private readonly Stopwatch _stopwatch = new();
    private Color _idleColor = default!;
    private CancellationTokenSource? _cts;

    public AlertIndexPage()
    {
        InitializeComponent();
        GaugeView.Drawable = _gauge;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _idleColor = (Color)Application.Current!.Resources["PrimaryAccent"];
    }

    private async void OnReflexButtonClicked(object sender, EventArgs e)
    {
        switch (_state)
        {
            case TestState.Idle:
                await StartWaitingPhaseAsync();
                break;

            case TestState.ReadyToTap:
                StopAndScoreReaction();
                break;

            case TestState.Waiting:
                await HandleEarlyTapAsync();
                break;
        }
    }

    private async Task StartWaitingPhaseAsync()
    {
        _state = TestState.Waiting;
        LabelResult.Text = string.Empty;
        BtnReflexTest.Text = "ESPERA...";
        BtnReflexTest.BackgroundColor = (Color)Application.Current!.Resources["TextSecondary"];
        LabelInstructions.Text = "Espera a que el botón cambie de color...";

        _cts = new CancellationTokenSource();
        var randomDelay = TimeSpan.FromMilliseconds(Random.Shared.Next(1500, 4000));

        try
        {
            await Task.Delay(randomDelay, _cts.Token);
        }
        catch (TaskCanceledException)
        {
            return;
        }

        if (_state != TestState.Waiting) return;

        _state = TestState.ReadyToTap;
        BtnReflexTest.Text = "¡TÓCALO AHORA!";
        BtnReflexTest.BackgroundColor = (Color)Application.Current!.Resources["SuccessSoft"];
        TryVibrate(TimeSpan.FromMilliseconds(120));
        _stopwatch.Restart();
    }

    private void StopAndScoreReaction()
    {
        _stopwatch.Stop();
        long ms = _stopwatch.ElapsedMilliseconds;

        _state = TestState.Idle;
        BtnReflexTest.Text = "▶ INICIAR PRUEBA";
        BtnReflexTest.BackgroundColor = _idleColor;
        LabelInstructions.Text = "Presiona el botón para iniciar. Cuando cambie de color, tócalo lo más rápido posible.";

        LabelResult.Text = $"⏱ Tiempo de reacción: {ms} ms";
        LabelResult.TextColor = ms < 400
            ? (Color)Application.Current!.Resources["SuccessSoft"]
            : (Color)Application.Current!.Resources["WarningSoft"];
    }

    private async Task HandleEarlyTapAsync()
    {
        _cts?.Cancel();
        _state = TestState.Idle;
        BtnReflexTest.Text = "▶ INICIAR PRUEBA";
        BtnReflexTest.BackgroundColor = _idleColor;
        LabelInstructions.Text = "Presiona el botón para iniciar. Cuando cambie de color, tócalo lo más rápido posible.";

        LabelResult.Text = "⚠ Tocaste antes de tiempo. Intenta de nuevo.";
        LabelResult.TextColor = (Color)Application.Current!.Resources["DangerSoft"];

        TryVibrate(TimeSpan.FromMilliseconds(60));
        await Task.CompletedTask;
    }

    private void TryVibrate(TimeSpan duration)
    {
        try
        {
            Vibration.Default.Vibrate(duration);
        }
        catch (FeatureNotSupportedException)
        {
            // Dispositivo sin soporte de vibración: se ignora, no es crítico.
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error de vibración: {ex.Message}");
        }
    }
}
