using Need_2_Do.Models;
using Need_2_Do.ViewModels;
using Syncfusion.Maui.Core.Carousel;

namespace Need_2_Do.Views;

public partial class AñadirNotaPage : ContentPage
{

    public AñadirNotaPage()
    {
        InitializeComponent();
        BindingContext = new AddNoteViewModel();
    }
    private void OnAsignarFechaClicked(object sender, EventArgs e)
    {
        if (BindingContext is AddNoteViewModel viewModel)
        {
            viewModel.MostrarCalendario = !viewModel.MostrarCalendario;

            if (viewModel.MostrarCalendario && viewModel.FechaTarea == null)
            {
                viewModel.FechaTarea = DateTime.Today;
            }

            if (!viewModel.MostrarCalendario)
            {
                viewModel.FechaTarea = null;
            }
        }
    }



    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        if (BindingContext is AddNoteViewModel viewModel)
        {
            Nota nota = new Nota
            {
                Titulo = tituloEntry.Text,
                Contenido = contenidoEditor.Text,
                FechaCreacion = DateTime.Now,
                FechaTarea = viewModel.MostrarCalendario ? viewModel.FechaTarea : null
            };
            await App.Database.GuardarNotaAsync(nota);
            await Shell.Current.GoToAsync("..");
        }
    }
}