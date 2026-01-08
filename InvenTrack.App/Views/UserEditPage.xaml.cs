using Inventrack.App.Models.Dtos.Users;
using Inventrack.App.Services;
using Inventrack.App.Services.Interfaces;

namespace Inventrack.App.Views;

[QueryProperty(nameof(UserId), "id")]
public partial class UserEditPage : ContentPage
{
    private IUserService? _users;
    private bool _loaded;

    public int UserId { get; set; }

    public UserEditPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_loaded)
            return;

        _loaded = true;

        _users = App.Services.GetRequiredService<IUserService>();

        if (_users is null)
        {
            await DisplayAlert("Error", "No se pudo resolver IUserService.", "OK");
            return;
        }

        ModeTitle.Text = UserId == 0 ? "Crear usuario" : "Editar usuario";
        PasswordEntry.IsVisible = UserId == 0;

        if (UserId > 0)
            await LoadUserAsync(UserId);
    }

    private async Task LoadUserAsync(int id)
    {
        try
        {
            SetBusy(true);

            var u = await _users!.GetByIdAsync(id);

            NombreEntry.Text = u.Nombre;
            EmailEntry.Text = u.Email;
            TelefonoEntry.Text = u.Telefono;
            RolIdEntry.Text = u.RolId.ToString();
            AlmacenIdEntry.Text = u.AlmacenId?.ToString() ?? "";
            ActivoSwitch.IsToggled = u.Activo ?? true;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (_users is null)
            return;

        try
        {
            SetBusy(true);

            var nombre = (NombreEntry.Text ?? "").Trim();
            var email = (EmailEntry.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(email))
            {
                await DisplayAlert("Error", "Nombre y Email son obligatorios.", "OK");
                return;
            }

            if (!int.TryParse(RolIdEntry.Text, out var rolId))
            {
                await DisplayAlert("Error", "RolId inválido.", "OK");
                return;
            }

            int? almacenId = null;
            if (!string.IsNullOrWhiteSpace(AlmacenIdEntry.Text))
            {
                if (!int.TryParse(AlmacenIdEntry.Text, out var aId))
                {
                    await DisplayAlert("Error", "AlmacenId inválido.", "OK");
                    return;
                }
                almacenId = aId;
            }

            var dto = new UserUpsertDto
            {
                Nombre = nombre,
                Email = email,
                Telefono = TelefonoEntry.Text ?? "",
                RolId = rolId,
                Activo = ActivoSwitch.IsToggled,
                AlmacenId = almacenId,
                ContrasenaHash = UserId == 0 ? PasswordEntry.Text : null
            };

            if (UserId == 0)
                await _users.CreateAsync(dto);
            else
                await _users.UpdateAsync(UserId, dto);

            await DisplayAlert("OK", "Usuario guardado correctamente.", "Cerrar");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void SetBusy(bool busy)
    {
        Busy.IsVisible = busy;
        Busy.IsRunning = busy;
        SaveButton.IsEnabled = !busy;
    }
}
