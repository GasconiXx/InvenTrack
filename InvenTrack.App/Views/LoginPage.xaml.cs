using Inventrack.App.ViewModels;

namespace Inventrack.App.Views;

public partial class LoginPage : ContentPage //TODO
{
    public LoginPage(LoginViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
