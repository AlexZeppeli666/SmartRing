using SQLite;
using Smart_Ring.Models;

namespace Smart_Ring.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection? _database;

    private async Task InitAsync()
    {
        if (_database is not null)
            return;

        _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        await _database.CreateTableAsync<UserModel>();
        // Inicializar la nueva tabla
        await _database.CreateTableAsync<EmergencyContactModel>();
    }

    // Obtener contacto por Id de usuario
    public async Task<EmergencyContactModel?> GetEmergencyContactByUserIdAsync(int userId)
    {
        await InitAsync();
        return await _database!.Table<EmergencyContactModel>()
                               .Where(c => c.UserId == userId)
                               .FirstOrDefaultAsync();
    }

    // Insertar o Modificar (Upsert)
    public async Task<int> SaveOrUpdateEmergencyContactAsync(EmergencyContactModel contact)
    {
        await InitAsync();

        // Buscar si ya existe un registro para este usuario
        var existingContact = await _database!.Table<EmergencyContactModel>()
                                              .Where(c => c.UserId == contact.UserId)
                                              .FirstOrDefaultAsync();

        if (existingContact == null)
        {
            // Insertar
            return await _database.InsertAsync(contact);
        }
        else
        {
            // Modificar conservando la PK original
            existingContact.FullName = contact.FullName;
            existingContact.PhoneNumber = contact.PhoneNumber;
            return await _database.UpdateAsync(existingContact);
        }
    }

    public async Task<int> RegisterUserAsync(UserModel user)
    {
        await InitAsync();
        var existingUser = await _database!.Table<UserModel>().Where(u => u.Username == user.Username).FirstOrDefaultAsync();
        if (existingUser != null) return -1;
        return await _database.InsertAsync(user);
    }

    public async Task<UserModel?> ValidateLoginAsync(string username, string password)
    {
        await InitAsync();
        return await _database!.Table<UserModel>().Where(u => u.Username == username && u.Password == password).FirstOrDefaultAsync();
    }
}