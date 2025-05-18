using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Need_2_Do.Models
{
    public class Nota
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Contenido { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaTarea { get; set; }
        public bool EsCompletada { get; set; } = false;


    }
}
