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
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            // Determinar la ruta activa desde Shell
            var currentRoute = Shell.Current?.CurrentItem?.CurrentItem?.Route;

            if (BindingContext is MainViewModel vm)
            {
                if (currentRoute == "NotasMes")
                {
                    vm.FiltrarNotas("Mes");
                }
                else if (currentRoute == "NotasSemana")
                {
                    vm.FiltrarNotas("Semana");
                }
                else
                {
                    vm.FiltrarNotas("Todas");
                }
            }

            ActualizarEstadoVacio();
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
