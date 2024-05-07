using System.Windows.Input;
using ReactiveUI;

namespace PC.Framework;


public class CommandItem : ReactiveObject
{
    private string? _imageUri;
    public string? ImageUri
    {
        get => _imageUri;
        set => this.RaiseAndSetIfChanged(ref _imageUri, value);
    }


    private string? _text;
    public string? Text
    {
        get => _text;
        set => this.RaiseAndSetIfChanged(ref _text, value);
    }


    private string? _detail;
    public string? Detail
    {
        get => _detail;
        set => this.RaiseAndSetIfChanged(ref _detail, value);
    }

    public ICommand? PrimaryCommand { get; set; }
    public ICommand? SecondaryCommand { get; set; }
    public object? Data { get; set; }
}

