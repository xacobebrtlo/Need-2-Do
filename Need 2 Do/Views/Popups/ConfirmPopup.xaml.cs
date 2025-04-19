using CommunityToolkit.Maui.Views;

namespace Need_2_Do.Views.Popups
{
    public partial class ConfirmPopup : Popup
    {
        private readonly Func<Task> onConfirm;

        public ConfirmPopup(Func<Task> onConfirmAction)
        {
            InitializeComponent();
            onConfirm = onConfirmAction;
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (onConfirm != null)
                await onConfirm();

            Close();
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            Close();
        }
    }
}
