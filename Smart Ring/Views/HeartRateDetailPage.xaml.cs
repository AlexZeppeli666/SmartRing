namespace Smart_Ring.Views;

public partial class HeartRateDetailPage : ContentPage
{
    private IDispatcherTimer? _blinkTimer;
    private IDispatcherTimer? _simulationTimer;

    public HeartRateDetailPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _blinkTimer = Dispatcher.CreateTimer();
        _blinkTimer.Interval = TimeSpan.FromMilliseconds(700);
        _blinkTimer.Tick += (s, e) => LabelStatusText.Opacity = LabelStatusText.Opacity == 1 ? 0.3 : 1;
        _blinkTimer.Start();

        _simulationTimer = Dispatcher.CreateTimer();
        _simulationTimer.Interval = TimeSpan.FromSeconds(2.5);
        _simulationTimer.Tick += (s, e) =>
        {
            LabelBpm.Text = "74 BPM";
            LabelStatusText.Text = "Lectura en vivo";
            LabelStatusText.Opacity = 1;
            _blinkTimer?.Stop();
            _simulationTimer?.Stop();
        };
        _simulationTimer.Start();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _blinkTimer?.Stop();
        _simulationTimer?.Stop();
    }
}