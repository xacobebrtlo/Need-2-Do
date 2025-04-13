using Need_2_Do.Models;

namespace Need_2_Do.Views;

[QueryProperty(nameof(NotaId), "notaId")]
public partial class EditarNotaPage : ContentPage
{
    private Nota notaActual;

    public EditarNotaPage()
    {
        InitializeComponent();
    }

    private string notaId;
    public string NotaId
    {
        get => notaId;
        set
        {
            notaId = value;
            CargarNota(); // cargamos la nota cuando el parámetro llega
        }
    }

    private async void CargarNota()
    {
        if (int.TryParse(NotaId, out int id))
        {
            notaActual = await App.Database.ObtenerNotaAsync(id);

            if (notaActual != null)
            {
                tituloEntry.Text = notaActual.Titulo;
                contenidoEditor.Text = notaActual.Contenido;
            }
        }
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        if (notaActual == null)
            return;

        notaActual.Titulo = tituloEntry.Text;
        notaActual.Contenido = contenidoEditor.Text;

        await App.Database.GuardarNotaAsync(notaActual);
        await Shell.Current.GoToAsync("..");
    }

    private async void OnEliminarClicked(object sender, EventArgs e)
    {
        if (notaActual == null)
            return;

        bool confirmar = await DisplayAlert("Eliminar", "¿Deseas eliminar esta nota?", "Sí", "Cancelar");

        if (confirmar)
        {
            await App.Database.BorrarNotaAsync(notaActual);
            await Shell.Current.GoToAsync($"BorrarNotaPage?notaId={notaActual.Id}");

        }
    }
}
