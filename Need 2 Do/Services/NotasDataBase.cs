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

        //Guarda Nota
        public Task<int> GuardarNotaAsync(Nota nota) =>
            nota.Id != 0 ? _database.UpdateAsync(nota) : _database.InsertAsync(nota);

        //Borra Nota
        public Task<int> BorrarNotaAsync(Nota nota) =>
            _database.DeleteAsync(nota);
    }
}
