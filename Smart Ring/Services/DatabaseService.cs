using SQLite;
using Smart_Ring.Models;

namespace Smart_Ring.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection? _database;

    public DatabaseService()
    {
    }

    private async Task InitAsync()
    {
        if (_database is not null)
            return;

        _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        await _database.CreateTableAsync<UserModel>();
    }

    public async Task<int> RegisterUserAsync(UserModel user)
    {
        await InitAsync();

        // Verificar primero si el nombre de usuario ya está tomado
        var existingUser = await _database!.Table<UserModel>()
                                           .Where(u => u.Username == user.Username)
                                           .FirstOrDefaultAsync();
        if (existingUser != null)
        {
            return -1; // Código de error personalizado: Usuario duplicado
        }

        return await _database.InsertAsync(user);
    }

    // Método para Validar el Login (Read)
    public async Task<UserModel?> ValidateLoginAsync(string username, string password)
    {
        await InitAsync();

        return await _database!.Table<UserModel>()
                               .Where(u => u.Username == username && u.Password == password)
                               .FirstOrDefaultAsync();
    }
}