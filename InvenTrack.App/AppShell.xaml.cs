using Inventrack.App.Views;

namespace Inventrack.App
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(UserEditPage), typeof(UserEditPage));
        }
    }
}
