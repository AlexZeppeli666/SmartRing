using System.Runtime.Versioning;
using Smart_Ring.Services;
using Smart_Ring.Models;

[assembly: SupportedOSPlatform("Android21.0")]

namespace Smart_Ring.Views;

public partial class ProfilePage : ContentPage
{
    private bool _isEditingEmergencyContact;
    private readonly DatabaseService _databaseService;

    public ProfilePage()
    {
        InitializeComponent();
        _databaseService = new DatabaseService();
        SetEmergencyFieldsEnabled(false);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (SessionService.UsuarioActual != null)
        {
            var user = SessionService.UsuarioActual;
            string nombreCompleto = $"{user.FirstName} {user.LastNamePaternal} {user.LastNameMaternal}".Trim();
            LabelName.Text = string.IsNullOrWhiteSpace(nombreCompleto) ? user.Username : nombreCompleto;
            LabelGender.Text = !string.IsNullOrWhiteSpace(user.Gender) ? user.Gender : "No especificado";

            // Cargar contacto exclusivo de este usuario
            await LoadEmergencyContactAsync(user.Id);
        }
        else
        {
            LabelName.Text = "Usuario Invitado";
            LabelGender.Text = "No disponible";
            ClearEmergencyFields();
        }
    }

    private async Task LoadEmergencyContactAsync(int userId)
    {
        var contact = await _databaseService.GetEmergencyContactByUserIdAsync(userId);
        if (contact != null)
        {
            EntryEmergencyName.Text = contact.FullName;
            EntryEmergencyPhone.Text = contact.PhoneNumber;
        }
        else
        {
            ClearEmergencyFields();
        }
    }

    private void OnEditEmergencyContactClicked(object sender, EventArgs e)
    {
        if (SessionService.UsuarioActual == null) return;

        _isEditingEmergencyContact = true;
        SetEmergencyFieldsEnabled(true);
        EntryEmergencyName.Focus();
    }

    private async void OnSaveEmergencyContactClicked(object sender, EventArgs e)
    {
        if (SessionService.UsuarioActual == null)
        {
            await DisplayAlertAsync("Error", "No hay una sesión de usuario activa.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(EntryEmergencyName.Text) || string.IsNullOrWhiteSpace(EntryEmergencyPhone.Text))
        {
            await DisplayAlertAsync("Datos incompletos", "Completa el contacto de emergencia.", "OK");
            return;
        }

        var emergencyContact = new EmergencyContactModel
        {
            UserId = SessionService.UsuarioActual.Id,
            FullName = EntryEmergencyName.Text.Trim(),
            PhoneNumber = EntryEmergencyPhone.Text.Trim()
        };

        // Guarda o actualiza según corresponda
        int result = await _databaseService.SaveOrUpdateEmergencyContactAsync(emergencyContact);

        if (result > 0)
        {
            _isEditingEmergencyContact = false;
            SetEmergencyFieldsEnabled(false);
            await DisplayAlertAsync("Guardado", "Contacto de emergencia actualizado correctamente.", "OK");
        }
        else
        {
            await DisplayAlertAsync("Error", "No se pudieron guardar los cambios en la base de datos.", "OK");
        }
    }

    private void SetEmergencyFieldsEnabled(bool enabled)
    {
        EntryEmergencyName.IsEnabled = enabled;
        EntryEmergencyPhone.IsEnabled = enabled;
    }

    private void ClearEmergencyFields()
    {
        EntryEmergencyName.Text = string.Empty;
        EntryEmergencyPhone.Text = string.Empty;
    }

    private async void OnLogoutTapped(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Cerrar sesión", "¿Seguro que deseas cerrar tu turno?", "Sí", "Cancelar");
        if (confirm)
        {
            SessionService.UsuarioActual = null!;
            SessionService.NombreUsuario = string.Empty;
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}