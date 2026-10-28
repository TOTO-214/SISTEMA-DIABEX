namespace DIABEX.Views;

public partial class TemporizadorPostComidaPage : ContentPage
{
    private const string ClaveHoraFin = "PostComidaHoraFin";
    private IDispatcherTimer timer;
    private DateTime horaObjetivo;

    public TemporizadorPostComidaPage()
    {
        InitializeComponent();
        ConfigurarTimer();
        VerificarEstadoGuardado();
    }

    private void ConfigurarTimer()
    {
        timer = Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += (s, e) => ActualizarConteo();
    }

    private void VerificarEstadoGuardado()
    {
        string guardado = Preferences.Default.Get(ClaveHoraFin, string.Empty);

        if (!string.IsNullOrEmpty(guardado) && DateTime.TryParse(guardado, out horaObjetivo))
        {
            if (DateTime.Now < horaObjetivo)
            {
                HoraObjetivoLabel.Text = $"Chequeo a las {horaObjetivo:hh:mm tt}";
                StartButton.IsEnabled = false;
                timer.Start();
                ActualizarConteo();
            }
            else
            {
                TimerTextLabel.Text = "00:00:00";
                HoraObjetivoLabel.Text = "¡Hora de Chequeo!";
                Preferences.Default.Remove(ClaveHoraFin);
            }
        }
    }

    private async void OnIniciarTemporizadorClicked(object sender, EventArgs e)
    {
        horaObjetivo = DateTime.Now.AddHours(2);
        Preferences.Default.Set(ClaveHoraFin, horaObjetivo.ToString("o"));

        HoraObjetivoLabel.Text = $"Chequeo a las {horaObjetivo:hh:mm tt}";
        StartButton.IsEnabled = false;

        timer.Start();
        ActualizarConteo();

        await DisplayAlert("Temporizador Iniciado", $"Tu control postprandial se ha programado. Mide tu glucosa a las {horaObjetivo:hh:mm tt}.", "OK");
    }

    private void ActualizarConteo()
    {
        TimeSpan restante = horaObjetivo - DateTime.Now;

        if (restante.TotalSeconds <= 0)
        {
            timer.Stop();
            TimerTextLabel.Text = "00:00:00";
            HoraObjetivoLabel.Text = "¡Hora de Medir Glucosa!";
            StartButton.IsEnabled = true;
            Preferences.Default.Remove(ClaveHoraFin);

            DisplayAlert("⏰ ¡Tiempo Completado!", "Han transcurrido 2 horas desde tu comida. Es hora de realizar tu chequeo de glucosa.", "OK");
        }
        else
        {
            TimerTextLabel.Text = restante.ToString(@"hh\:mm\:ss");
        }
    }

    private void OnReiniciarTemporizadorClicked(object sender, EventArgs e)
    {
        timer.Stop();
        Preferences.Default.Remove(ClaveHoraFin);

        TimerTextLabel.Text = "02:00:00";
        HoraObjetivoLabel.Text = "Presiona Iniciar";
        StartButton.IsEnabled = true;
    }

    private async void OnVolverInicioClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}