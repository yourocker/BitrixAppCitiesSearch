namespace BitrixAppCitiesSearch;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers(); // Регистрируем контроллеры

        // Добавляем поддержку HttpClient для API-запросов
        builder.Services.AddHttpClient();

        // Swagger для документации API
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        // Добавляем маршруты контроллеров
        app.MapControllers();

        // Принудительно задаём порты
        app.Urls.Add("http://localhost:5000");
        app.Urls.Add("https://localhost:5001");

        app.Run();
    }
}