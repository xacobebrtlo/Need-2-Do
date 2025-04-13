using Need_2_Do.Views;

namespace Need_2_Do
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("AñadirNotaPage", typeof(AñadirNotaPage));
            Routing.RegisterRoute("EditarNotaPage", typeof(EditarNotaPage));
            Routing.RegisterRoute("BorrarNotaPage", typeof(Need_2_Do.Views.BorrarNotaPage));



        }
    }
}
