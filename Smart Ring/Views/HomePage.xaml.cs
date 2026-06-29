using Smart_Ring.Graphics;
using Smart_Ring.Services;

namespace Smart_Ring.Views;

public partial class HomePage : ContentPage
{
    private readonly FatigueChartDrawable _fatigueChart = new();

    public HomePage()
    {
        InitializeComponent();
        FatigueChartView.Drawable = _fatigueChart;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // 💡 Revisamos de forma estricta si guardamos el nombre en el SessionService
        if (!string.IsNullOrWhiteSpace(SessionService.NombreUsuario))
        {
            LabelWelcome.Text = $"¡Bienvenido(a) {SessionService.NombreUsuario}!";
        }
        else if (SessionService.UsuarioActual != null)
        {
            // Alternativa: Si por alguna razón NombreUsuario está vacío, lo armamos desde el modelo completo
            LabelWelcome.Text = $"¡Bienvenido(a) {SessionService.UsuarioActual.FirstName}!";
        }
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