using Need_2_Do;
using Need_2_Do.Models;
using Need_2_Do.ViewModels;
using System.Linq;

namespace NotasApp.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is MainViewModel vm)
            {
                await vm.CargarNotas();
                ActualizarEstadoVacio();
            }
        }

        private void ActualizarEstadoVacio()
        {
            if (BindingContext is MainViewModel vm)
            {
                bool hayNotas = vm.Notas?.Any() ?? false;

                EmptyImage.IsVisible = !hayNotas;
                NotasCollection.IsVisible = hayNotas;
            }
        }

        private async void OnFiltroButtonClicked(object sender, EventArgs e)
        {
            try
            {
                // Animación opcional (puedes dejarla si quieres que haya una ligera vibración)
                await FiltroLabel.ScaleTo(1.1, 100);
                await FiltroLabel.ScaleTo(1.0, 100);

                string accion = await DisplayActionSheet("Filtrar notas", "Cancelar", null,
                    "Todas", "Esta semana", "Este mes");

                if (BindingContext is MainViewModel vm)
                {
                    switch (accion)
                    {
                        case "Todas":
                            vm.FiltrarNotas("Todas");
                            FiltroLabel.Text = "Todas ▼"; // Apunta hacia abajo
                            break;
                        case "Esta semana":
                            vm.FiltrarNotas("Semana");
                            FiltroLabel.Text = "Esta semana ▼"; // Apunta hacia abajo
                            break;
                        case "Este mes":
                            vm.FiltrarNotas("Mes");
                            FiltroLabel.Text = "Este mes ▼"; // Apunta hacia abajo
                            break;
                    }
                }

                ActualizarEstadoVacio();
            }
            finally
            {
                // Si quieres que al cerrar el menú vuelva a ▶ puedes hacerlo aquí
                // Pero normalmente dejamos ▼ cuando ya está filtrado
            }
        }

        private async void OnNuevaNotaClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("AñadirNotaPage");
        }

        private async void OnCardTapped(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is Nota nota)
            {
                await Shell.Current.GoToAsync($"EditarNotaPage?notaId={nota.Id}");
            }
        }
    }
}
