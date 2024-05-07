using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using PC.Framework;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Sample;


public sealed class ValidationViewModel : ViewModel
{
    public ValidationViewModel(BaseServices services) : base(services)
    {
        Save = ReactiveCommand.CreateFromTask(
            async () => await this.Dialogs.Snackbar("Fired Saved because form was valid"),
            this.WhenValid()
        );
        BindValidation();
    }


    public ICommand Save { get; }

    [Reactive]
    [Required(AllowEmptyStrings = false)]
    [StringLength(5, MinimumLength = 3)]
    public string String { get; set; }

    [Reactive]
    [Url]
    public string Website { get; set; }

    [Reactive]
    [EmailAddress]
    [Required(AllowEmptyStrings = false)]
    public string Email { get; set; }
}