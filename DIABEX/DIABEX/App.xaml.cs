using DIABEX.Services;
using DIABEX.Views;

namespace DIABEX;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Inicializar Supabase al arrancar
        _ = SupabaseService.InitializeAsync();

        // Cargar primero la pantalla de Bienvenida envolviéndola en una NavigationPage
        MainPage = new NavigationPage(new BienvenidaPage());
    }
}