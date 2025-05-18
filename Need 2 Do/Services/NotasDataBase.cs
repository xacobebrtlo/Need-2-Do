using Need_2_Do.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Need_2_Do.Services
{
    public class NotasDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public NotasDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Nota>().Wait();
        }

        //Obtiene array de notas ordenado por fecha de creacion
        public Task<List<Nota>> ObtenerNotasAsync() =>
            _database.Table<Nota>().OrderByDescending(n => n.FechaCreacion).ToListAsync();

        //Obtiene nota por id
        public Task<Nota> ObtenerNotaAsync(int id) =>
            _database.Table<Nota>().Where(i => i.Id == id).FirstOrDefaultAsync();

        public async Task<int> GuardarNotaAsync(Nota nota)
        {
            if (nota.Id != 0)
                return await _database.UpdateAsync(nota);  // 👈 Esto actualiza
            else
                return await _database.InsertAsync(nota);  // 👈 Esto crea
        }



        //Borra Nota
        public Task<int> BorrarNotaAsync(Nota nota) =>
            _database.DeleteAsync(nota);

        public Task<Nota> ObtenerNotaPorIdAsync(int id)
        {
            return _database.Table<Nota>()
                           .Where(n => n.Id == id)
                           .FirstOrDefaultAsync();
        }
    }
}
