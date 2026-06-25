namespace Smart_Ring.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(EntryUsername.Text) || string.IsNullOrWhiteSpace(EntryPassword.Text))
        {
            await DisplayAlert("Datos incompletos", "Ingresa tu usuario y contraseña para continuar.", "OK");
            return;
        }

        // Prototipo: no hay backend real, se simula el inicio de sesión.
        await Shell.Current.GoToAsync("//HomePage");
    }

    private async void OnRegisterTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//RegisterPage");
    }
}
