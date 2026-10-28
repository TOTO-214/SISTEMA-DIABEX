using DIABEX.Models;
using DIABEX.Services;

namespace DIABEX.Views;

public partial class RecordatoriosPage : ContentPage
{
    public RecordatoriosPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarRecordatoriosAsync();
    }

    private async Task CargarRecordatoriosAsync()
    {
        try
        {
            var currentUser = SupabaseService.Instance.Auth.CurrentUser;
            if (currentUser == null) return;

            var respuesta = await SupabaseService.Instance
                .From<Recordatorio>()
                .Where(x => x.UserId == currentUser.Id)
                .Get();

            RecordatoriosCollectionView.ItemsSource = respuesta.Models;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudieron cargar los recordatorios: {ex.Message}", "OK");
        }
    }

    private async void OnGuardarRecordatorioClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TituloEntry.Text))
        {
            await DisplayAlert("Atención", "Escribe un título para el recordatorio.", "OK");
            return;
        }

        try
        {
            var currentUser = SupabaseService.Instance.Auth.CurrentUser;
            if (currentUser == null) return;

            // 1. Pedir permiso de notificaciones en Android 13+
            var status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.PostNotifications>();
            }

            // 2. Formato de hora limpio HH:mm para la interfaz
            var hora = HoraPicker.Time ?? TimeSpan.Zero;
            string horaFormateada = DateTime.Today.Add(hora).ToString("hh:mm tt");

            var nuevoRecordatorio = new Recordatorio
            {
                UserId = currentUser.Id,
                Titulo = TituloEntry.Text,
                Hora = horaFormateada,
                Activo = true
            };

            // 3. Guardar en la base de datos Supabase
            await SupabaseService.Instance.From<Recordatorio>().Insert(nuevoRecordatorio);

            // 4. Calcular la fecha y hora exactas para la alarma local
            DateTime fechaProgramada = DateTime.Today.Add(hora);
            if (fechaProgramada < DateTime.Now)
            {
                fechaProgramada = fechaProgramada.AddDays(1); // Si la hora ya pasó hoy, se agenda para mañana
            }

            // 5. Programar la notificación local en Android
#if ANDROID
            if (status == PermissionStatus.Granted)
            {
                var notificationService = new DIABEX.Platforms.Android.Services.NotificationService();
                notificationService.ProgramarNotificacion(
                    id: new Random().Next(1000, 9999),
                    titulo: "⏰ Recordatorio DIABEX",
                    mensaje: TituloEntry.Text,
                    horaProgramada: fechaProgramada
                );
            }
#endif

            TituloEntry.Text = string.Empty;
            await CargarRecordatoriosAsync();
            await DisplayAlert("Éxito", "Recordatorio y notificación programados correctamente.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo guardar: {ex.Message}", "OK");
        }
    }

    private async void OnEliminarClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Recordatorio item)
        {
            bool confirmar = await DisplayAlert("Eliminar", $"¿Deseas eliminar '{item.Titulo}'?", "Sí", "No");
            if (confirmar)
            {
                try
                {
                    await SupabaseService.Instance
                        .From<Recordatorio>()
                        .Where(x => x.Id == item.Id)
                        .Delete();

                    await CargarRecordatoriosAsync();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"No se pudo eliminar: {ex.Message}", "OK");
                }
            }
        }
    }

    private async void OnVolverInicioClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}