using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

#if ANDROID
using Android.Content;
#endif

namespace DIABEX.Views;

public partial class EmergenciaPage : ContentPage
{
    private const string ClaveTelefono = "ContactoEmergenciaNum";

    public EmergenciaPage()
    {
        InitializeComponent();
        CargarContactoGuardado();
    }

    private void CargarContactoGuardado()
    {
        string numero = Preferences.Default.Get(ClaveTelefono, string.Empty);
        if (!string.IsNullOrEmpty(numero))
        {
            ContactoLabel.Text = $"Contacto: {numero}";
            TelefonoEntry.Text = numero;
        }
        else
        {
            ContactoLabel.Text = "No has guardado un número aún";
        }
    }

    private async void OnGuardarContactoClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TelefonoEntry.Text))
        {
            await DisplayAlert("Atención", "Ingresa un número de teléfono válido.", "OK");
            return;
        }

        Preferences.Default.Set(ClaveTelefono, TelefonoEntry.Text.Trim());
        CargarContactoGuardado();
        await DisplayAlert("Éxito", "Contacto de emergencia guardado en tu dispositivo.", "OK");
    }

    private async void OnLlamarContactoClicked(object sender, EventArgs e)
    {
        string numero = Preferences.Default.Get(ClaveTelefono, string.Empty);

        if (string.IsNullOrEmpty(numero))
        {
            await DisplayAlert("Atención", "Por favor guarda primero un número de emergencia abajo.", "OK");
            return;
        }

        try
        {
#if ANDROID
            // 1. Pedir permiso explícito de llamadas
            var status = await Permissions.CheckStatusAsync<Permissions.Phone>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Phone>();
            }

            if (status == PermissionStatus.Granted)
            {
                // 2. Realizar la llamada directa especificando Android.Net.Uri para evitar la ambigüedad
                var intent = new Intent(Intent.ActionCall);
                intent.SetData(Android.Net.Uri.Parse($"tel:{numero}"));
                intent.AddFlags(ActivityFlags.NewTask);

                Android.App.Application.Context.StartActivity(intent);
            }
            else
            {
                await DisplayAlert("Permiso Denegado", "Se requiere permiso para realizar la llamada de emergencia directamente.", "OK");
            }
#else
            if (PhoneDialer.Default.IsSupported)
            {
                PhoneDialer.Default.Open(numero);
            }
            else
            {
                await DisplayAlert("Error", "Tu dispositivo no soporta llamadas directas.", "OK");
            }
#endif
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo iniciar la llamada: {ex.Message}", "OK");
        }
    }

    private async void OnVolverInicioClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}