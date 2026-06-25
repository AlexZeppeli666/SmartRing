using Microsoft.Maui.Graphics;

namespace Smart_Ring.Graphics;

/// <summary>
/// Dibuja el medidor semicircular (gauge) del "Índice de Alerta", con una aguja
/// que apunta según el porcentaje de capacidad operativa (0.0 a 1.0).
/// </summary>
public class AlertGaugeDrawable : IDrawable
{
    public double Percentage { get; set; } = 0.88;
    public Color TrackColor { get; set; } = Color.FromArgb("#D9CCF5");
    public Color NeedleColor { get; set; } = Color.FromArgb("#8A72D6");

    public void Draw(ICanvas canvas, RectF rect)
    {
        float cx = rect.Width / 2f;
        float cy = rect.Height - 6f;
        float radius = Math.Min(rect.Width / 2f, rect.Height) - 16f;
        if (radius <= 0) return;

        double pct = Math.Clamp(Percentage, 0, 1);

        // Pista de fondo: semicírculo trazado punto a punto (de 180° a 0°, pasando por arriba)
        canvas.StrokeColor = TrackColor;
        canvas.StrokeSize = 22;
        canvas.StrokeLineCap = LineCap.Round;

        var track = new PathF();
        const int segments = 48;
        for (int i = 0; i <= segments; i++)
        {
            double t = (double)i / segments;
            double angle = Math.PI - t * Math.PI;
            float x = cx + (float)(Math.Cos(angle) * radius);
            float y = cy - (float)(Math.Sin(angle) * radius);
            if (i == 0) track.MoveTo(x, y); else track.LineTo(x, y);
        }
        canvas.DrawPath(track);

        // Aguja
        double needleAngle = Math.PI - pct * Math.PI;
        float needleLength = radius - 12;
        float nx = cx + (float)(Math.Cos(needleAngle) * needleLength);
        float ny = cy - (float)(Math.Sin(needleAngle) * needleLength);

        canvas.StrokeColor = NeedleColor;
        canvas.StrokeSize = 6;
        canvas.StrokeLineCap = LineCap.Round;
        canvas.DrawLine(cx, cy, nx, ny);

        canvas.FillColor = NeedleColor;
        canvas.FillCircle(cx, cy, 6);
    }
}
