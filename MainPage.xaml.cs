using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Storage;

namespace MawiCompass;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
    // Preferences keys
    private const string PrefUseTrue = "use_true_north";
    private const string PrefDeclDeg = "declination_deg";

    private readonly LowPassFilter _filter = new(0.15);
    private double _magneticHeading;   // smoothed magnetic
    private bool _useTrueNorth;
    private double _declinationDeg;    // east +, west -

    private string _status = "Tip: move phone in a figure-8 to calibrate.";
    private bool _interference;
    private readonly double _minEarth = 25.0; // µT
    private readonly double _maxEarth = 65.0; // µT

    public CompassDrawable CompassDrawable { get; } = new();

    public string HeadingText
    {
        get
        {
            var outDeg = OutputHeadingDeg();
            var mode = _useTrueNorth ? "True" : "Magnetic";
            return $"{outDeg:0}°  ({mode})";
        }
    }

    public string StatusText { get => _status; private set { _status = value; OnPropertyChanged(); } }

    public event PropertyChangedEventHandler? PropertyChanged;

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;

        // load prefs
        _useTrueNorth = Preferences.Get(PrefUseTrue, false);
        _declinationDeg = Preferences.Get(PrefDeclDeg, 0.0);

        // reflect in UI
        TrueSwitch.IsToggled = _useTrueNorth;
        DeclinationEntry.Text = _declinationDeg.ToString("0.##");

        CompassView.Drawable = CompassDrawable;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (!Compass.IsMonitoring)
            Compass.Start(SensorSpeed.Game);
        Compass.ReadingChanged += OnHeadingChanged;

        if (!Magnetometer.IsMonitoring)
            Magnetometer.Start(SensorSpeed.Game);
        Magnetometer.ReadingChanged += OnMagChanged;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        Compass.ReadingChanged -= OnHeadingChanged;
        if (Compass.IsMonitoring) Compass.Stop();

        Magnetometer.ReadingChanged -= OnMagChanged;
        if (Magnetometer.IsMonitoring) Magnetometer.Stop();

        // save prefs
        Preferences.Set(PrefUseTrue, _useTrueNorth);
        Preferences.Set(PrefDeclDeg, _declinationDeg);
    }

    private void OnHeadingChanged(object? sender, CompassChangedEventArgs e)
    {
        var raw = e.Reading.HeadingMagneticNorth; // 0..360
        _magneticHeading = _filter.Step(raw);

        CompassDrawable.HeadingDeg = (float)OutputHeadingDeg();

        OnPropertyChanged(nameof(HeadingText));
        CompassView.Invalidate();
    }

    private void OnMagChanged(object? sender, MagnetometerChangedEventArgs e)
    {
        var v = e.Reading.MagneticField;
        var mag = Math.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);

        bool inf = mag < _minEarth || mag > _maxEarth;
        if (inf != _interference)
        {
            _interference = inf;
            StatusText = _interference
                ? "Magnetic interference nearby (metal/electronics). Step away & recalibrate."
                : "Good signal. If it drifts, do a figure-8 to recalibrate.";
        }
    }

    // === UI handlers ===
    private void OnTrueToggled(object? sender, ToggledEventArgs e)
    {
        _useTrueNorth = e.Value;
        Preferences.Set(PrefUseTrue, _useTrueNorth);
        // refresh immediately
        CompassDrawable.HeadingDeg = (float)OutputHeadingDeg();
        OnPropertyChanged(nameof(HeadingText));
        CompassView.Invalidate();
    }

    private void OnDeclinationChanged(object? sender, TextChangedEventArgs e)
    {
        if (double.TryParse(e.NewTextValue?.Replace(',', '.'), System.Globalization.NumberStyles.Float,
            System.Globalization.CultureInfo.InvariantCulture, out var val))
        {
            // clamp to reasonable range
            _declinationDeg = Math.Clamp(val, -30.0, 30.0);
            Preferences.Set(PrefDeclDeg, _declinationDeg);
            CompassDrawable.HeadingDeg = (float)OutputHeadingDeg();
            OnPropertyChanged(nameof(HeadingText));
            CompassView.Invalidate();
        }
    }

    // === helpers ===
    private double OutputHeadingDeg()
    {
        var h = _magneticHeading;
        if (_useTrueNorth)
            h = Wrap360(h + _declinationDeg);
        return h;
    }

    private static double Wrap360(double deg)
    {
        deg %= 360.0;
        if (deg < 0) deg += 360.0;
        return deg;
    }

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
