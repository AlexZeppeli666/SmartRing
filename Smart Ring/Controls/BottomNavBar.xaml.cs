namespace Smart_Ring.Controls;

public partial class BottomNavBar : ContentView
{
    public BottomNavBar()
    {
        InitializeComponent();
    }

    private async void OnHomeTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//HomePage");
    }

    private async void OnWearableTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//WearableAccessoryPage");
    }

    private async void OnProfileTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//ProfilePage");
    }
}
