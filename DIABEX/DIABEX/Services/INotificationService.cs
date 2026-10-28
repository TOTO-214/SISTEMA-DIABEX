namespace DIABEX.Services;

public interface INotificationService
{
    void ProgramarNotificacion(int id, string titulo, string mensaje, DateTime horaProgramada);
    void CancelarNotificacion(int id);
}