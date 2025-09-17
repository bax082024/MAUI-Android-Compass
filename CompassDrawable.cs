using Microsoft.Maui.Graphics;

namespace MawiCompass;

public sealed class CompassDrawable : IDrawable
{
    public float HeadingDeg { get; set; } // 0..360 (magnetic)

    // Palette (tweak freely)
    readonly Color RingColor = Color.FromArgb("#D1D5DB"); // gray-300
    readonly Color MajorTick = Color.FromArgb("#F3F4F6"); // near-white
    readonly Color MidTick = Color.FromArgb("#D1D5DB"); // gray-300
    readonly Color MinorTick = Color.FromArgb("#9CA3AF"); // gray-400
    readonly Color LabelColor = Color.FromArgb("#F3F4F6"); // near-white
    readonly Color NorthColor = Color.FromArgb("#EF4444"); // red-500
    readonly Color SouthColor = Color.FromArgb("#9CA3AF"); // gray-400
    readonly Color HubColor = Color.FromArgb("#E5E7EB");
    readonly Color OutlineShadow = Colors.Black.WithAlpha(0.35f);

    public void Draw(ICanvas canvas, RectF rect)
    {
        float cx = rect.Center.X, cy = rect.Center.Y;
        float r = MathF.Min(rect.Width, rect.Height) * 0.42f;

        // === rotate the rose opposite the heading ===
        canvas.SaveState();
        canvas.Translate(cx, cy);
        canvas.Rotate(-HeadingDeg);

        // ---- ring with soft shadow (subtle glow) ----
        canvas.SetShadow(new SizeF(0, 2), 10, OutlineShadow);
        canvas.StrokeColor = RingColor;
        canvas.StrokeSize = 4;
        canvas.DrawCircle(0, 0, r);
        canvas.SetShadow(new SizeF(0, 0), 0, Colors.Transparent);

        // ---- ticks every 10° (minor), 30° (mid), 90° (major) ----
        for (int i = 0; i < 360; i += 10)
        {
            float a = i * (MathF.PI / 180f);
            float len, stroke;
            Color color;

            if (i % 90 == 0) { len = 20; stroke = 3.5f; color = MajorTick; }
            else if (i % 30 == 0) { len = 12; stroke = 3.0f; color = MidTick; }
            else { len = 7; stroke = 2.0f; color = MinorTick; }

            float x1 = (r - len) * MathF.Sin(a);
            float y1 = -(r - len) * MathF.Cos(a);
            float x2 = r * MathF.Sin(a);
            float y2 = -r * MathF.Cos(a);

            canvas.StrokeSize = stroke;
            canvas.StrokeColor = color;
            canvas.DrawLine(x1, y1, x2, y2);
        }

        // ---- needles with shadow ----
        canvas.SetShadow(new SizeF(0, 2), 6, OutlineShadow);

        float needleLen = r - 14;

        // North (red)
        var north = new PathF();
        north.MoveTo(0, -needleLen);
        north.LineTo(12, 18);
        north.LineTo(-12, 18);
        north.Close();
        canvas.FillColor = NorthColor;
        canvas.FillPath(north);
        canvas.StrokeColor = Colors.Black.WithAlpha(0.5f);
        canvas.DrawPath(north);

        // South (short, gray)
        var south = new PathF();
        south.MoveTo(0, needleLen * 0.55f);
        south.LineTo(10, -10);
        south.LineTo(-10, -10);
        south.Close();
        canvas.FillColor = SouthColor;
        canvas.FillPath(south);
        canvas.StrokeColor = Colors.Black.WithAlpha(0.5f);
        canvas.DrawPath(south);

        // Hub
        canvas.SetShadow(new SizeF(0, 0), 0, Colors.Transparent);
        canvas.FillColor = HubColor;
        canvas.FillCircle(0, 0, 5);

        canvas.RestoreState(); // stop rotating for labels

        // ---- cardinal letters (don’t rotate) ----
        canvas.FontColor = LabelColor;
        canvas.FontSize = 18;
        canvas.DrawString("N", cx, cy - r - 22, HorizontalAlignment.Center);
        canvas.DrawString("S", cx, cy + r + 30, HorizontalAlignment.Center);
        canvas.DrawString("E", cx + r + 30, cy + 6, HorizontalAlignment.Center);
        canvas.DrawString("W", cx - r - 30, cy + 6, HorizontalAlignment.Center);
    }
}
