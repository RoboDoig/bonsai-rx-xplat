using bonsai_rx_xplat.ViewModels;
using bonsai_rx_xplat.Views;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Maui;

namespace bonsai_rx_xplat;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        builder.UseMauiApp<App>().UseMauiCommunityToolkit();

        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<MainPage>();

		return builder.Build();
	}
}
