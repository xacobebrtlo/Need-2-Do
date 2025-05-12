using Need_2_Do;
using Need_2_Do.Models;
using Need_2_Do.ViewModels;
using Need_2_Do.Views.Base;
using System.Linq;

namespace Need_2_Do.Views
{
    public partial class MainPage : BasePage
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
        private async void OnEditarNotaDesdeLista(object sender, EventArgs e)
        {
            if (sender is SwipeItem item && item.CommandParameter is Nota nota)
            {
                await Shell.Current.GoToAsync($"EditarNotaPage?notaId={nota.Id}");
            }
        }

        private async void ActualizarEstadoVacio()
        {
            if (BindingContext is MainViewModel vm)
            {
                bool hayNotas = vm.Notas?.Any() ?? false;

                if (hayNotas)
                {
                    if (!NotasCollection.IsVisible)
                    {
                        NotasCollection.Opacity = 0;
                        NotasCollection.IsVisible = true;
                        await NotasCollection.FadeTo(1, 300, Easing.CubicInOut);
                    }

                    if (EmptyImage.IsVisible)
                    {
                        await EmptyImage.FadeTo(0, 200, Easing.CubicInOut);
                        EmptyImage.IsVisible = false;
                    }
                }
                else
                {
                    if (!EmptyImage.IsVisible)
                    {
                        EmptyImage.Opacity = 0;
                        EmptyImage.IsVisible = true;
                        await EmptyImage.FadeTo(1, 300, Easing.CubicInOut);
                    }

                    if (NotasCollection.IsVisible)
                    {
                        await NotasCollection.FadeTo(0, 200, Easing.CubicInOut);
                        NotasCollection.IsVisible = false;
                    }
                }
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

        private async void OnBorrarNotadeLista(object sender, EventArgs e)
        {
            if (sender is SwipeItem item && item.CommandParameter is Nota nota)
            {
                bool confirmar = await DisplayAlert("Eliminar", "¿Deseas eliminar esta nota?", "Sí", "Cancelar");

                if (confirmar && BindingContext is MainViewModel vm)
                {
                    await App.Database.BorrarNotaAsync(nota);
                    await vm.CargarNotas();
                    ActualizarEstadoVacio();

                }
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
