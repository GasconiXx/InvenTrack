using Inventrack.App.Models.Dtos.Users;
using Inventrack.App.ViewModels;

namespace Inventrack.App.Views;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _vm;
    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.RefreshForCurrentUser();
        UpdateNewUserButtonVisibility();
    }

    private async void OnItemSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selected = e.CurrentSelection?.FirstOrDefault();
        if (selected is not MainViewModel.ListItemVm row) return;

        if (sender is CollectionView cv) cv.SelectedItem = null;

        if (_vm.SelectedMenuItem?.Key != "users") return;

        if (row.Raw is Inventrack.App.Models.Dtos.Users.UserListItemDto u)
            await Shell.Current.GoToAsync($"{nameof(UserEditPage)}?id={u.UsuarioId}");
    }
    private async void OnNewUserClicked(object sender, EventArgs e)
    {
        if (_vm.SelectedMenuItem?.Key != "users") return;
        await Shell.Current.GoToAsync($"{nameof(UserEditPage)}?id=0");
    }
    private void UpdateNewUserButtonVisibility()
    {
        NewUserButton.IsVisible = _vm.SelectedMenuItem?.Key == "users";
    }

    private void OnMenuSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        UpdateNewUserButtonVisibility();
    }
}
