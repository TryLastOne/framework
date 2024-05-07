#if PLATFORM
using System;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Prism.AppModel;
using Prism.Common;
using Prism.Ioc;
using Prism.Navigation.Xaml;
using Prism.Services;
using Shiny;
using Font = Microsoft.Maui.Font;
using SB = CommunityToolkit.Maui.Alerts.Snackbar;
using TM = CommunityToolkit.Maui.Alerts.Toast;

namespace PC.Framework.Impl;


public class NativeDialogs(IApplication application, IPlatform platform) : IDialogs
{
    public Task<string?> ActionSheet(string title, string? acceptText = null, string? dismissText = null, params string[] options)
        => Run(dialogs => dialogs.DisplayActionSheetAsync(
            title,
            acceptText,
            dismissText,
            options
        ));

    public Task Alert(string message, string? title = null, string? dismissText = null)
        => Run(async dialogs =>
        {
            await dialogs.DisplayAlertAsync(title, message, dismissText ?? "OK");
            return true;
        });

    public Task<bool> Confirm(string message, string? title = null, string? okText = "OK", string? cancelText = "Cancel")
        => Run(dialogs => dialogs.DisplayAlertAsync(title, message, okText, cancelText));

    public Task<string?> Input(string question, string? title = null, string? acceptText = "OK", string? dismissText = "Cancel", string? placeholder = null, int? maxLength = null, InputKeyboard keyboard = InputKeyboard.Default)
    {
        var keyboardType = Enum.Parse<KeyboardType>(keyboard.ToString());
        return Run(dialogs => dialogs.DisplayPromptAsync(title, question, acceptText, dismissText, placeholder, maxLength ?? -1, keyboardType));
    }

    public async Task<bool> Snackbar(string message, int durationMillis = 3000, string? actionText = null)
    {
        var clicked = false;        
        var snackbar = SB.Make(
            message,
            () => clicked = true,
            actionText ?? "OK",
            TimeSpan.FromMilliseconds(durationMillis),
            SnackbarOptions
        );
        await snackbar.Show();
        return clicked;
    }
    
    public async Task Toast(string message, int durationMillis= 4000, double fontSize=14.0)
    {
        const ToastDuration duration = ToastDuration.Long;
        var toast = TM.Make(message, duration, fontSize);
        await toast.Show();
    }

    private async Task<T> Run<T>(Func<IPageDialogService, Task<T>> func)
    {
        var window = application.Windows.OfType<Window>().First();
        var currentPage = MvvmHelpers.GetCurrentPage(window.Page);
        var container = currentPage.GetContainerProvider();

        var dialogs = container.Resolve<IPageDialogService>();
        var result = await platform.InvokeTaskOnMainThread(() => func(dialogs));
        return result;
    }


    /// <summary>
    /// 
    /// </summary>
    private static readonly SnackbarOptions SnackbarOptions = new()
    {
        BackgroundColor = Colors.LightGray,
        TextColor = Colors.White,
        ActionButtonTextColor = Colors.Gray,
        CornerRadius = new CornerRadius(15),
        Font = Font.SystemFontOfSize(14),
        ActionButtonFont = Font.SystemFontOfSize(14)
    };
}
#endif
