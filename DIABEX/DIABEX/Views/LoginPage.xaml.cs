using DIABEX.Services;

namespace DIABEX.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginUserClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(EmailEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            await DisplayAlert("Error", "Ingresa tu correo y contraseña", "OK");
            return;
        }

        try
        {
            await SupabaseService.Instance.Auth.SignInWithPassword(EmailEntry.Text, PasswordEntry.Text);
            Application.Current.MainPage = new AppShell();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error de Inicio de Sesión", ex.Message, "OK");
        }
    }

    private async void OnRegresarClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private void OnTogglePasswordClicked(object sender, EventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
        TogglePasswordButton.Source = PasswordEntry.IsPassword ? "ojocerrado.png" : "ojoabierto.png";
    }
}