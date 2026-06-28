using Smart_Ring.Views;

namespace Smart_Ring;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // REGISTRA AQUÍ TUS PÁGINAS DE DETALLE
        Routing.RegisterRoute(nameof(Views.HeartRateDetailPage), typeof(Views.HeartRateDetailPage));
        Routing.RegisterRoute(nameof(Views.OxygenDetailPage), typeof(Views.OxygenDetailPage));
        Routing.RegisterRoute(nameof(Views.AlertIndexPage), typeof(Views.AlertIndexPage));
    }
}
