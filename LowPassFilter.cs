namespace MawiCompass;

public sealed class LowPassFilter
{
    private readonly double _alpha;
    private double? _prev;

    public LowPassFilter(double alpha = 0.15) => _alpha = alpha;

    // Heading-aware smoothing with unwrap around 0/360
    public double Step(double value)
    {
        if (_prev is null) { _prev = value; return value; }
        var prev = _prev.Value;

        var delta = value - prev;
        if (delta > 180) value -= 360;
        if (delta < -180) value += 360;

        var y = _alpha * value + (1 - _alpha) * prev;
        if (y < 0) y += 360;
        if (y >= 360) y -= 360;

        _prev = y;
        return y;
    }
}
