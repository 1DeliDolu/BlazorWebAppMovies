using BlazorWebAppMovies.Components;
using BlazorWebAppMovies.Data;
using BlazorWebAppMovies.Hubs;
using BlazorSignalRApp.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🎬 Ana film veritabanı bağlantısı (mevcut)
builder.Services.AddDbContextFactory<BlazorWebAppMoviesContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("BlazorWebAppMoviesContext")
        ?? throw new InvalidOperationException("Connection string 'BlazorWebAppMoviesContext' not found.")
    ));

// 👤 Identity + chat profilleri için veritabanı
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BlazorWebAppMoviesContext")));
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BlazorWebAppMoviesContext")),
    ServiceLifetime.Scoped);

// QuickGrid ve EF araçları
builder.Services.AddQuickGridEntityFrameworkAdapter();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Identity & kimlik doğrulama
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

AuthenticationBuilder? externalAuthenticationBuilder = null;

var googleClientId = builder.Configuration["Authentication:Google:ClientId"];
var googleClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
if (!string.IsNullOrWhiteSpace(googleClientId) && !string.IsNullOrWhiteSpace(googleClientSecret))
{
    externalAuthenticationBuilder ??= builder.Services.AddAuthentication();
    externalAuthenticationBuilder.AddGoogle(options =>
    {
        options.ClientId = googleClientId;
        options.ClientSecret = googleClientSecret;
    });
}

var microsoftClientId = builder.Configuration["Authentication:Microsoft:ClientId"];
var microsoftClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"];
if (!string.IsNullOrWhiteSpace(microsoftClientId) && !string.IsNullOrWhiteSpace(microsoftClientSecret))
{
    externalAuthenticationBuilder ??= builder.Services.AddAuthentication();
    externalAuthenticationBuilder.AddMicrosoftAccount(options =>
    {
        options.ClientId = microsoftClientId;
        options.ClientSecret = microsoftClientSecret;
    });
}

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

// Razor ve interaktif bileşenler
builder.Services.AddRazorPages();
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
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorPages();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// 💬 SignalR Hub
app.MapHub<ChatHub>("/chathub").RequireAuthorization();

// 🧱 CRUD sayfaları için varsayılan rota (isteğe bağlı)
app.MapControllers();

app.Run();
