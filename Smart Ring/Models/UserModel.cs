using SQLite;

namespace Smart_Ring.Models;

[Table("Users")]
public class UserModel
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string LastNamePaternal { get; set; } = string.Empty;

    [MaxLength(100)]
    public string LastNameMaternal { get; set; } = string.Empty;

    [Unique, MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    // NUEVO CAMPO: Guardará el género seleccionado
    [MaxLength(20)]
    public string Gender { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}