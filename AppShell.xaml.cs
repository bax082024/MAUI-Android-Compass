namespace MawiCompass
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(DeclinationInfoPage), typeof(DeclinationInfoPage));

        }
    }
}
