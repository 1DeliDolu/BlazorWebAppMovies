using BlazorWebAppMovies.Components;
using BlazorWebAppMovies.Data;
using BlazorWebAppMovies.Hubs;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using BlazorSignalRApp.Data; // <— User CRUD için DbContext

var builder = WebApplication.CreateBuilder(args);

// 🎬 Ana film veritabanı bağlantısı (mevcut)
builder.Services.AddDbContextFactory<BlazorWebAppMoviesContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("BlazorWebAppMoviesContext")
        ?? throw new InvalidOperationException("Connection string 'BlazorWebAppMoviesContext' not found.")
    ));

// 👤 User tablosu için ek DbContextFactory (aynı veritabanına erişecek)
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("BlazorWebAppMoviesContext")
    ));

// QuickGrid ve EF araçları
builder.Services.AddQuickGridEntityFrameworkAdapter();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Razor ve interaktif bileşenler
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

// 🔔 SignalR kurulumu
builder.Services.AddSignalR();


// ✅ Bunu ekle
builder.Services.AddControllers();

// 🔽 Response Compression (SignalR için gerekli)
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(["application/octet-stream"]);
});

var app = builder.Build();

// Response Compression’ı etkinleştir
app.UseResponseCompression();

// ✅ HTTP Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
else
{
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// 💬 SignalR Hub
app.MapHub<ChatHub>("/chathub");

// 🧱 CRUD sayfaları için varsayılan rota (isteğe bağlı)
app.MapControllers();

app.Run();
