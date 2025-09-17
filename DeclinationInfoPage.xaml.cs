using Microsoft.Maui.ApplicationModel;

namespace MawiCompass;

public partial class DeclinationInfoPage : ContentPage
{
    public DeclinationInfoPage() => InitializeComponent();

    private async void OpenDeclinationMap(object? sender, EventArgs e)
        => await Launcher.OpenAsync(new Uri("https://www.magnetic-declination.com/"));

    private async void OpenNoaa(object? sender, EventArgs e)
        => await Launcher.OpenAsync(new Uri("https://www.ngdc.noaa.gov/geomag/calculators/magcalc.shtml#declination"));
}
