using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Need_2_Do.Models;
using Need_2_Do.Services;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Need_2_Do.Extensions;

namespace Need_2_Do.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly NoteService _noteService;

    public ObservableCollection<Nota> Notas { get; set; } = new();
    private List<Nota> todasLasNotas = new(); // Todas las notas para aplicar filtros
    private string filtroActual = "Todas";

    public MainViewModel()
    {
        _noteService = new NoteService();
        _ = CargarNotas(); // llamamos al cargar, no como Command
    }

    public async Task CargarNotas()
    {
        var lista = await _noteService.GetAllNotesAsync();
        todasLasNotas = lista.ToList(); // guardar todo
        AplicarFiltroActual(); // mostrar en pantalla según filtro actual
    }

    [RelayCommand]
    private async Task IrAEditarNota(Nota nota)
    {
        if (nota is not null)
            await Shell.Current.GoToAsync($"EditarNotaPage?notaId={nota.Id}");
    }

    [RelayCommand]
    private async Task BorrarNota(Nota nota)
    {
        if (nota is null) return;

        bool confirmar = await Application.Current.MainPage.DisplayAlert(
            "Eliminar", "¿Deseas eliminar esta nota?", "Sí", "Cancelar");

        if (!confirmar) return;

        // Clonar la nota antes de borrarla
        var notaBackup = new Nota
        {
            Titulo = nota.Titulo,
            Contenido = nota.Contenido,
            FechaCreacion = nota.FechaCreacion,
            FechaTarea = nota.FechaTarea
        };

        // Borrar de la base de datos
        await _noteService.DeleteNoteAsync(nota);
        await CargarNotas(); // ✅ Refresca CollectionView

        // Mostrar Snackbar con opción de deshacer
        var snackbar = Snackbar.Make(
            "🗑 Nota eliminada",
            async () =>
            {
                await App.Database.GuardarNotaAsync(notaBackup); // ✅ Recupera
                await CargarNotas(); // ✅ Refresca CollectionView
            },
            "Deshacer",
            TimeSpan.FromSeconds(5));

        await snackbar.Show();
    }


    [RelayCommand]
    private async Task TocarNota(Nota nota)
    {
        await IrAEditarNota(nota);
    }

    public void FiltrarNotas(string filtro)
    {
        filtroActual = filtro;
        AplicarFiltroActual();
    }

    private void AplicarFiltroActual()
    {
        Notas.Clear();

        IEnumerable<Nota> filtradas = todasLasNotas;

        if (filtroActual == "Semana")
        {
            var inicioSemana = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            var finSemana = inicioSemana.AddDays(7);
            filtradas = filtradas.Where(n => n.FechaTarea >= inicioSemana && n.FechaTarea <= finSemana);
        }
        else if (filtroActual == "Mes")
        {
            var inicioMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var finMes = inicioMes.AddMonths(1).AddDays(-1);
            filtradas = filtradas.Where(n => n.FechaTarea >= inicioMes && n.FechaTarea <= finMes);
        }

        foreach (var nota in filtradas)
            Notas.Add(nota);
    }
}
