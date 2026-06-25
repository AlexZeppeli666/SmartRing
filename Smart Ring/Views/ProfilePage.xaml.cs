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

    private async void OnSaveSexClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Guardado", "Preferencia de sexo actualizada.", "OK");
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
            await DisplayAlert("Datos incompletos", "Completa el contacto de emergencia.", "OK");
            return;
        }

        _isEditingEmergencyContact = false;
        SetEmergencyFieldsEnabled(false);
        await DisplayAlert("Guardado", "Contacto de emergencia actualizado correctamente.", "OK");
    }

    private void SetEmergencyFieldsEnabled(bool enabled)
    {
        EntryEmergencyName.IsEnabled = enabled;
        EntryEmergencyPhone.IsEnabled = enabled;
    }

    private async void OnLogoutTapped(object sender, TappedEventArgs e)
    {
        bool confirm = await DisplayAlert("Cerrar sesión", "¿Seguro que deseas cerrar tu turno?", "Sí", "Cancelar");
        if (confirm)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
