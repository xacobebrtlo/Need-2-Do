using Need_2_Do.Models;

namespace Need_2_Do.Services
{
    public class NoteService
    {
        // ✅ Obtener todas las notas no completadas
        public async Task<List<Nota>> GetAllNotesAsync()
        {
            var todas = await App.Database.ObtenerNotasAsync();
            return todas.Where(n => !n.EsCompletada).ToList();
        }

        // ✅ Obtener solo notas completadas
        public async Task<List<Nota>> GetNotasCompletadasAsync()
        {
            var todas = await App.Database.ObtenerNotasAsync();
            return todas.Where(n => n.EsCompletada).ToList();
        }

        // ✅ Obtener una nota por Id
        public async Task<Nota> GetByIdAsync(int id)
        {
            return await App.Database.ObtenerNotaPorIdAsync(id);
        }

        // ✅ Guardar una nota (nueva o actualizada)
        public async Task SaveNoteAsync(Nota nota)
        {
            await App.Database.GuardarNotaAsync(nota);
        }

        // ✅ Borrar una nota
        public async Task DeleteNoteAsync(Nota nota)
        {
            await App.Database.BorrarNotaAsync(nota);
        }
    }
}
