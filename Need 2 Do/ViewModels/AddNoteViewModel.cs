using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Need_2_Do.ViewModels
{
    public class AddNoteViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string nombre = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));

        private bool mostrarCalendario;
        public bool MostrarCalendario
        {
            get => mostrarCalendario;
            set
            {
                if (mostrarCalendario == value) return;
                mostrarCalendario = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TextoBotonFecha));
            }
        }

        private DateTime? fechaTarea;
        public DateTime? FechaTarea
        {
            get => fechaTarea;
            set
            {
                if (fechaTarea == value) return;
                fechaTarea = value;
                OnPropertyChanged();
            }
        }

        public string TextoBotonFecha => MostrarCalendario ? "📅 Quitar fecha" : "📅 Añadir fecha";

        public AddNoteViewModel()
        {
            MostrarCalendario = false;
            FechaTarea = null;
        }
    }
}
