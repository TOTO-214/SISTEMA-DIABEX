using System.Globalization;
using DIABEX.Services;

namespace DIABEX.Views;

public partial class DashboardPage : ContentPage
{
    public DashboardPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // 1. Cargar fecha actual y saludo de usuario
        CargarInformacionInicial();

        // 2. Verificar si existe la bandera para mostrar la bienvenida
        bool mostrarBienvenida = Preferences.Default.Get("MostrarBienvenida", false);

        if (mostrarBienvenida)
        {
            // Limpiar la bandera para que no vuelva a aparecer
            Preferences.Default.Set("MostrarBienvenida", false);

            await Task.Delay(400);

            await DisplayAlert(
            "¡Bienvenido a DiabeX!",
            "Tu salud es muy importante, por eso hemos creado este espacio para ti, no queremos tener inconvenientes y por eso queremos decirte que esta aplicacion no esta diseñada para dar un resultado tan certero, te recomendamos siempre acudir a tu medico de confianza" +
            "",
            "Comenzar"
             );
        }
    }

    private void CargarInformacionInicial()
    {
        try
        {
            // Formatear la fecha actual (ej. "Miércoles, 16 de septiembre")
            var culturaEs = new CultureInfo("es-ES");
            string fechaFormateada = DateTime.Now.ToString("dddd, d 'de' MMMM", culturaEs);

            if (DateLabel != null)
            {
                DateLabel.Text = char.ToUpper(fechaFormateada[0]) + fechaFormateada.Substring(1);
            }

            // Obtener usuario actual de Supabase
            var usuarioActual = SupabaseService.Instance.Auth.CurrentUser;
            if (usuarioActual != null && GreetingLabel != null)
            {
                string nombre = usuarioActual.UserMetadata?.ContainsKey("full_name") == true
                    ? usuarioActual.UserMetadata["full_name"].ToString()
                    : usuarioActual.Email;

                GreetingLabel.Text = $"¡Hola de nuevo, {nombre}!";
            }
        }
        catch
        {
            if (DateLabel != null) DateLabel.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }

    private async void OnRecordatoriosTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new RecordatoriosPage());
    }

    private async void OnChequeateTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new ChequeatePage());
    }

    private async void OnTemporizadorTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new TemporizadorPostComidaPage());
    }

    private async void OnCalculadoraTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new CalculadoraCarbohidratosPage());
    }

    private async void OnEmergenciaTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new EmergenciaPage());
    }

    private async void OnMiCuentaTapped(object sender, TappedEventArgs e)
    {
        // Reemplaza 'MisResultadosPage' por el nombre exacto de tu vista de resultados
        await Navigation.PushAsync(new MiCuentaPage());
    }
}