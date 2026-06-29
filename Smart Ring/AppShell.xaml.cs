using Smart_Ring.Views;

namespace Smart_Ring;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // REGISTRA AQUÍ TUS PÁGINAS DE DETALLE (Se abren encima de las pestañas principales)
        Routing.RegisterRoute(nameof(HeartRateDetailPage), typeof(HeartRateDetailPage));
        Routing.RegisterRoute(nameof(OxygenDetailPage), typeof(OxygenDetailPage));
        Routing.RegisterRoute(nameof(AlertIndexPage), typeof(AlertIndexPage));
    }
}