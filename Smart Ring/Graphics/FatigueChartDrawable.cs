using Microsoft.Maui.Graphics;

namespace Smart_Ring.Graphics;

/// <summary>
/// Dibuja la gráfica de líneas "Historial de Fatiga" del HOME usando Microsoft.Maui.Graphics
/// (no requiere librerías externas de gráficas).
/// </summary>
public class FatigueChartDrawable : IDrawable
{
    public List<double> Values { get; set; } = new() { 8, 18, 20, 33, 36 };
    public List<string> Labels { get; set; } = new() { "ene 2021", "ene 2022", "ene 2023", "ene 2024", "ene 2025" };
    public Color LineColor { get; set; } = Color.FromArgb("#8A72D6");
    public Color GridColor { get; set; } = Color.FromArgb("#E3DEF2");
    public Color LabelColor { get; set; } = Color.FromArgb("#9089A6");

    public void Draw(ICanvas canvas, RectF rect)
    {
        if (Values.Count < 2) return;

        const float paddingLeft = 32, paddingBottom = 22, paddingTop = 8, paddingRight = 8;
        float chartWidth = rect.Width - paddingLeft - paddingRight;
        float chartHeight = rect.Height - paddingTop - paddingBottom;

        double maxVal = Math.Max(40, Values.Max());

        // Líneas de cuadrícula horizontales + etiquetas del eje Y
        canvas.StrokeColor = GridColor;
        canvas.StrokeSize = 1;
        canvas.FontColor = LabelColor;
        canvas.FontSize = 10;

        for (int i = 0; i <= 4; i++)
        {
            double val = i * (maxVal / 4);
            float y = paddingTop + chartHeight - (float)(val / maxVal * chartHeight);
            canvas.DrawLine(paddingLeft, y, rect.Width - paddingRight, y);
            canvas.DrawString(((int)val).ToString(), 0, y - 6, paddingLeft - 6, 12,
                HorizontalAlignment.Right, VerticalAlignment.Center);
        }

        // Línea de datos
        canvas.StrokeColor = LineColor;
        canvas.StrokeSize = 2.5f;
        var path = new PathF();
        for (int i = 0; i < Values.Count; i++)
        {
            float x = paddingLeft + (chartWidth * i / (Values.Count - 1));
            float y = paddingTop + chartHeight - (float)(Values[i] / maxVal * chartHeight);
            if (i == 0) path.MoveTo(x, y); else path.LineTo(x, y);
        }
        canvas.DrawPath(path);

        // Puntos
        canvas.FillColor = LineColor;
        for (int i = 0; i < Values.Count; i++)
        {
            float x = paddingLeft + (chartWidth * i / (Values.Count - 1));
            float y = paddingTop + chartHeight - (float)(Values[i] / maxVal * chartHeight);
            canvas.FillCircle(x, y, 3.5f);
        }

        // Etiquetas del eje X
        canvas.FontColor = LabelColor;
        canvas.FontSize = 9;
        for (int i = 0; i < Labels.Count; i++)
        {
            float x = paddingLeft + (chartWidth * i / (Labels.Count - 1));
            canvas.DrawString(Labels[i], x - 26, rect.Height - paddingBottom + 4, 52, 14,
                HorizontalAlignment.Center, VerticalAlignment.Top);
        }
    }
}
