using DIABEX.Models;
using DIABEX.Services;

namespace DIABEX.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
    }

    private async void OnRegisterUserClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NombreEntry.Text) ||
            string.IsNullOrWhiteSpace(EmailEntry.Text) ||
            string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            await DisplayAlert("Error", "Por favor completa todos los campos", "OK");
            return;
        }

        if (PasswordEntry.Text != ConfirmPasswordEntry.Text)
        {
            await DisplayAlert("Error", "Las contraseñas no coinciden", "OK");
            return;
        }

        try
        {
            // 1. Crear el usuario en Autenticación de Supabase
            var response = await SupabaseService.Instance.Auth.SignUp(EmailEntry.Text, PasswordEntry.Text);

            // 2. Insertar los datos en la tabla 'usuarios'
            if (response.User != null)
            {
                var nuevoUsuario = new Usuario
                {
                    NombreCompleto = NombreEntry.Text,
                    Correo = EmailEntry.Text,
                    Contrasena = PasswordEntry.Text,
                    Rol = "usuario"
                };

                await SupabaseService.Instance.From<Usuario>().Insert(nuevoUsuario);
            }

            await DisplayAlert("Éxito", "Cuenta creada correctamente.", "OK");

            // Guardamos la bandera en Preferences para que el Dashboard muestre la bienvenida
            Preferences.Default.Set("MostrarBienvenida", true);

            Application.Current.MainPage = new AppShell();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error de Registro", ex.Message, "OK");
        }
    }

    private async void OnRegresarClicked(object sender, EventArgs e)
    {
        if (Navigation.NavigationStack.Count > 1)
        {
            await Navigation.PopAsync();
        }
        else
        {
            Application.Current.MainPage = new AppShell();
        }
    }

    private void OnTogglePasswordClicked(object sender, EventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
        TogglePasswordButton.Source = PasswordEntry.IsPassword ? "ojocerrado.png" : "ojoabierto.png";
    }

    private void OnToggleConfirmPasswordClicked(object sender, EventArgs e)
    {
        ConfirmPasswordEntry.IsPassword = !ConfirmPasswordEntry.IsPassword;
        ToggleConfirmPasswordButton.Source = ConfirmPasswordEntry.IsPassword ? "ojocerrado.png" : "ojoabierto.png";
    }
}