using Inventrack.App.Services;
using Inventrack.App.Services.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Linq;

namespace Inventrack.App.ViewModels;
public sealed class MainViewModel : INotifyPropertyChanged
{
    private readonly ISessionService _session;
    private readonly IPackagesService _packagesService;
    private readonly IUserService _userService;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<MenuItemVm> MenuItems { get; } = new();
    public ObservableCollection<ListItemVm> Items { get; } = new();

    private MenuItemVm? _selectedMenuItem;

    private string? _lastSectionKey;

    public MenuItemVm? SelectedMenuItem
    {
        get => _selectedMenuItem;
        set
        {
            if (_selectedMenuItem?.Key == value?.Key)
                return;

            _selectedMenuItem = value;
            OnPropertyChanged();
            _ = LoadSectionAsync(value);
        }
    }

    private string _currentSectionTitle = "";
    public string CurrentSectionTitle
    {
        get => _currentSectionTitle;
        set { _currentSectionTitle = value; OnPropertyChanged(); }
    }

    public ICommand LogoutCommand { get; }

    public MainViewModel(ISessionService session, IPackagesService packagesService, IUserService userService)
    {
        _session = session;
        _packagesService = packagesService;
        _userService = userService;

        LogoutCommand = new Command(async () =>
        {
            MenuItems.Clear();
            Items.Clear();
            SelectedMenuItem = null;
            CurrentSectionTitle = "";
            await _session.ClearAsync();
            await Shell.Current.GoToAsync("//login");
        });

        BuildMenuForRole();
    }

    private void BuildMenuForRole()
    {
        MenuItems.Clear();

        var user = _session.CurrentUser;
        var rolId = user?.RolId ?? 0;

        if (rolId == 1) // Cliente
        {
            MenuItems.Add(new() { Key = "my_packages", Title = "Mis paquetes" });
        }
        else if (rolId == 3) // Repartidor
        {
            MenuItems.Add(new() { Key = "assigned", Title = "Asignados" });
        }
        else if (rolId == 2) // Almacén
        {
            MenuItems.Add(new() { Key = "warehouse", Title = "Paquetes en almacén" });
        }
        else if (rolId == 4) // Admin
        {
            MenuItems.Add(new() { Key = "all_packages", Title = "Todos los paquetes" });
            MenuItems.Add(new() { Key = "users", Title = "Usuarios" });
        }

        SelectedMenuItem = MenuItems.FirstOrDefault();
    }

    private async Task LoadSectionAsync(MenuItemVm? section)
    {
        if (section is null) return;

        CurrentSectionTitle = section.Title;
        Items.Clear();

        switch (section.Key)
        {
            case "my_packages":
                {
                    var res = await _packagesService.GetMyPackagesAsync();
                    foreach (var p in res)
                        Items.Add(new() { MainText = p.Codigo, SubText = $"{ p.Estado } - {p.Destinatario}" });
                    break;
                }
            case "assigned":
                {
                    var res = await _packagesService.GetMyShipmentsAsync();
                    foreach (var s in res)
                        Items.Add(new()
                        {
                            MainText = s.DireccionDestino,
                            SubText = $"{s.CodigoSeguimiento} · {s.Estado} · Intentos: {s.IntentosEntrega}",
                            Raw = s
                        });
                    break;
                }
            case "warehouse":
                {
                    var res = await _packagesService.GetWarehouseAsync();
                    foreach (var p in res)
                        Items.Add(new() { MainText = p.Codigo, SubText = p.Estado });
                    break;
                }
            case "all_packages":
                {
                    var res = await _packagesService.GetAllAsync();
                    foreach (var p in res)
                        Items.Add(new() { MainText = p.Codigo, SubText = $"{p.Estado} | Remitente: {p.Remitente} | Destinatario: {p.Destinatario}" });
                    break;
                }
            case "users":
                {
                    var res = await _userService.GetAllUsersAsync();
                    foreach (var p in res)
                        Items.Add(new() { MainText = $"{p.Nombre} ({p.Rol})", SubText = $"{p.Email} - {p.Telefono}" });
                    break;
                }
        }
    }

    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public sealed class MenuItemVm
    {
        public string Key { get; init; } = "";
        public string Title { get; init; } = "";
    }

    public sealed class ListItemVm
    {
        public string MainText { get; init; } = "";
        public string SubText { get; init; } = "";
        public object? Raw { get; init; } 
    }

    public void RefreshForCurrentUser()
    {
        if (MenuItems.Count == 0)
            BuildMenuForRole();
    }
}

