# 🎬 Blazor film veritabanı uygulaması oluşturma (Bölüm 1 - Blazor Web Uygulaması oluşturma)

## 🧰 Araçlarınızı seçin

Bu makale, bir film veritabanını yönetme özelliklerine sahip bir ASP.NET Core Blazor Web Uygulaması oluşturmanın temellerini öğreten Blazor film veritabanı uygulaması eğitim serisinin ilk bölümüdür.

Bu bölüm, **statik sunucu tarafı işleme (static SSR)** kullanan bir Blazor Web Uygulaması oluşturmayı kapsar. Statik SSR, içeriğin sunucuda işlenip istemciye bireysel istekler üzerine görüntülenmek üzere gönderilmesi anlamına gelir.

---

## ⚙️ Önkoşullar

Aşağıdakilerin en son sürümleri:

* **Visual Studio Code**
* **C# Dev Kit**
* **.NET SDK**

Bu eğitimde ASP.NET Core geliştirme için Visual Studio Code (VS Code) kullanılır ve **.NET CLI** komutları (dotnet komutları) VS Code’un entegre Terminal penceresinde (varsayılan olarak PowerShell kabuğu) çalıştırılır. Terminal’i açmak için menü çubuğundan **Terminal > New Terminal** seçin.

---

## 🆕 Blazor Web Uygulaması oluşturma

Bu eğitim, VS Code’a aşina olduğunuzu varsayar. VS Code’a yeniyseniz, [VS Code belgelerine](https://code.visualstudio.com/docs) bakabilirsiniz.

1. En son **C# Dev Kit** ve  **.NET SDK** ’nın yüklü olduğundan emin olun.
2. VS Code’da:
   * **Explorer** görünümüne gidin ve **Create .NET Project** düğmesini seçin.
   * Alternatif olarak, **Ctrl+Shift+P** kısayoluyla  **Command Palette** ’i açın, “.NET” yazın ve **.NET: New Project** komutunu seçin.
3. **Blazor Web App** proje şablonunu seçin.
4. **Project Location** penceresinde proje klasörünüzü oluşturun veya seçin.
5. **Command Palette** ’te projeyi şu adla adlandırın:

   **BlazorWebAppMovies** (büyük/küçük harf eşleşmesi önemlidir).

1. **Create project** seçeneğini tıklayarak uygulamayı oluşturun.

---

## ▶️ Uygulamayı çalıştırma

* VS Code’da **F5** tuşuna basın.
* Üstteki  **Command Palette** ’teki hata ayıklayıcı seçim penceresinde  **C#** ’ı seçin.
* Varsayılan yapılandırmayı seçin ( **C#: BlazorWebAppMovies [Default Configuration]** ).

Varsayılan tarayıcı, uygulamanın kullanıcı arayüzünü göstermek için `http://localhost:{PORT}` adresinde açılır. `{PORT}`, uygulama oluşturulduğunda rastgele atanır.

Port çakışması yaşarsanız, **Properties/launchSettings.json** dosyasındaki portu değiştirin.

Uygulamanın sayfalarında gezinin ve düzgün çalıştığını doğrulayın.

---

## ⏹️ Uygulamayı durdurma

Uygulamayı durdurmak için:

* Tarayıcı penceresini kapatın.
* VS Code’da:
  * **Run > Stop Debugging** menüsünü seçin
  * veya **Shift+F5** tuşlarına basın.

---

## 📂 Proje dosyalarını inceleme

Aşağıdaki bölümler proje klasörlerinin ve dosyalarının genel bir açıklamasını içerir.

Uygulamayı oluşturuyorsanız, bu dosyalarda değişiklik yapmanız gerekmez.

Yalnızca makaleyi okuyorsanız, tamamlanmış örnek uygulamayı şu adreste inceleyebilirsiniz:

**[Blazor örnekleri GitHub deposu (dotnet/blazor-samples)](https://github.com/dotnet/blazor-samples)**

Bu eğitimin proje klasörünün adı  **BlazorWebAppMovies** ’tir.

---

### 🧾 Properties klasörü

**launchSettings.json** dosyasını içeren geliştirme ortamı yapılandırmasını tutar.

---

### 🌐 wwwroot klasörü

Görseller, JavaScript (.js) ve stil sayfası (.css) gibi statik varlıkları içerir.

---

### 🧩 Components, Components/Pages ve Components/Layout klasörleri

Bu klasörler, **Razor bileşenlerini** ve destekleyici dosyaları içerir.

Bir bileşen, kullanıcı arayüzünün (UI) kendi içinde bir bölümüdür ve C# + HTML kullanılarak oluşturulur (`.razor` uzantılı dosyalar).

* **Components** : Diğer bileşenlere gömülü olan, URL’den doğrudan erişilemeyen bileşenler.
* **Components/Pages** : URL üzerinden yönlendirilebilen (“routable”) bileşenler.
* **Components/Layout** :
* `MainLayout.razor`: Ana düzen bileşeni
* `MainLayout.razor.css`: Ana düzen için stil sayfası
* `NavMenu.razor`: Yan menü bileşeni (NavLink öğeleri içerir)
* `NavMenu.razor.css`: Menü stili

---

### ⚙️ Components/_Imports.razor dosyası

Razor bileşenlerinde ortak olarak kullanılacak **Razor yönergelerini** içerir.

Razor yönergeleri `@` ile başlayan özel anahtar kelimelerdir.

---

### 🏁 Components/App.razor dosyası

**App** bileşeni uygulamanın kök bileşenidir ve şunları içerir:

* HTML işaretlemesi
* **Routes** bileşeni
* **Blazor script etiketi** (`<script src="blazor.web.js">`)

Uygulama başlatıldığında yüklenen ilk bileşendir.

---

### 🗺️ Components/Routes.razor dosyası

Uygulamanın yönlendirmesini (routing) yapılandırır.

---

### ⚙️ appsettings.json dosyası

Bağlantı dizeleri gibi yapılandırma verilerini içerir.

⚠️ **Uyarı:**

Uygulamanın istemci tarafı kodunda hiçbir zaman aşağıdakileri saklamayın:

* Uygulama sırları
* Bağlantı dizeleri
* Kimlik bilgileri
* Parolalar
* PIN’ler
* Özel anahtarlar veya token’lar

Yerel geliştirme dışındaki ortamlarda **güvenli kimlik doğrulama akışları** kullanın.

Yerel geliştirmede gizli veriler için **Secret Manager** aracını tercih edin.

---

### 🧠 Program.cs dosyası

Uygulamayı oluşturmak ve istek işleme hattını yapılandırmak için gereken kodu içerir.

Satırların sırası .NET sürümlerine göre değişebilir.

#### 🔹 Uygulama oluşturma

```csharp
var builder = WebApplication.CreateBuilder(args);
```

#### 🔹 Razor bileşen servisleri ekleme

```csharp
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
```

#### 🔹 Uygulamayı oluşturma

```csharp
var app = builder.Build();
```

#### 🔹 HTTP isteği hattını yapılandırma

Geliştirme ortamında hata yönetimi ve güvenlik ara yazılımları yapılandırılır:

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

#### 🔹 Statik dosya eşleme

```csharp
app.MapStaticAssets();
```

#### 🔹 Razor bileşenlerini eşleme ve render modu

```csharp
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
```

> Not:
>
> `AddInteractiveServerComponents` ve `AddInteractiveServerRenderMode` yöntemleri, uygulamayı **interaktif SSR** (son bölümde işlenecek) için hazırlar.
>
> Şimdilik uygulama **statik SSR** kullanır.

#### 🔹 Uygulamayı çalıştırma

```csharp
app.Run();
```

---

## 🧩 Sorun giderme

Bir hata ile karşılaşırsanız kodunuzu aşağıdaki tamamlanmış proje ile karşılaştırın:

**[Blazor samples GitHub repository (dotnet/blazor-samples)](https://github.com/dotnet/blazor-samples)**

Proje klasörü adı: **BlazorWebAppMovies**

---

## 📚 Ek kaynaklar

Bu eğitimde kolaylık sağlamak için varsayılan olarak **HTTP** protokolü kullanılır.

SSL/HTTPS etkinleştirmek için şu belgeye bakın:

**[ASP.NET Core’da HTTPS’i zorunlu kılma (Enforce HTTPS)](https://learn.microsoft.com/aspnet/core/security/enforcing-ssl)**
