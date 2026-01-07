using InvenTrack.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace InvenTrack.ViewModels;
public sealed class LoginViewModel : INotifyPropertyChanged
{
    private readonly IAuthService _auth;
    private readonly ISessionService _session;

    private string _email = "";
    private string _password = "";
    private bool _isBusy;
    private string _errorMessage = "";

    public event PropertyChangedEventHandler? PropertyChanged;

    public LoginViewModel(IAuthService auth, ISessionService session)
    {
        _auth = auth;
        _session = session;

        LoginCommand = new Command(async () => await LoginAsync(), () => !IsBusy);
    }

    public string Email
    {
        get => _email;
        set { _email = value; OnPropertyChanged(); }
    }

    public string Password
    {
        get => _password;
        set { _password = value; OnPropertyChanged(); }
    }

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            OnPropertyChanged();
            ((Command)LoginCommand).ChangeCanExecute();
        }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set { _errorMessage = value; OnPropertyChanged(); }
    }

    public ICommand LoginCommand { get; }

    private async Task LoginAsync()
    {
        ErrorMessage = "";

        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Introduce email y contraseña.";
            return;
        }

        try
        {
            IsBusy = true;

            var res = await _auth.LoginAsync(Email.Trim(), Password);
            if (res.Usuario.Activo.HasValue && res.Usuario.Activo.Value == false)
            {
                ErrorMessage = "Usuario desactivado. Contacta con un administrador.";
                return;
            }

            await _session.SetSessionAsync(res.Token, res.Usuario);

            // Navegación según rol (ajusta los nombres de rutas a tus páginas Shell)
            var route = res.Usuario.RolId switch
            {
                1 => "//cliente/home",     // Cliente
                2 => "//repartidor/home",  // Repartidor
                3 => "//almacen/home",     // Almacenero
                4 => "//admin/home",       // Admin
                _ => "//cliente/home"
            };

            await Shell.Current.GoToAsync(route);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

