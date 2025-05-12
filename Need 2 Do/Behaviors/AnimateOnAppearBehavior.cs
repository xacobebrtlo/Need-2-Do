using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Need_2_Do.Behaviors
{
    public class AnimateOnFirstLoadBehavior : Behavior<View>
    {
        private bool _hasAnimated = false;

        protected override void OnAttachedTo(View bindable)
        {
            base.OnAttachedTo(bindable);

            bindable.BindingContextChanged += OnBindingContextChanged;
        }

        private async void OnBindingContextChanged(object sender, EventArgs e)
        {
            if (_hasAnimated)
                return;

            if (sender is View view)
            {
                _hasAnimated = true;

                view.Opacity = 0;
                view.TranslationY = 30;

                await view.FadeTo(1, 300, Easing.CubicOut);
                await view.TranslateTo(0, 0, 300, Easing.CubicOut);
            }
        }

        protected override void OnDetachingFrom(View bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.BindingContextChanged -= OnBindingContextChanged;
        }
    }
}

