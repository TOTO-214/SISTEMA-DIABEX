namespace DIABEX
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Mantiene activa la barra para el botón de hamburguesa pero elimina sombras
            Shell.SetNavBarIsVisible(this, true);
        }
    }
}