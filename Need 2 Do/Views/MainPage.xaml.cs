using Need_2_Do;
using Need_2_Do.Models;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core; // Opcional si quieres personalizar
using CommunityToolkit.Maui.Views;
using Need_2_Do.Views.Popups;
using Need_2_Do.ViewModels;
namespace NotasApp.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
            //CargarNotas();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is MainViewModel vm)
                vm.CargarNotasCommand.Execute(null);
        }

        public async void AnimarYEditar(Frame card, Nota nota)
        {
            if (card == null || nota == null) return;

            await card.ScaleTo(0.96, 80);
            await card.ScaleTo(1.0, 80);

            await Shell.Current.GoToAsync($"EditarNotaPage?notaId={nota.Id}");
        }
        private void OnCardTapped(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is Nota nota)
            {
                AnimarYEditar(frame, nota);
            }
        }

        private async void OnNotaSeleccionada(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Nota nota)
            {
                await Shell.Current.GoToAsync($"EditarNotaPage?notaId={nota.Id}");
                //NotasCollection.SelectedItem = null; // deseleccionar
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
                    //CargarNotas(); // Recargar después de borrar

                }
            }
        }


    }
}
