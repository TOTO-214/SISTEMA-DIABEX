using DIABEX.Models;
using DIABEX.Services;

namespace DIABEX.Views;

public partial class ChequeatePage : ContentPage
{
    public ChequeatePage()
    {
        InitializeComponent();
    }

    private async void OnEvaluarClicked(object sender, EventArgs e)
    {
        int puntaje = 0;

        // Pregunta 1: Antecedentes
        if (P1_Opcion2.IsChecked) puntaje += 1;
        if (P1_Opcion3.IsChecked) puntaje += 3;

        // Pregunta 2: Glucosa
        if (P2_Opcion2.IsChecked) puntaje += 2;
        if (P2_Opcion3.IsChecked) puntaje += 3;
        if (P2_Opcion4.IsChecked) puntaje += 1;

        // Pregunta 3: Sed
        if (P3_Opcion2.IsChecked) puntaje += 1;
        if (P3_Opcion3.IsChecked) puntaje += 2;

        // Pregunta 4: Orinar de noche
        if (P4_Opcion2.IsChecked) puntaje += 1;
        if (P4_Opcion3.IsChecked) puntaje += 2;

        // Pregunta 5: Hambre constante
        if (P5_Opcion2.IsChecked) puntaje += 1;
        if (P5_Opcion3.IsChecked) puntaje += 2;

        // Pregunta 6: Pérdida de peso
        if (P6_Opcion2.IsChecked) puntaje += 1;
        if (P6_Opcion3.IsChecked) puntaje += 2;

        // Pregunta 7: Hormigueo en pies/manos
        if (P7_Opcion2.IsChecked) puntaje += 1;
        if (P7_Opcion3.IsChecked) puntaje += 2;

        // Pregunta 8: Oscurecimiento en cuello/axilas
        if (P8_Opcion2.IsChecked) puntaje += 1;
        if (P8_Opcion3.IsChecked) puntaje += 2;

        // Pregunta 9: Curación de heridas
        if (P9_Opcion2.IsChecked) puntaje += 1;
        if (P9_Opcion3.IsChecked) puntaje += 2;

        // Pregunta 10: Visión borrosa
        if (P10_Opcion2.IsChecked) puntaje += 1;
        if (P10_Opcion3.IsChecked) puntaje += 2;

        // Evaluación del resultado (3 niveles)
        string resultado;
        if (puntaje <= 2)
        {
            resultado = "Totalmente sano: Tus hábitos e indicadores están en excelente estado.";
        }
        else if (puntaje <= 6)
        {
            resultado = "Riesgo Bajo: Mantén tus hábitos saludables y monitorea tus síntomas.";
        }
        else
        {
            resultado = "Riesgo Alto: Consulta a un médico para una revisión detallada.";
        }

        try
        {
            var currentUser = SupabaseService.Instance.Auth.CurrentUser;
            if (currentUser != null)
            {
                var nuevoResultado = new ResultadoQuiz
                {
                    UserId = currentUser.Id,
                    Puntaje = puntaje,
                    ResultadoTexto = resultado,
                    Fecha = DateTime.UtcNow
                };

                // Guardar en la base de datos de Supabase
                await SupabaseService.Instance.From<ResultadoQuiz>().Insert(nuevoResultado);

                // Mostrar la alerta con 'Ver Recomendaciones' (true) y 'OK' (false)
                bool irARecomendaciones = await DisplayAlert(
                    "Resultado",
                    $"Tu puntaje fue: {puntaje}\n\n{resultado}",
                    "Ver Recomendaciones",
                    "OK"
                );

                if (irARecomendaciones)
                {
                    // Se envía el puntaje exacto a CuidadosPage
                    await Navigation.PushAsync(new CuidadosPage(puntaje));
                }
                else
                {
                    // Redirige al usuario fuera de la página tras presionar "OK"
                    await Shell.Current.GoToAsync("..");
                }
            }
            else
            {
                await DisplayAlert("Atención", "Debes haber iniciado sesión para guardar la prueba.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un error al guardar: {ex.Message}", "OK");
        }
    }
}