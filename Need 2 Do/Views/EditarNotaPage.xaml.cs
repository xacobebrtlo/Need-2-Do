using Need_2_Do.Models;
using Need_2_Do.Views.Base;

namespace Need_2_Do.Views
{
    public partial class EditarNotaPage : BasePage, IQueryAttributable
    {
        private Nota currentNota;

        public EditarNotaPage()
        {
            InitializeComponent();
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("notaId"))
            {
                int notaId = Convert.ToInt32(query["notaId"]);
                currentNota = await App.Database.ObtenerNotaPorIdAsync(notaId);

                if (currentNota != null)
                {
                    TituloEntry.Text = currentNota.Titulo;
                    ContenidoEditor.Text = currentNota.Contenido;
                    FechaPicker.Date = currentNota.FechaTarea ?? DateTime.Now;
                }
            }
        }

        private async void OnGuardarClicked(object sender, EventArgs e)
        {
            if (currentNota == null)
                return;

            currentNota.Titulo = TituloEntry.Text;
            currentNota.Contenido = ContenidoEditor.Text;
            currentNota.FechaTarea = FechaPicker.Date;

            await App.Database.GuardarNotaAsync(currentNota);
            await DisplayAlert("Actualizado", "Nota actualizada correctamente.", "OK");
            await Shell.Current.GoToAsync("..");
        }

        private async void OnBorrarClicked(object sender, EventArgs e)
        {
            if (currentNota == null)
                return;

            bool confirmar = await DisplayAlert("Eliminar", "¿Deseas eliminar esta nota?", "Sí", "Cancelar");

            if (confirmar)
            {
                await App.Database.BorrarNotaAsync(currentNota);
                await DisplayAlert("Eliminada", "Nota eliminada correctamente.", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}
