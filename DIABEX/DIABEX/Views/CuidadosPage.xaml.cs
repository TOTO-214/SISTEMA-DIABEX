namespace DIABEX.Views;

public partial class CuidadosPage : ContentPage
{
    // Constructor por defecto
    public CuidadosPage() : this(0)
    {
    }

    // Constructor que recibe el puntaje calculado en ChequeatePage
    public CuidadosPage(int puntaje)
    {
        InitializeComponent();
        CargarRecomendaciones(puntaje);
    }

    private void CargarRecomendaciones(int puntaje)
    {
        if (puntaje <= 2)
        {
            // --- NIVEL 1: TOTALMENTE SANO ---
            BannerEstado.BackgroundColor = Color.FromArgb("#2E7D32"); // Verde
            TituloEstadoLabel.Text = "¡Excelente Estado de Salud!";
            SubtituloEstadoLabel.Text = "Tus hábitos actuales son óptimos. Mantén estas recomendaciones para prevenir riesgos a futuro.";

            AlimentacionLabel.Text = "• Prioriza alimentos integrales, verduras de hoja verde y proteínas magras.\n• Reduce el consumo de azúcares refinados y bebidas azucaradas.\n• Mantén una correcta hidratación tomando al menos 2 litros de agua al día.";
            EjercicioLabel.Text = "• Realiza al menos 150 minutos de actividad física moderada a la semana (caminatas, ciclismo, etc.).\n• Procura dormir entre 7 y 8 horas diarias para regular el metabolismo.";
            MonitoreoLabel.Text = "• Realiza un chequeo general de glucosa en sangre una vez al año como medida preventiva.";
        }
        else if (puntaje <= 6)
        {
            // --- NIVEL 2: RIESGO BAJO / MODERADO ---
            BannerEstado.BackgroundColor = Color.FromArgb("#F57C00"); // Naranja
            TituloEstadoLabel.Text = "Riesgo Bajo / Moderado";
            SubtituloEstadoLabel.Text = "Presentas algunos factores a los que debes poner atención para evitar complicaciones.";

            AlimentacionLabel.Text = "• Controla las porciones de carbohidratos usando el método del plato (50% verduras, 25% proteína, 25% carbohidratos).\n• Evita ultraprocesados y comida rápida.\n• Prefiere frutas enteras en lugar de jugos procesados.";
            EjercicioLabel.Text = "• Camina 15 minutos después de cada comida principal para reducir picos postprandiales.\n• Reduce el tiempo de sedentarismo o periodos largos sentado.";
            MonitoreoLabel.Text = "• Realiza pruebas de glucosa en ayunas cada 6 meses.\n• Registra cualquier síntoma inusual como fatiga constante o sed excesiva.";
        }
        else
        {
            // --- NIVEL 3: RIESGO ALTO ---
            BannerEstado.BackgroundColor = Color.FromArgb("#C62828"); // Rojo
            TituloEstadoLabel.Text = "Riesgo Alto Detectado";
            SubtituloEstadoLabel.Text = "Se identificaron varios indicadores de alerta. Te aconsejamos tomar medidas inmediatas.";

            AlimentacionLabel.Text = "• Reduce drásticamente el consumo de carbohidratos simples, harinas blancas y azúcares.\n• Diseña un plan de alimentación guiado por un profesional en nutrición clínica.";
            EjercicioLabel.Text = "• Comienza con actividad física de bajo impacto (caminatas diarias) con previa autorización médica.\n• Mantén un control de peso corporal continuo.";
            MonitoreoLabel.Text = "• Agenda una consulta con un médico o endocrinólogo lo antes posible.\n• Realízate exámenes de Glucosa en Ayunas y Hemoglobina Glicada (HbA1c).";
        }
    }

    private async void OnVolverClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}