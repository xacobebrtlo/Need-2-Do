using CommunityToolkit.Maui.Alerts;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
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

            // Leer tema guardado y aplicarlo
            bool temaOscuro = Preferences.Get("TemaOscuro", false);
            App.Current.UserAppTheme = temaOscuro ? AppTheme.Dark : AppTheme.Light;

            // Mover el botón al lugar correspondiente
            SwitchKnob.HasShadow = true;
            SwitchKnob.Shadow = new Shadow
            {
                Brush = Brush.Black,
                Offset = new Point(0, 2),
                Radius = 8,
                Opacity = 0.25f
            };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is MainViewModel vm)
            {
                var rutaActual = Shell.Current?.CurrentState?.Location.ToString();

                if (rutaActual.Contains("NotasMes"))
                    vm.FiltrarNotas("Mes");
                else if (rutaActual.Contains("NotasSemana"))
                    vm.FiltrarNotas("Semana");
                else
                    vm.FiltrarNotas("Todas");

                await vm.CargarNotas();
                await Task.Delay(50);
                ActualizarEstadoVacio();
            }
        }

        private async void OnThemeToggleTapped(object sender, EventArgs e)
        {
            bool temaOscuro = App.Current.UserAppTheme == AppTheme.Dark;

            // Cambiar tema
            temaOscuro = !temaOscuro;
            App.Current.UserAppTheme = temaOscuro ? AppTheme.Dark : AppTheme.Light;

            // Guardar en preferencias
            Preferences.Set("TemaOscuro", temaOscuro);

            // Coordenadas ajustadas
            double destinoX = temaOscuro ? 32 : 2;
            double reboteX = temaOscuro ? destinoX - 2 : destinoX + 2;

            // 1. Rebote sutil antes de asentarse
            await SwitchKnob.TranslateTo(reboteX, 0, 70, Easing.CubicOut);
            await SwitchKnob.TranslateTo(destinoX, 0, 100, Easing.CubicIn);

            // Sombra elegante
            SwitchKnob.HasShadow = true;
            SwitchKnob.Shadow = new Shadow
            {
                Brush = Brush.Black,
                Offset = new Point(0, 1),
                Radius = 4,
                Opacity = 0.2f
            };
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

        private async void OnEditarNotaDesdeLista(object sender, EventArgs e)
        {
            if (sender is SwipeItem item && item.CommandParameter is Nota nota)
            {
                await Shell.Current.GoToAsync($"EditarNotaPage?notaId={nota.Id}");
            }
        }

        private async void OnBorrarNotadeLista(object sender, EventArgs e)
        {
            if (sender is SwipeItem item && item.CommandParameter is Nota nota && BindingContext is MainViewModel vm)
            {
                bool confirmar = await DisplayAlert("Eliminar", "¿Deseas eliminar esta nota?", "Sí", "Cancelar");

                if (confirmar)
                {
                    await App.Database.BorrarNotaAsync(nota);
                    await vm.CargarNotas();
                    ActualizarEstadoVacio();
                    var notaBackup = new Nota
                    {
                        Id = nota.Id,
                        Titulo = nota.Titulo,
                        Contenido = nota.Contenido,
                        FechaCreacion = nota.FechaCreacion,
                        FechaTarea = nota.FechaTarea,
                        EsCompletada = true
                    };
                    // Mostrar Snackbar con opción de deshacer
                    var snackbar = Snackbar.Make(
                        "🗑 Nota eliminada",
                        async () =>
                        {
                            await App.Database.GuardarNotaAsync(notaBackup);
                            await vm.CargarNotas();
                        },
                        "Deshacer",
                        TimeSpan.FromSeconds(5));

                    await snackbar.Show();
                }
            }

                
        }

        private async void OnMarcarCompletado(object sender, EventArgs e)
        {
            if (sender is SwipeItem item && item.CommandParameter is Nota nota && BindingContext is MainViewModel vm)
            {
                nota.EsCompletada = true;
                await App.Database.GuardarNotaAsync(nota);
                await vm.CargarNotas();
                ActualizarEstadoVacio();
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
    }
}
