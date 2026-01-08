using Inventrack.App.Services;
using Inventrack.App.Services.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Inventrack.App.ViewModels;
public sealed class LoginViewModel : INotifyPropertyChanged
{
    private readonly IAuthService _auth;
    private readonly ISessionService _session;

    public event PropertyChangedEventHandler? PropertyChanged;

    private string _email = "";
    public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }

    private string _password = "";
    public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }

    private bool _isBusy;
    public bool IsBusy { get => _isBusy; set { _isBusy = value; OnPropertyChanged(); ((Command)LoginCommand).ChangeCanExecute(); } }

    private string _error = "";
    public string Error { get => _error; set { _error = value; OnPropertyChanged(); } }

    public ICommand LoginCommand { get; }

    public LoginViewModel(IAuthService auth, ISessionService session)
    {
        _auth = auth;
        _session = session;

        LoginCommand = new Command(async () => await LoginAsync(), () => !IsBusy);
    }

    private async Task LoginAsync()
    {
        Error = "";

        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            Error = "Introduce email y contraseña.";
            return;
        }

        try
        {
            IsBusy = true;

            var res = await _auth.LoginAsync(Email.Trim(), Password);

            if (res.Usuario.Activo.HasValue && res.Usuario.Activo.Value == false)
            {
                Error = "Usuario desactivado.";
                return;
            }

            await _session.SetSessionAsync(res.Token, res.Usuario);

            await Shell.Current.GoToAsync("//main");
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

