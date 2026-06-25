using Smart_Ring.Views;

namespace Smart_Ring;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Pantallas de detalle: se navegan de forma relativa (push) desde HomePage,
        // por eso no aparecen como ShellContent de nivel superior en el XAML.
        Routing.RegisterRoute(nameof(HeartRateDetailPage), typeof(HeartRateDetailPage));
        Routing.RegisterRoute(nameof(OxygenDetailPage), typeof(OxygenDetailPage));
        Routing.RegisterRoute(nameof(AlertIndexPage), typeof(AlertIndexPage));
    }
}
