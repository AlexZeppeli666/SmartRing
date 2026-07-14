using SQLite;

namespace Smart_Ring.Models;

[Table("EmergencyContacts")]
public class EmergencyContactModel
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    // Cambia [Indexed, Unique] por solo [Unique]
    [Unique]
    public int UserId { get; set; }

    [MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;
}