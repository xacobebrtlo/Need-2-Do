using Need_2_Do.Models;

namespace Need_2_Do.Views;

public partial class A�adirNotaPage : ContentPage
{
    public A�adirNotaPage()
    {
        InitializeComponent();
    }
    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        Nota nota = new Nota
        {
            Titulo = tituloEntry.Text,
            Contenido = contenidoEditor.Text,
            FechaCreacion = DateTime.Now
        };
        await App.Database.GuardarNotaAsync(nota);
        await Shell.Current.GoToAsync("..");
    }
}