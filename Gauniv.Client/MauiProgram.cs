using CommunityToolkit.Maui;
using Gauniv.Client.Services;
using Gauniv.Client.ViewModel;
using Gauniv.Network;
using Microsoft.Extensions.Logging;

namespace Gauniv.Client
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>().UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddTransient<IndexViewModel>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            Task.Run(() =>
            {
                //var client = app.Services.GetRequiredService<v1Client>(); test
                //var categories = await client.CategoriesAsync();

                //Console.WriteLine("📌 Liste des catégories :");
                //foreach (var category in categories)
                //{
                //    Console.WriteLine($"- {category.Nom} (ID: {category.Id})");
                //}
                // Vous pouvez initialiser la connection au serveur a partir d'ici
            });
            return app;
        }
    }
}
