using Microsoft.Maui.Graphics;

namespace MawiCompass;

public sealed class CompassDrawable : IDrawable
{
    public float HeadingDeg { get; set; } // 0..360 (magnetic)

    // Palette (tweak freely)
    readonly Color RingColor = Color.FromArgb("#D1D5DB"); // gray-300
    readonly Color TickColor = Color.FromArgb("#D1D5DB");
    readonly Color LabelColor = Color.FromArgb("#F3F4F6"); // near-white
    readonly Color NorthColor = Color.FromArgb("#EF4444"); // red-500
    readonly Color SouthColor = Color.FromArgb("#9CA3AF"); // gray-400
    readonly Color HubColor = Color.FromArgb("#E5E7EB"); // gray-200
    readonly Color StrokeColor = Colors.Black.WithAlpha(0.5f);

    public void Draw(ICanvas canvas, RectF rect)
    {
        float cx = rect.Center.X, cy = rect.Center.Y;
        float r = MathF.Min(rect.Width, rect.Height) * 0.42f;

        // Rotate the rose opposite the heading
        canvas.SaveState();
        canvas.Translate(cx, cy);
        canvas.Rotate(-HeadingDeg);

        // Outer ring
        canvas.StrokeColor = RingColor;
        canvas.StrokeSize = 4;
        canvas.DrawCircle(0, 0, r);

        // Ticks every 30°
        canvas.StrokeColor = TickColor;
        canvas.StrokeSize = 3;
        for (int i = 0; i < 360; i += 30)
        {
            float tick = (i % 90 == 0) ? 18 : 9;
            float a = i * (MathF.PI / 180f);
            float x1 = (r - tick) * MathF.Sin(a);
            float y1 = -(r - tick) * MathF.Cos(a);
            float x2 = r * MathF.Sin(a);
            float y2 = -r * MathF.Cos(a);
            canvas.DrawLine(x1, y1, x2, y2);
        }

        // Needles with a soft shadow for visibility
        canvas.SetShadow(new SizeF(0, 2), 6, Colors.Black.WithAlpha(0.35f));

        // North needle (red)
        float needleLen = r - 14;
        var north = new PathF();
        north.MoveTo(0, -needleLen);
        north.LineTo(12, 18);
        north.LineTo(-12, 18);
        north.Close();
        canvas.FillColor = NorthColor;
        canvas.FillPath(north);
        canvas.StrokeColor = StrokeColor;
        canvas.DrawPath(north);

        // South needle (short, gray)
        var south = new PathF();
        south.MoveTo(0, needleLen * 0.55f);
        south.LineTo(10, -10);
        south.LineTo(-10, -10);
        south.Close();
        canvas.FillColor = SouthColor;
        canvas.FillPath(south);
        canvas.StrokeColor = StrokeColor;
        canvas.DrawPath(south);

        // Hub
        canvas.SetShadow(new SizeF(0, 0), 0, Colors.Transparent);
        canvas.FillColor = HubColor;
        canvas.FillCircle(0, 0, 5);

        canvas.RestoreState(); // stop rotating for labels

        // Cardinal labels (don’t rotate) — pushed farther from ring
        canvas.FontColor = LabelColor;
        canvas.FontSize = 18;
        canvas.DrawString("N", cx, cy - r - 22, HorizontalAlignment.Center);   // was -22
        canvas.DrawString("S", cx, cy + r + 30, HorizontalAlignment.Center);   // was +6
        canvas.DrawString("E", cx + r + 30, cy + 6, HorizontalAlignment.Center); // was +22
        canvas.DrawString("W", cx - r - 30, cy + 6, HorizontalAlignment.Center); // was -22

    }
}
