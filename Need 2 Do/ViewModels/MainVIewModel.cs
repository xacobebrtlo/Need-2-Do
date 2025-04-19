using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Need_2_Do.Models;
using Need_2_Do.Services;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;



namespace Need_2_Do.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly NoteService _noteService;

    public ObservableCollection<Nota> Notas { get; set; } = new();

    public MainViewModel()
    {
        _noteService = new NoteService(); // ajusta si usas inyección de dependencias
        CargarNotasCommand.Execute(null);
    }

    [RelayCommand]
    private async Task CargarNotas()
    {
        var lista = await _noteService.GetAllNotesAsync();
        Notas.Clear();
        foreach (var n in lista)
            Notas.Add(n);
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





}
