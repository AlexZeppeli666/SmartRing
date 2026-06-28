namespace Smart_Ring.Componentes;

public partial class BottomNavBar : ContentView
{
    public BottomNavBar()
    {
        InitializeComponent();

        // Ejecutar en el hilo principal para evaluar la ruta actual una vez cargado el componente
        MainThread.BeginInvokeOnMainThread(EvaluateActiveTab);
    }

    private void EvaluateActiveTab()
    {
        // Detectamos de forma segura la ruta actual de Shell
        string currentRoute = Shell.Current?.CurrentState?.Location?.ToString() ?? string.Empty;

        // Reseteamos estados visuales
        ActiveHomeIndicator.IsVisible = false;
        ActiveDeviceIndicator.IsVisible = false;
        ActiveProfileIndicator.IsVisible = false;

        // Encendemos el indicador correspondiente
        if (currentRoute.Contains("HomePage"))
            ActiveHomeIndicator.IsVisible = true;
        else if (currentRoute.Contains("WearableAccessoryPage"))
            ActiveDeviceIndicator.IsVisible = true;
        else if (currentRoute.Contains("ProfilePage"))
            ActiveProfileIndicator.IsVisible = true;
    }

    private async void OnHomeTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//HomePage");
    }

    private async void OnDeviceTapped(object sender, TappedEventArgs e)
    {
        //await Shell.Current.GoToAsync("//WearableAccessoryPage");
    }

    private async void OnProfileTapped(object sender, TappedEventArgs e)
    {
        //await Shell.Current.GoToAsync("//ProfilePage");
    }
}