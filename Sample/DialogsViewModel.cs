using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Sample;

public class DialogsViewModel : ViewModel
{
    public DialogsViewModel(BaseServices services) : base(services)
    {
        Snackbar = ReactiveCommand.CreateFromTask(async () =>
        {
            Message = "Testing Snackbar";
            var clicked = await Dialogs.Snackbar("This is a snackbar", 5000, "OK");
            Message = clicked ? "The snackbar was tapped" : "The snackbar was not touched";
        });
    }


    public ICommand Snackbar { get; }
    [Reactive] public string Message { get; private set; }
}