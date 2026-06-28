using Smart_Ring.Models;
using Smart_Ring.Services;

namespace Smart_Ring.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        var nuevoUsuario = new UserModel
        {
            FirstName = EntryFirstName.Text?.Trim() ?? string.Empty,
            LastNamePaternal = EntryLastNamePaternal.Text?.Trim() ?? string.Empty,
            LastNameMaternal = EntryLastNameMaternal.Text?.Trim() ?? string.Empty,
            Username = EntryUsername.Text?.Trim() ?? string.Empty,
            Password = EntryPassword.Text ?? string.Empty
        };

        if (string.IsNullOrEmpty(nuevoUsuario.FirstName) || string.IsNullOrEmpty(nuevoUsuario.Username) || string.IsNullOrEmpty(nuevoUsuario.Password))
        {
            await DisplayAlert("Error", "Por favor, llena los datos obligatorios.", "OK");
            return;
        }

        // Llamamos al servicio (Instanciación directa o vía inyección de dependencias)
        var dbService = App.Current?.Handler?.MauiContext?.Services.GetService<DatabaseService>() ?? new DatabaseService();
        int resultado = await dbService.RegisterUserAsync(nuevoUsuario);

        if (resultado == -1)
        {
            await DisplayAlert("Error", "El nombre de usuario ya está registrado.", "OK");
        }
        else if (resultado > 0)
        {
            await DisplayAlert("Éxito", "Cuenta creada con éxito.", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Error", "No se pudo guardar en la base de datos.", "OK");
        }
    }

    private async void OnBackToLoginTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }
}