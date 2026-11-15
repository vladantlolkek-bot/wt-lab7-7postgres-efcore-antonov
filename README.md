# WT-7: PostgreSQL и Entity Framework Core

**Дисциплина:** Web-технологии
**Студент:** Антонов В.
**Тема:** ORM, Code First, миграции и публикация на Render

## Описание проекта
REST API на базе ASP.NET Core Minimal API для предметной области **«Прокат автомобилей»**. 
В данной лабораторной работе хранение данных перенесено из оперативной памяти (WT-6) в реляционную базу данных **PostgreSQL** с использованием **Entity Framework Core (Code First)**.

## Технологии
*   ASP.NET Core 8.0 (Minimal API)
*   Entity Framework Core 8.0
*   Npgsql (провайдер для PostgreSQL)
*   Swagger (OpenAPI) для документации
*   Docker
*   Render (PostgreSQL + Web Service)

## Функциональность API
| Метод | URL | Назначение | Успешный статус |
| :--- | :--- | :--- | :--- |
| GET | `/api/cars` | Получить список всех авто | 200 OK |
| GET | `/api/cars/{id}` | Получить авто по ID | 200 OK / 404 |
| GET | `/api/cars/search?search=bmw` | Поиск по марке (LINQ) | 200 OK |
| POST | `/api/cars` | Создать новое авто | 201 Created |
| PUT | `/api/cars/{id}` | Обновить авто по ID | 200 OK / 404 |
| DELETE | `/api/cars/{id}` | Удалить авто по ID | 204 No Content / 404 |

## Миграции и база данных
В проекте настроен подход **Code First**.
*   Настроен `AppDbContext` для связи с PostgreSQL.
*   Создана миграция `InitialCreate` (создание таблицы `Cars`).
*   Создана миграция `SeedCars` (добавление начальных данных: Toyota, BMW).
*   В `Program.cs` настроено **автоматическое применение миграций** при запуске приложения (`db.Database.Migrate()`).

## Локальный запуск
1. Установите PostgreSQL и создайте базу данных `wt_lab7`.
2. В файле `appsettings.Development.json` укажите свою строку подключения (пароль от БД).
3. Примените миграции:
   ```bash
   dotnet ef database update