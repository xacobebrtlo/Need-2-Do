using Need_2_Do.Models;

namespace Need_2_Do.Views;

[QueryProperty(nameof(NotaId), "notaId")]
public partial class BorrarNotaPage : ContentPage
{
    private Nota notaActual;

    public BorrarNotaPage()
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
            CargarNota();
        }
    }

    private async void CargarNota()
    {
        if (int.TryParse(NotaId, out int id))
        {
            notaActual = await App.Database.ObtenerNotaAsync(id);

            if (notaActual != null)
            {
                tituloLabel.Text = notaActual.Titulo;
                contenidoLabel.Text = notaActual.Contenido;
            }
        }
    }

    private async void OnConfirmarBorradoClicked(object sender, EventArgs e)
    {
        if (notaActual != null)
        {
            await App.Database.BorrarNotaAsync(notaActual);
            await Shell.Current.GoToAsync("..");
        }
    }

    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
