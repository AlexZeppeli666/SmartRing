using Smart_Ring.Models;

namespace Smart_Ring.Services;

public static class SessionService
{
    public static string NombreUsuario { get; set; } = string.Empty;
    public static UserModel UsuarioActual { get; set; }
}