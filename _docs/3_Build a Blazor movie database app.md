# 🎬 Blazor film veritabanı uygulaması oluşturma (Bölüm 1 - Blazor Web Uygulaması oluşturma)

## 🧰 Araçlarınızı seçin

Bu makale, bir film veritabanını yönetme özelliklerine sahip bir ASP.NET Core Blazor Web Uygulaması oluşturmanın temellerini öğreten Blazor film veritabanı uygulaması eğitim serisinin ilk bölümüdür.

Bu bölüm, **statik sunucu tarafı işleme (static SSR)** kullanan bir Blazor Web Uygulaması oluşturmayı kapsar.

Statik SSR, içeriğin sunucuda işlenip istemciye bireysel istekler üzerine görüntülenmek üzere gönderilmesi anlamına gelir.

---

## ⚙️ Önkoşullar

* **.NET SDK (en son sürüm)**

.NET CLI, .NET SDK’nın bir parçasıdır. Projeyi etkileyen komutları çalıştırmak için komut kabuğunu (terminali) proje kök klasöründe açın.

---

## 🆕 Blazor Web Uygulaması oluşturma

1. En son  **.NET SDK** ’nın kurulu olduğundan emin olun.
2. Komut kabuğunda şu adımları izleyin:
   * `cd` komutunu kullanarak proje klasörünü oluşturmak istediğiniz dizine gidin.

     ```bash
     cd c:/users/Bernie_Kopell/Documents
     ```
   * `dotnet new` komutuyla yeni bir **Blazor Web App** projesi oluşturun.

     `-o` seçeneği, projenin oluşturulacağı yeni klasör adını belirtir.
     Proje adını **BlazorWebAppMovies** olarak yazın (büyük/küçük harf dahil), böylece öğreticideki ad alanları (namespace) ile eşleşir.

     ```bash
     dotnet new blazor -o BlazorWebAppMovies
     ```

---

## ▶️ Uygulamayı çalıştırma

Proje kök klasöründeki komut kabuğunda uygulamayı derleyip başlatmak için aşağıdaki komutu çalıştırın:

```bash
dotnet watch
```

Uygulama derlenir ve **[http://localhost:{PORT}](http://localhost:%7BPORT%7D/)** adresinde çalıştırılır.

`{PORT}`, uygulama oluşturulurken rastgele atanan bağlantı noktasıdır.

Yerel bağlantı noktası çakışması durumunda, portu **Properties/launchSettings.json** dosyasında değiştirebilirsiniz.

Uygulamanın sayfalarında gezinin ve normal çalıştığını doğrulayın.

---

## ⏹️ Uygulamayı durdurma

Uygulamayı durdurmak için:

* Tarayıcı penceresini kapatın.
* Komut kabuğunda **Ctrl+C** tuşlarına basın.

---

## 📂 Proje dosyalarını inceleme

Aşağıdaki bölümler, projenin klasör ve dosya yapısına genel bir bakış sunar.

> Eğer uygulamayı inşa ediyorsanız, bu dosyalarda değişiklik yapmanız gerekmez.
>
> Yalnızca okuyorsanız, tamamlanmış örnek uygulamayı GitHub’daki **dotnet/blazor-samples** deposundan inceleyebilirsiniz.
>
> Bu eğitimin proje klasörünün adı  **BlazorWebAppMovies** ’tir.

---

### 🧾 Properties klasörü

**launchSettings.json** dosyasını içerir.

Bu dosya, geliştirme ortamı yapılandırmalarını tutar.

---

### 🌐 wwwroot klasörü

Görseller, JavaScript (.js) ve CSS (.css) dosyaları gibi statik içerikleri içerir.

---

### 🧩 Components, Components/Pages ve Components/Layout klasörleri

Bu klasörler, **Razor bileşenlerini** (component) ve destekleyici dosyaları içerir.

Bir bileşen, kullanıcı arayüzünün (UI) kendi içinde bir bölümüdür ve gerekirse mantık (C# kodu) içerebilir.

Bileşenler `.razor` uzantılı dosyalarda C# ve HTML birleşimiyle oluşturulur.

* **Components** :

  Başka bileşenlerin içinde kullanılan ve URL üzerinden doğrudan erişilemeyen bileşenler.

* **Components/Pages** :

  URL aracılığıyla yönlendirilebilen (routable) bileşenler.

* **Components/Layout** :
* `MainLayout.razor`: Uygulamanın ana düzeni
* `MainLayout.razor.css`: Ana düzenin stil dosyası
* `NavMenu.razor`: Yan gezinme menüsü bileşeni (NavLink kullanır)
* `NavMenu.razor.css`: Menü stili

---

### ⚙️ Components/_Imports.razor dosyası

Razor bileşenlerinde ortak olarak kullanılacak yönergeleri içerir.

Razor yönergeleri, `@` ile başlayan özel anahtar kelimelerdir.

---

### 🏁 Components/App.razor dosyası

**App** bileşeni uygulamanın kök bileşenidir ve şu bölümleri içerir:

* HTML işaretlemesi
* **Routes** bileşeni
* **Blazor script etiketi** (`<script src="blazor.web.js">`)

Bu bileşen, uygulama yüklendiğinde ilk çalıştırılan bileşendir.

---

### 🗺️ Components/Routes.razor dosyası

Uygulamanın yönlendirmesini (routing) yapılandırır.

---

### ⚙️ appsettings.json dosyası

Bağlantı dizeleri gibi yapılandırma verilerini içerir.

⚠️ **Uyarı:**

İstemci tarafı kodunda aşağıdakileri asla saklamayın:

* Uygulama sırları
* Bağlantı dizeleri
* Kimlik bilgileri
* Parolalar veya PIN’ler
* Özel anahtarlar veya token’lar

Yerel geliştirme dışında, güvenli kimlik doğrulama akışlarını kullanın.

Yerel ortamda test için gizli verileri saklamak adına **Secret Manager** aracını kullanın.

---

### 🧠 Program.cs dosyası

Uygulamanın oluşturulması ve HTTP istek hattının yapılandırılması için kod içerir.

Satır sıralaması .NET sürümüne göre değişiklik gösterebilir.

#### 🔹 Uygulama oluşturma

```csharp
var builder = WebApplication.CreateBuilder(args);
```

#### 🔹 Razor bileşen hizmetleri ekleme

```csharp
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
```

#### 🔹 Uygulamayı oluşturma

```csharp
var app = builder.Build();
```

#### 🔹 HTTP istek hattını yapılandırma

```csharp
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
```

#### 🔹 HTTPS yönlendirmesi

```csharp
app.UseHttpsRedirection();
```

#### 🔹 CSRF (Antiforgery) koruması

```csharp
app.UseAntiforgery();
```

#### 🔹 Statik dosyaları eşleme

```csharp
app.MapStaticAssets();
```

#### 🔹 Razor bileşenlerini eşleme

```csharp
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
```

> 💡 Not:
>
> `AddInteractiveServerComponents` ve `AddInteractiveServerRenderMode` yöntemleri, uygulamayı **etkileşimli SSR** için hazırlar.
>
> Ancak bu özellik, eğitimin son bölümlerinde kullanılacaktır. Şu anda uygulama yalnızca **statik SSR** kullanır.

#### 🔹 Uygulamayı çalıştırma

```csharp
app.Run();
```

---

## 🧩 Sorun giderme

Eğitimi takip ederken çözülmeyen bir hata ile karşılaşırsanız kodunuzu şu örnekle karşılaştırın:

**[Blazor örnekleri GitHub deposu (dotnet/blazor-samples)](https://github.com/dotnet/blazor-samples)**

Proje klasörü: **BlazorWebAppMovies**

---

## 📚 Ek kaynaklar

Bu eğitimde kolaylık sağlamak amacıyla **HTTP** protokolü kullanılır.

**Linux** ve **macOS** kullanıcıları için HTTPS geçişini kolaylaştırmak adına SSL varsayılan olarak devre dışıdır.

Daha fazla bilgi için şu belgeye bakın:

👉 [ASP.NET Core’da HTTPS’i zorunlu kılma (Enforce HTTPS)](https://learn.microsoft.com/aspnet/core/security/enforcing-ssl)
