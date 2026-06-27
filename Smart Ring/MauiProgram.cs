namespace Smart_Ring;
using SQLite;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold", "OpenSansSemibold");
            });

        // 🛠️ SOLUCIÓN: Quitar la línea nativa con la conversión de tipo correcta
#if ANDROID
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderlineNative", (handler, view) =>
        {
            var nativeColor = Microsoft.Maui.Platform.ColorExtensions.ToPlatform(Colors.Transparent);
            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(nativeColor);

            handler.PlatformView.Background = null;
        });
#endif
        builder.Services.AddSingleton<Services.DatabaseService>();
        return builder.Build();
    }
}