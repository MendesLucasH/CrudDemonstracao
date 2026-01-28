using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

// Railway provides a PORT environment variable
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    builder.WebHost.UseUrls($"http://+:{port}");
}

// Resolve connection string: Railway env vars > appsettings.json
var connectionString = BuildConnectionString(builder.Configuration);
CrudDemonstracao.Data.Db.ConnectionString = connectionString!;

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Auto-initialize database schema on startup
await InitializeDatabase(connectionString!);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Skip HTTPS redirection when behind Railway's reverse proxy
if (string.IsNullOrEmpty(port))
{
    app.UseHttpsRedirection();
}

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();

static string? BuildConnectionString(IConfiguration configuration)
{
    // 1. Check for Railway individual MySQL variables
    var mysqlHost = Environment.GetEnvironmentVariable("MYSQLHOST");
    if (!string.IsNullOrEmpty(mysqlHost))
    {
        var mysqlPort = Environment.GetEnvironmentVariable("MYSQLPORT") ?? "3306";
        var mysqlDb = Environment.GetEnvironmentVariable("MYSQLDATABASE") ?? "railway";
        var mysqlUser = Environment.GetEnvironmentVariable("MYSQLUSER") ?? "root";
        var mysqlPwd = Environment.GetEnvironmentVariable("MYSQLPASSWORD") ?? "";
        return $"Server={mysqlHost};Port={mysqlPort};Database={mysqlDb};Uid={mysqlUser};Pwd={mysqlPwd};";
    }

    // 2. Check for MYSQL_URL (mysql://user:password@host:port/database)
    var mysqlUrl = Environment.GetEnvironmentVariable("MYSQL_URL");
    if (!string.IsNullOrEmpty(mysqlUrl))
    {
        var uri = new Uri(mysqlUrl);
        var userInfo = uri.UserInfo.Split(':');
        var user = userInfo[0];
        var pwd = userInfo.Length > 1 ? userInfo[1] : "";
        var database = uri.AbsolutePath.TrimStart('/');
        return $"Server={uri.Host};Port={uri.Port};Database={database};Uid={user};Pwd={pwd};";
    }

    // 3. Fallback to appsettings.json
    return configuration.GetConnectionString("MySqlConexao");
}

static async Task InitializeDatabase(string connectionString)
{
    var schemaPath = Path.Combine(AppContext.BaseDirectory, "Database", "schema.sql");
    if (!File.Exists(schemaPath))
    {
        Console.WriteLine("schema.sql not found, skipping database initialization.");
        return;
    }

    // Retry logic: the database may not be ready immediately on container startup
    for (int attempt = 1; attempt <= 10; attempt++)
    {
        try
        {
            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            var sql = await File.ReadAllTextAsync(schemaPath);
            using var cmd = new MySqlCommand(sql, connection);
            await cmd.ExecuteNonQueryAsync();

            Console.WriteLine("Database schema initialized successfully.");
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database init attempt {attempt}/10 failed: {ex.Message}");
            if (attempt < 10)
                await Task.Delay(3000);
        }
    }

    Console.WriteLine("WARNING: Could not initialize database schema after 10 attempts.");
}
