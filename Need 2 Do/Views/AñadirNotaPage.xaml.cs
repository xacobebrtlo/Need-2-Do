using Need_2_Do.Models;
using Need_2_Do.Views.Base;

namespace Need_2_Do.Views
{
    public partial class A�adirNotaPage : BasePage
    {
        public A�adirNotaPage()
        {
            InitializeComponent();
        }

        private void OnSeleccionarFechaClicked(object sender, EventArgs e)
        {
            FechaPicker.IsVisible = !FechaPicker.IsVisible;
        }

        private async void OnGuardarClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TituloEntry.Text))
            {
                await DisplayAlert("Error", "Por favor ingresa un t�tulo.", "OK");
                return;
            }

            var nota = new Nota
            {
                Titulo = TituloEntry.Text,
                Contenido = ContenidoEditor.Text,
                FechaCreacion = DateTime.Now,
                FechaTarea = FechaPicker.IsVisible ? FechaPicker.Date : null
            };

            await App.Database.GuardarNotaAsync(nota);
            await DisplayAlert("Guardado", "Nota guardada correctamente.", "OK");
            await Shell.Current.GoToAsync("..");
        }
    }
}
