namespace DIABEX.Views;

public partial class BienvenidaPage : ContentPage
{
    public BienvenidaPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage());
    }
}