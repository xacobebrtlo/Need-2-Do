using Need_2_Do;
using Need_2_Do.Models;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core; // Opcional si quieres personalizar
using CommunityToolkit.Maui.Views;
using Need_2_Do.Views.Popups;
namespace NotasApp.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            CargarNotas();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CargarNotas();
        }

        private async void CargarNotas()
        {
            var notas = await App.Database.ObtenerNotasAsync();

            // Solo insertar si no hay ninguna nota
            //if (notas.Count == 0)
            //{
            //    var notaDemo = new Nota
            //    {
            //        Titulo = "Ejemplo de nota",
            //        Contenido = "Esta es una nota de prueba para ver el diseño.",
            //        FechaCreacion = DateTime.Now
            //    };

            //    await App.Database.GuardarNotaAsync(notaDemo);

            //    // Recargar lista
            //    notas = await App.Database.ObtenerNotasAsync();
            //}

            NotasCollection.ItemsSource = notas;
        }


        private async void OnNotaSeleccionada(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Nota nota)
            {
                await Shell.Current.GoToAsync($"EditarNotaPage?notaId={nota.Id}");
                NotasCollection.SelectedItem = null; // deseleccionar
            }
        }


        private async void OnNuevaNotaClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("AñadirNotaPage");
        }

        private async void OnEditarNotaDesdeLista(object sender, EventArgs e)
        {
            if (sender is Button boton && boton.CommandParameter is Nota nota)
            {
                await Shell.Current.GoToAsync($"EditarNotaPage?notaId={nota.Id}");
            }
        }

        private async void OnBorrarNotadeLista(object sender, EventArgs e)
        {
            if (sender is Button boton && boton.CommandParameter is Nota nota)
            {
                bool confirmar = await DisplayAlert("Eliminar", "¿Deseas eliminar esta nota?", "Sí", "Cancelar");

                if (confirmar)
                {
                    await App.Database.BorrarNotaAsync(nota);
                    CargarNotas(); // Recargar después de borrar

                    // 🎉 Mostrar Snackbar
                    //await Snackbar.Make("Nota eliminada", duration: TimeSpan.FromSeconds(2)).Show();
                }
            }
        }


    }
}
