namespace Smart_Ring.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(EntryFullName.Text) ||
            string.IsNullOrWhiteSpace(EntryUsername.Text) ||
            string.IsNullOrWhiteSpace(EntryPassword.Text))
        {
            await DisplayAlert("Datos incompletos", "Completa todos los campos para registrarte.", "OK");
            return;
        }

        // Prototipo: no hay backend real, se simula el registro exitoso.
        await DisplayAlert("Registro exitoso", $"Bienvenido, {EntryFullName.Text}.", "Continuar");
        await Shell.Current.GoToAsync("//HomePage");
    }

    private async void OnBackToLoginTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
