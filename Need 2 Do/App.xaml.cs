using Need_2_Do.Services;
using System.Diagnostics;

namespace Need_2_Do
{
    public partial class App : Application
    {
        static NotasDatabase database;

        public static NotasDatabase Database
        {

            get
            {
                if (database == null)
                {
                    var path =Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Notas.db3");
                    var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Notas.db3");

                 //Esto elimina la base de datos
                    //if (File.Exists(dbPath))
                    //{
                    //    File.Delete(dbPath);
                    //    System.Diagnostics.Debug.WriteLine("🗑 Base de datos eliminada correctamente.");
                    //}


                    database = new NotasDatabase(path);
                }

                return database;
            }
        }
        public App()
        {
            InitializeComponent();
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("TU_CLAVE_DE_LICENCIA");

            MainPage = new AppShell();
        }
    }
}
