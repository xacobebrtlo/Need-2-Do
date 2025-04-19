using Need_2_Do.Models;

namespace Need_2_Do.Services;

public class NoteService
{
    public async Task<List<Nota>> GetAllNotesAsync()
    {
        return await App.Database.ObtenerNotasAsync(); // ✅ usa tu base de datos real
    }

    public async Task DeleteNoteAsync(Nota nota)
    {
        await App.Database.BorrarNotaAsync(nota); // ✅ también conecta el borrado
    }
}
