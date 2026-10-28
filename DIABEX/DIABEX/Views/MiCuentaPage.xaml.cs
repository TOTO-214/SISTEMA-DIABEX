using DIABEX.Models;
using DIABEX.Services;

namespace DIABEX.Views;

public partial class MiCuentaPage : ContentPage
{
    public MiCuentaPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarDatosUsuarioAsync();
    }

    private async Task CargarDatosUsuarioAsync()
    {
        try
        {
            var currentUser = SupabaseService.Instance.Auth.CurrentUser;

            if (currentUser != null)
            {
                EmailLabel.Text = currentUser.Email;

                var respuesta = await SupabaseService.Instance
                    .From<ResultadoQuiz>()
                    .Where(x => x.UserId == currentUser.Id)
                    .Get();

                var listaResultados = respuesta.Models;

                TotalTestsLabel.Text = $"Pruebas realizadas: {listaResultados.Count}";
                HistorialCollectionView.ItemsSource = listaResultados.OrderByDescending(x => x.Fecha);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudieron cargar los datos: {ex.Message}", "OK");
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

    private async void OnCerrarSesionClicked(object sender, EventArgs e)
    {
        bool confirmar = await DisplayAlert("Cerrar Sesión", "¿Estás seguro de que deseas salir?", "Sí", "No");
        if (confirmar)
        {
            await SupabaseService.Instance.Auth.SignOut();
            Application.Current.MainPage = new NavigationPage(new BienvenidaPage());
        }
    }
}