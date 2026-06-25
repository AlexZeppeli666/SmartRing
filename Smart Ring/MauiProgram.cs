using Microsoft.Extensions.Logging;

namespace Smart_Ring;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Nota: Microsoft.Maui.Devices.Vibration es un servicio estático
        // (Vibration.Default), no requiere registro en el contenedor de DI.

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
