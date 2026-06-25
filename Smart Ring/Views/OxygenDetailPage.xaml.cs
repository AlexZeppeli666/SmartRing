namespace Smart_Ring.Views;

public partial class OxygenDetailPage : ContentPage
{
    private const double RangeMin = 70;
    private const double RangeMax = 100;
    private double? _currentValue;
    private IDispatcherTimer? _simulationTimer;

    public OxygenDetailPage()
    {
        InitializeComponent();
        LabelDate.Text = DateTime.Now.ToString("dd/MMM");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Simula que el anillo entrega una lectura de SpO2 tras un par de segundos.
        _simulationTimer = Dispatcher.CreateTimer();
        _simulationTimer.Interval = TimeSpan.FromSeconds(2);
        _simulationTimer.Tick += (s, e) =>
        {
            SetReading(98);
            _simulationTimer?.Stop();
        };
        _simulationTimer.Start();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _simulationTimer?.Stop();
    }

    private void SetReading(double value)
    {
        _currentValue = value;
        LabelSpo2.Text = $"{value:0}%";
        PositionIndicator();
    }

    private void OnRangeBarSizeChanged(object sender, EventArgs e)
    {
        PositionIndicator();
    }

    /// <summary>
    /// Posiciona la línea indicadora dentro de la barra de rango (70-100)
    /// de forma proporcional al ancho real del contenedor.
    /// </summary>
    private void PositionIndicator()
    {
        if (_currentValue is null || RangeBarGrid.Width <= 0)
        {
            IndicatorLine.IsVisible = false;
            return;
        }

        double clamped = Math.Clamp(_currentValue.Value, RangeMin, RangeMax);
        double ratio = (clamped - RangeMin) / (RangeMax - RangeMin);
        double x = ratio * (RangeBarGrid.Width - IndicatorLine.Width);

        IndicatorLine.TranslationX = x;
        IndicatorLine.IsVisible = true;
    }
}
