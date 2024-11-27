using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TPCESI_RECETTES.Context;
using TPCESI_RECETTES.Repository;
using TPCESI_RECETTES.Services;

DotNetEnv.Env.Load();
var services = new ServiceCollection();

// Add services to the container.
services.AddLogging(configure => {
    configure.AddConsole();
    configure.AddDebug();
});
services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
services.AddScoped<DbContext, PostgresContext>();
services.AddScoped<IRecetteRepository, RecetteRepository>();
services.AddScoped<IIngredientRepository, IngredientRepository>();
services.AddScoped<ICommentaireRepository, CommentaireRepository>();
services.AddScoped<MenuService>();

// Configuration de la connexion à la base de données
string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("La chaîne de connexion à la base de données n'est pas définie.");
}
services.AddDbContext<PostgresContext>(options =>
    options.UseNpgsql(connectionString)
);

var serviceProvider = services.BuildServiceProvider();

// Utilisez le serviceProvider pour exécuter votre logique d'application console
using (var scope = serviceProvider.CreateScope())
{
    var menuService = scope.ServiceProvider.GetRequiredService<MenuService>();
    await menuService.StartMenuAsync();
}