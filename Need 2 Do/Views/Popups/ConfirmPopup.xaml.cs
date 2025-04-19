using CommunityToolkit.Maui.Views;

namespace Need_2_Do.Views.Popups;

public partial class ConfirmPopup : Popup
{
    private readonly TaskCompletionSource<bool> _tcs = new();

    public ConfirmPopup(string message)
    {
        try
        {
            InitializeComponent();
            MessageLabel.Text = message;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[POPUP ERROR] {ex.Message}");
        }
    }


    public Task<bool> ShowConfirmationAsync() => _tcs.Task;

    private void OnCancelClicked(object sender, EventArgs e)
    {
        _tcs.TrySetResult(false);
        Close();
    }

    private void OnConfirmClicked(object sender, EventArgs e)
    {
        _tcs.TrySetResult(true);
        Close();
    }
}
