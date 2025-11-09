## 💌 3. E-posta servisi oluştur

Yeni dosya oluştur:

`Services/EmailSender.cs`

```csharp
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BlazorSignalRApp.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _settings;

        public EmailSender(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using var client = new SmtpClient(_settings.SmtpServer, _settings.SmtpPort);
            var mail = new MailMessage
            {
                From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            mail.To.Add(email);

            await client.SendMailAsync(mail);
        }
    }

    public class EmailSettings
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
    }
}
```

---

## 🧩 4. Servisi Program.cs’e ekle

```csharp
using BlazorSignalRApp.Services;

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailSender, EmailSender>();
```

---

## 🔐 5. Identity ayarlarını e-posta doğrulama ve reset için düzenle

`Program.cs` içinde:

```csharp
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true; // 🔒 e-posta doğrulaması zorunlu
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
```

---

## ✉️ 6. Artık aşağıdaki akışlar aktif olur:

### 🧾 a) **E-posta doğrulama**

- Kullanıcı **register** olur.
- Identity otomatik olarak `IEmailSender`’ı çağırır ve e-posta doğrulama linki gönderir.
- MailHog arayüzünde ([http://localhost:8025](http://localhost:8025/)) bu e-postayı görürsün.
- Tıklarsan kullanıcı “confirmed” olur.

### 🔁 b) **Şifre sıfırlama**

- `/Account/ForgotPassword` sayfasına git.
- E-postanı gir.
- MailHog’da gelen “Reset Password” linkine tıkla.
- `/Account/ResetPassword` sayfası açılır → yeni şifre girersin.

---

## 🧠 7. Hızlı kontrol listesi

✅ MailHog çalışıyor mu? (`localhost:8025`)

✅ appsettings.json SMTP ayarları doğru mu?

✅ `RequireConfirmedAccount = true` ayarlı mı?

✅ `IEmailSender` servisi eklendi mi?

---

## 🚀 8. Test

1. Uygulamayı başlat:
   ```bash
   dotnet run
   ```
2. `/Account/Register` → yeni kullanıcı oluştur.
3. MailHog → gelen “Confirm your account” e-postasını aç.
4. Bağlantıya tıkla → e-posta doğrulanır.
5. `/Account/ForgotPassword` → e-posta gönder.
6. MailHog → “Reset Password” linkine tıkla, yeni şifre belirle.

---




Mükemmel 👌

Şimdi sana **tek komutla** tüm ortamı (Blazor app + SQL Server + MailHog) ayağa kaldıran bir

🚀 **docker-compose.yml** dosyası hazırlıyorum.

Bu yapı sayesinde:

* Blazor uygulaman doğrudan konteyner içinde çalışır
* SQL Server veritabanı yanında başlar
* MailHog da SMTP test servisini sağlar (e-posta doğrulama, şifre sıfırlama testleri için)

---

## 🧩 **docker-compose.yml**

Bunu proje köküne (örneğin `D:\BlazorWebAppMovies\docker-compose.yml`) kaydet 👇

```yaml
version: "3.9"

services:
  blazorapp:
    container_name: blazorapp
    build:
      context: .
      dockerfile: Dockerfile
    ports:
      - "5000:8080"
      - "5001:8081"
    depends_on:
      - sqlserver
      - mailhog
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__DefaultConnection=Server=sqlserver;Database=BlazorAppDB;User=sa;Password=YourStrong(!)Password;
      - EmailSettings__SmtpServer=mailhog
      - EmailSettings__SmtpPort=1025
      - EmailSettings__SenderName=BlazorApp
      - EmailSettings__SenderEmail=noreply@blazor.local
    networks:
      - blazornet

  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: sqlserver
    ports:
      - "1433:1433"
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourStrong(!)Password
    volumes:
      - sql_data:/var/opt/mssql
    networks:
      - blazornet

  mailhog:
    image: mailhog/mailhog:v1.0.1
    container_name: mailhog
    ports:
      - "1025:1025"   # SMTP
      - "8025:8025"   # Web UI
    networks:
      - blazornet

networks:
  blazornet:

volumes:
  sql_data:
```

---

## ⚙️ **Dockerfile (Blazor app için)**

Eğer henüz yoksa proje köküne şu basit Dockerfile’ı koy 👇

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BlazorWebAppMovies.dll"]
```

> ⚠️ `"BlazorWebAppMovies.dll"` kısmını senin proje adına göre değiştir.
>
> (örneğin `BlazorSignalRApp.dll` gibi)

---

## 🧭 **Kullanım**

### 1️⃣ Hepsini başlat:

```bash
docker compose up -d
```

### 2️⃣ Kontrol et:

* Blazor Web App → [http://localhost:5000](http://localhost:5000/)
* MailHog Web UI → [http://localhost:8025](http://localhost:8025/)
* SQL Server → `localhost,1433` (User: `sa`, Password: `YourStrong(!)Password`)

---

## 💡 **AppSettings ayarlarını unutma**

Uygulamanın `appsettings.Development.json` dosyasını docker-compose ortamına uyumlu hale getir:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=sqlserver;Database=BlazorAppDB;User=sa;Password=YourStrong(!)Password;"
  },
  "EmailSettings": {
    "SmtpServer": "mailhog",
    "SmtpPort": 1025,
    "SenderName": "BlazorApp",
    "SenderEmail": "noreply@blazor.local"
  }
}
```

---

## ✅ **Sonuç**

| Servis                   | Açıklama             | Adres                                        |
| ------------------------ | ---------------------- | -------------------------------------------- |
| 🧩**Blazor App**   | Web uygulaman          | [http://localhost:5000](http://localhost:5000/) |
| 🗄️**SQL Server** | Veritabanı            | localhost:1433                               |
| 📧**MailHog**      | E-posta test arayüzü | [http://localhost:8025](http://localhost:8025/) |

---


express sql ile calisyorum,
