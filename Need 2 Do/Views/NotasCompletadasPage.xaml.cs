using Need_2_Do.Models;
using Need_2_Do.Views.Base;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace Need_2_Do.Views
{
    public partial class NotasCompletadasPage : BasePage
    {
        public ObservableCollection<Nota> Notas { get; set; } = new();

        public NotasCompletadasPage()
        {
            InitializeComponent();
            BindingContext = this;
        }
        private async void OnRestaurarNota(object sender, EventArgs e)
        {
            if (sender is SwipeItem item && item.CommandParameter is Nota notaOriginal)
            {
                // Guardamos backup por si el usuario quiere deshacer
                var notaBackup = new Nota
                {
                    Id = notaOriginal.Id,
                    Titulo = notaOriginal.Titulo,
                    Contenido = notaOriginal.Contenido,
                    FechaCreacion = notaOriginal.FechaCreacion,
                    FechaTarea = notaOriginal.FechaTarea,
                    EsCompletada = true
                };

                // Restauramos la nota
                notaOriginal.EsCompletada = false;
                await App.Database.GuardarNotaAsync(notaOriginal);
                await CargarNotasCompletadasAsync();

                // Mostrar snackbar con opción a deshacer
                var snackbar = Snackbar.Make(
                    "? Nota restaurada",
                    async () =>
                    {
                        // Deshacer: volver a poner como completada
                        notaBackup.EsCompletada = true;
                        await App.Database.GuardarNotaAsync(notaBackup);
                        await CargarNotasCompletadasAsync();
                    },
                    "Deshacer",
                    TimeSpan.FromSeconds(5));

                await snackbar.Show();
            }
        }


        private async Task CargarNotasCompletadasAsync()
        {
            var completadas = await App.Database.ObtenerNotasAsync();
            var lista = completadas.Where(n => n.EsCompletada).ToList();

            Notas.Clear();
            foreach (var nota in lista)
                Notas.Add(nota);

            EmptyImage.IsVisible = !Notas.Any();
            NotasCollection.IsVisible = Notas.Any();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CargarNotasCompletadasAsync();
        }

    }
}
