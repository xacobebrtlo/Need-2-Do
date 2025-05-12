using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls;

namespace Need_2_Do.Views.Base
{
    public class BasePage : ContentPage
    {
        public BasePage()
        {
            // Fondo dinámico en toda la app
            this.SetAppThemeColor(BackgroundColorProperty, Color.FromArgb("#FFFFFF"), Color.FromArgb("#121212"));

            // Puedes aplicar configuraciones globales aquí si deseas (como márgenes, paddings, etc.)
        }
    }
}

