using Smart_Ring.Graphics;

namespace Smart_Ring.Views;

public partial class HomePage : ContentPage
{
    private readonly FatigueChartDrawable _fatigueChart = new();

    public HomePage()
    {
        InitializeComponent();
        FatigueChartView.Drawable = _fatigueChart;
    }

    private async void OnAddDeviceTapped(object sender, TappedEventArgs e)
    {
        await DisplayAlert("Agregar dispositivo", "Búsqueda de nuevos anillos disponible próximamente.", "OK");
    }

    private async void OnHeartRateTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(HeartRateDetailPage));
    }

    private async void OnOxygenTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(OxygenDetailPage));
    }

    private async void OnAlertIndexTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AlertIndexPage));
    }
}
