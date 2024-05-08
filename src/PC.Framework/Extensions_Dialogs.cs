using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.ApplicationModel;
using Shiny;
using Shiny.Hosting;

namespace PC.Framework;


public static class DialogExtensions
{
    public static async Task<AccessState> OpenAppSettingsIf(this IDialogs dialogs, Func<Task<AccessState>> accessRequest, string deniedMessage, string restrictedMessage)
    {
        var result = await accessRequest.Invoke();

        switch (result)
        {
            case AccessState.Denied:
                await dialogs.SnackbarToOpenAppSettings(deniedMessage);
                break;

            case AccessState.Restricted:
                await dialogs.SnackbarToOpenAppSettings(restrictedMessage);
                break;
        }

        return result;
    }


    public static async Task SnackbarToOpenAppSettings(this IDialogs dialogs, string message, string actionText = "Open", int durationMillis = 3000)
    {
        var appInfo = Host.Current.Services.GetRequiredService<IAppInfo>();

        var result = await dialogs.Snackbar(
            message,
            durationMillis,
            actionText
        );
        if (result)
            appInfo.ShowSettingsUI();
    }
}
