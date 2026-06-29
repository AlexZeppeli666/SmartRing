using Smart_Ring.Services;

namespace Smart_Ring.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string username = EntryUsername.Text?.Trim() ?? string.Empty;
        string password = EntryPassword.Text ?? string.Empty;

        // 1. Validación básica de campos vacíos
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            await DisplayAlertAsync("Campos Vacíos", "Por favor, ingresa tu usuario y contraseña.", "OK");
            return;
        }

        try
        {
            // 2. Recuperamos el servicio de base de datos desde las dependencias de la App
            var dbService = App.Current?.Handler?.MauiContext?.Services.GetService<DatabaseService>();

            if (dbService == null)
            {
                await DisplayAlertAsync("Error", "No se pudo conectar a la base de datos local.", "OK");
                return;
            }

            // 3. Validamos las credenciales contra la tabla de SQLite
            var usuarioValido = await dbService.ValidateLoginAsync(username, password);

            if (usuarioValido != null)
            {
                // ==================== 💡 ASIGNAR LA SESIÓN GLOBAL ====================
                // Guardamos los datos reales extraídos de SQLite antes de ir al Home
                SessionService.UsuarioActual = usuarioValido;
                SessionService.NombreUsuario = usuarioValido.FirstName;
                // =====================================================================

                await DisplayAlertAsync("¡Bienvenido(a)!", $"Hola de nuevo, {usuarioValido.FirstName}.", "OK");

                // CORREGIDO: Usar navegación absoluta del Shell para limpiar la pila 
                // y activar correctamente los componentes como el BottomNavBar
                await Shell.Current.GoToAsync("//HomePage");
            }
            else
            {
                await DisplayAlertAsync("Acceso Denegado", "Usuario o contraseña incorrectos. Inténtalo de nuevo.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error Crítico", $"Ocurrió un inconveniente: {ex.Message}", "OK");
        }
    }

    private async void OnRegisterTapped(object sender, TappedEventArgs e)
    {
        // Para ir al registro está bien usar PushAsync si deseas que puedan regresar al Login con la flecha
        await Navigation.PushAsync(new RegisterPage());
    }
}