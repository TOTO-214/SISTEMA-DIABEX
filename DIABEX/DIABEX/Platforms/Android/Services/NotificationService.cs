using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using DIABEX.Services;
using AndroidApp = Android.App.Application; // Resuelve la ambigüedad con MAUI Application

namespace DIABEX.Platforms.Android.Services;

public class NotificationService : INotificationService
{
    private const string ChannelId = "diabex_reminders_channel";
    private const string ChannelName = "Recordatorios DIABEX";

    public NotificationService()
    {
        CrearCanalNotificacion();
    }

    private void CrearCanalNotificacion()
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            var channel = new NotificationChannel(ChannelId, ChannelName, NotificationImportance.High)
            {
                Description = "Canal para las alarmas y recordatorios de medicamentos de DIABEX"
            };

            var notificationManager = (NotificationManager)AndroidApp.Context.GetSystemService(Context.NotificationService);
            notificationManager?.CreateNotificationChannel(channel);
        }
    }

    public void ProgramarNotificacion(int id, string titulo, string mensaje, DateTime horaProgramada)
    {
        var intent = new Intent(AndroidApp.Context, typeof(AlarmReceiver));
        intent.PutExtra("titulo", titulo);
        intent.PutExtra("mensaje", mensaje);
        intent.PutExtra("id", id);

        var pendingIntent = PendingIntent.GetBroadcast(
            AndroidApp.Context,
            id,
            intent,
            PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable
        );

        var alarmManager = (AlarmManager)AndroidApp.Context.GetSystemService(Context.AlarmService);
        long timeInMillis = ((DateTimeOffset)horaProgramada).ToUnixTimeMilliseconds();

        if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
        {
            alarmManager?.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, timeInMillis, pendingIntent);
        }
        else
        {
            alarmManager?.SetExact(AlarmType.RtcWakeup, timeInMillis, pendingIntent);
        }
    }

    public void CancelarNotificacion(int id)
    {
        var intent = new Intent(AndroidApp.Context, typeof(AlarmReceiver));
        var pendingIntent = PendingIntent.GetBroadcast(
            AndroidApp.Context,
            id,
            intent,
            PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable
        );

        var alarmManager = (AlarmManager)AndroidApp.Context.GetSystemService(Context.AlarmService);
        alarmManager?.Cancel(pendingIntent);
    }
}

[BroadcastReceiver(Enabled = true, Exported = false)]
public class AlarmReceiver : BroadcastReceiver
{
    public override void OnReceive(Context context, Intent intent)
    {
        string titulo = intent.GetStringExtra("titulo") ?? "Recordatorio DIABEX";
        string mensaje = intent.GetStringExtra("mensaje") ?? "Es hora de tu medicamento.";
        int id = intent.GetIntExtra("id", 0);

        var builder = new NotificationCompat.Builder(context, "diabex_reminders_channel")
            .SetSmallIcon(global::Android.Resource.Drawable.IcPopupReminder) // Corregido: SetSmallIcon en lugar de SetSmallResource
            .SetContentTitle(titulo)
            .SetContentText(mensaje)
            .SetPriority(NotificationCompat.PriorityHigh)
            .SetAutoCancel(true);

        var notificationManager = NotificationManagerCompat.From(context);
        notificationManager.Notify(id, builder.Build());
    }
}