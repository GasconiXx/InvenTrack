using InvenTrack.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace InvenTrack.ViewModels;
public sealed class MainViewModel : INotifyPropertyChanged
{
    private readonly ISessionService _session;
    private readonly IPackagesService _packagesService; // el que llame a tu API

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<MenuItemVm> MenuItems { get; } = new();
    public ObservableCollection<ListItemVm> Items { get; } = new();

    private MenuItemVm? _selectedMenuItem;
    public MenuItemVm? SelectedMenuItem
    {
        get => _selectedMenuItem;
        set
        {
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

    public MainViewModel(ISessionService session, IPackagesService packagesService)
    {
        _session = session;
        _packagesService = packagesService;

        LogoutCommand = new Command(async () =>
        {
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

        // Ejemplo: ajusta IDs a tus roles reales
        if (rolId == 1) // Cliente
        {
            MenuItems.Add(new() { Key = "my_packages", Title = "Mis paquetes" });
        }
        else if (rolId == 2) // Repartidor
        {
            MenuItems.Add(new() { Key = "assigned", Title = "Asignados" });
        }
        else if (rolId == 3) // Almacén
        {
            MenuItems.Add(new() { Key = "warehouse", Title = "Paquetes en almacén" });
        }
        else if (rolId == 4) // Admin
        {
            MenuItems.Add(new() { Key = "all_packages", Title = "Todos los paquetes" });
            MenuItems.Add(new() { Key = "users", Title = "Usuarios" });
        }

        // Selecciona primera sección por defecto
        SelectedMenuItem = MenuItems.FirstOrDefault();
    }

    private async Task LoadSectionAsync(MenuItemVm? section)
    {
        if (section is null) return;

        CurrentSectionTitle = section.Title;
        Items.Clear();

        // Aquí conectas con tu API según sección
        switch (section.Key)
        {
            case "my_packages":
                {
                    var res = await _packagesService.GetMyPackagesAsync();
                    foreach (var p in res)
                        Items.Add(new() { MainText = p.Codigo, SubText = p.Estado });
                    break;
                }
            case "assigned":
                {
                    var res = await _packagesService.GetAssignedAsync();
                    foreach (var p in res)
                        Items.Add(new() { MainText = p.Codigo, SubText = p.Estado });
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
                        Items.Add(new() { MainText = p.Codigo, SubText = p.Estado });
                    break;
                }
            case "users":
                {
                    // opcional: listado simple de usuarios para admin
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
        public object? Raw { get; init; } // opcional: referencia al modelo real
    }
}

