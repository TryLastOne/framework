namespace Sample;


public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		try
		{
			var builder = MauiApp
				.CreateBuilder()
				.UseMauiApp<App>()
				.UsePcFramework(
					new DryIocContainerExtension(),
					prism => prism.CreateWindow(
						"NavigationPage/MainPage",
						Console.WriteLine
					)
                )
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});

			builder.Services.AddLocalization();
			builder.Services.AddDataAnnotationValidation();

			builder.Services.RegisterForNavigation<MainPage, MainViewModel>();
			builder.Services.RegisterForNavigation<DialogsPage, DialogsViewModel>();
			builder.Services.RegisterForNavigation<ValidationPage, ValidationViewModel>();

			return builder.Build();
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
			throw;
		}
	}
}

