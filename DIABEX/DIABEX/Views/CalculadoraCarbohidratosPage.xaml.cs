using System.Collections.ObjectModel;
using DIABEX.Models;

namespace DIABEX.Views;

public partial class CalculadoraCarbohidratosPage : ContentPage
{
    private List<Alimento> listaAlimentos = new();
    private ObservableCollection<ItemPlato> miPlato = new();
    private double totalCarbosCalculados = 0;

    public CalculadoraCarbohidratosPage()
    {
        InitializeComponent();
        CargarBaseDeDatosLocal();
        PlatoCollectionView.ItemsSource = miPlato;
    }

    private void CargarBaseDeDatosLocal()
    {
        listaAlimentos = new List<Alimento>
        {
            new Alimento { Nombre = "Arroz blanco cocido", CarbosPor100g = 28.0 },
            new Alimento { Nombre = "Frijoles negros cocidos", CarbosPor100g = 23.0 },
            new Alimento { Nombre = "Tortilla de maíz", CarbosPor100g = 45.0 },
            new Alimento { Nombre = "Pan integral", CarbosPor100g = 41.0 },
            new Alimento { Nombre = "Manzana", CarbosPor100g = 14.0 },
            new Alimento { Nombre = "Guineo / Banano", CarbosPor100g = 23.0 },
            new Alimento { Nombre = "Papa cocida", CarbosPor100g = 17.0 },
            new Alimento { Nombre = "Avena en hojuelas", CarbosPor100g = 66.0 },
            new Alimento { Nombre = "Pechuga de pollo", CarbosPor100g = 0.0 },
            new Alimento { Nombre = "Huevo entero", CarbosPor100g = 0.6 },
            new Alimento { Nombre = "Leche entera", CarbosPor100g = 5.0 }
        };

        AlimentoPicker.ItemsSource = listaAlimentos;
    }

    private void OnAlimentoSeleccionado(object sender, EventArgs e)
    {
        if (AlimentoPicker.SelectedItem is Alimento seleccionado)
        {
            Carbos100gLabel.Text = $"{seleccionado.CarbosPor100g} g";
        }
    }

    private async void OnAgregarAlPlatoClicked(object sender, EventArgs e)
    {
        if (AlimentoPicker.SelectedItem is not Alimento seleccionado)
        {
            await DisplayAlert("Atención", "Por favor selecciona un alimento.", "OK");
            return;
        }

        if (!double.TryParse(GramosEntry.Text, out double gramos) || gramos <= 0)
        {
            await DisplayAlert("Atención", "Ingresa una cantidad en gramos válida.", "OK");
            return;
        }

        double carbosCalculados = (seleccionado.CarbosPor100g * gramos) / 100.0;

        miPlato.Add(new ItemPlato
        {
            Nombre = seleccionado.Nombre,
            DetalleGramaje = $"{gramos} g consumidos",
            CarbosTotalesText = $"{carbosCalculados:F1} g carbos",
            CarbosNumericos = carbosCalculados
        });

        RecalcularTotal();
    }

    private void RecalcularTotal()
    {
        totalCarbosCalculados = miPlato.Sum(x => x.CarbosNumericos);
        TotalCarbosLabel.Text = $"{totalCarbosCalculados:F1} g";
    }

    private void OnLimpiarPlatoClicked(object sender, EventArgs e)
    {
        miPlato.Clear();
        RecalcularTotal();
    }

    private async void OnVolverInicioClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}