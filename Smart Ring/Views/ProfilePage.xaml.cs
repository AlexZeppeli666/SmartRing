using System.Runtime.Versioning;
using Smart_Ring.Services;

// Con esto mitigamos todas las advertencias CA1416 del archivo de forma masiva
[assembly: SupportedOSPlatform("Android21.0")]

namespace Smart_Ring.Views;

public partial class ProfilePage : ContentPage
{
    private bool _isEditingEmergencyContact;

    public ProfilePage()
    {
        InitializeComponent();
        SetEmergencyFieldsEnabled(false);
        EntryEmergencyName.Text = "Supervisor de Guardia";
        EntryEmergencyPhone.Text = "+52 55 0000 0000";
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (SessionService.UsuarioActual != null)
        {
            var user = SessionService.UsuarioActual;
            string nombreCompleto = $"{user.FirstName} {user.LastNamePaternal} {user.LastNameMaternal}".Trim();
            LabelName.Text = string.IsNullOrWhiteSpace(nombreCompleto) ? user.Username : nombreCompleto;
            LabelGender.Text = !string.IsNullOrWhiteSpace(user.Gender) ? user.Gender : "No especificado";
        }
        else
        {
            LabelName.Text = "Usuario Invitado";
            LabelGender.Text = "No disponible";
        }
    }

    private void OnEditEmergencyContactClicked(object sender, EventArgs e)
    {
        _isEditingEmergencyContact = true;
        SetEmergencyFieldsEnabled(true);
        EntryEmergencyName.Focus();
    }

    private async void OnSaveEmergencyContactClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(EntryEmergencyName.Text) ||
            string.IsNullOrWhiteSpace(EntryEmergencyPhone.Text))
        {
            await DisplayAlertAsync("Datos incompletos", "Completa el contacto de emergencia.", "OK");
            return;
        }

        _isEditingEmergencyContact = false;
        SetEmergencyFieldsEnabled(false);
        await DisplayAlertAsync("Guardado", "Contacto de emergencia actualizado correctamente.", "OK");
    }

    private void SetEmergencyFieldsEnabled(bool enabled)
    {
        EntryEmergencyName.IsEnabled = enabled;
        EntryEmergencyPhone.IsEnabled = enabled;
    }

    private async void OnLogoutTapped(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlertAsync("Cerrar sesión", "¿Seguro que deseas cerrar tu turno?", "Sí", "Cancelar");
        if (confirm)
        {
            SessionService.UsuarioActual = null!;
            SessionService.NombreUsuario = string.Empty;

            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}