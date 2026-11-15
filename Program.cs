using Microsoft.EntityFrameworkCore;
using wt_lab7_7postgres_efcore_antonov.Data;
using wt_lab7_7postgres_efcore_antonov.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Берем строку подключения для локальной разработки (из appsettings.Development.json)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Проверяем, есть ли переменная окружения DATABASE_URL (для Render)
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
if (!string.IsNullOrWhiteSpace(databaseUrl))
{
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':', 2);

    // Явно указываем порт 5432, если он не указан в URL
    var port = uri.Port > 0 ? uri.Port : 5432;

    connectionString = $"Host={uri.Host};Port={port};Database={uri.AbsolutePath.TrimStart('/')};Username={Uri.UnescapeDataString(userInfo[0])};Password={Uri.UnescapeDataString(userInfo[1])};SSL Mode=Disable";
}

// 3. Подключаем базу данных PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

// 4. Настраиваем Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 5. Автоматически применяем миграции при старте (создаст таблицы в БД)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// 6. Включаем Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cars API V1");
});

// ==========================================
// ЭНДПОИНТЫ (CRUD через Entity Framework Core)
// ==========================================

// GET: Получить все автомобили
app.MapGet("/api/cars", async (AppDbContext db) => {
    var cars = await db.Cars.ToListAsync();
    return Results.Ok(cars);
});

// GET: Получить по ID
app.MapGet("/api/cars/{id}", async (int id, AppDbContext db) => {
    var car = await db.Cars.FirstOrDefaultAsync(x => x.Id == id);
    if (car == null) return Results.NotFound();
    return Results.Ok(car);
});

// GET: Поиск по марке (LINQ)
app.MapGet("/api/cars/search", async (string? search, AppDbContext db) => {
    var query = db.Cars.AsQueryable();
    if (!string.IsNullOrWhiteSpace(search))
    {
        query = query.Where(x => x.Brand.ToLower().Contains(search.ToLower()));
    }
    var cars = await query.ToListAsync();
    return Results.Ok(cars);
});

// POST: Добавить новый автомобиль
app.MapPost("/api/cars", async (Car car, AppDbContext db) => {
    car.CreatedAt = DateTime.UtcNow; // Заполняем дату создания
    db.Cars.Add(car);
    await db.SaveChangesAsync();
    return Results.Created($"/api/cars/{car.Id}", car);
});

// PUT: Обновить существующий
app.MapPut("/api/cars/{id}", async (int id, Car updatedCar, AppDbContext db) => {
    var car = await db.Cars.FirstOrDefaultAsync(x => x.Id == id);
    if (car == null) return Results.NotFound();

    car.Brand = updatedCar.Brand;
    car.Model = updatedCar.Model;
    car.Year = updatedCar.Year;
    car.PricePerDay = updatedCar.PricePerDay;
    car.IsAvailable = updatedCar.IsAvailable;

    await db.SaveChangesAsync();
    return Results.Ok(car);
});

// DELETE: Удалить автомобиль
app.MapDelete("/api/cars/{id}", async (int id, AppDbContext db) => {
    var car = await db.Cars.FirstOrDefaultAsync(x => x.Id == id);
    if (car == null) return Results.NotFound();

    db.Cars.Remove(car);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// --- Настройка порта для Render ---
if (app.Environment.IsDevelopment())
{
    app.Run();
}
else
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
    app.Run($"http://0.0.0.0:{port}");
}