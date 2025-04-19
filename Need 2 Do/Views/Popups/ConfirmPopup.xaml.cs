using CommunityToolkit.Maui.Views;

namespace Need_2_Do.Views.Popups
{
    public partial class ConfirmPopup : Popup
    {
        private readonly Action onConfirm;

        public ConfirmPopup(Action onConfirmAction)
        {
            InitializeComponent();
            onConfirm = onConfirmAction;
        }

        private void OnDeleteClicked(object sender, EventArgs e)
        {
            onConfirm?.Invoke();
            Close();
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            Close();
        }
    }
}
