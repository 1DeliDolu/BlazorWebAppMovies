# 🎬 Blazor film veritabanı uygulaması oluşturma (Bölüm 2 - Model ekleme ve iskelet oluşturma)

## 🧰 Araçlarınızı seçin

Bu makale, film veritabanı yönetimi özelliklerine sahip bir **ASP.NET Core Blazor Web Uygulaması** oluşturmayı öğreten eğitim serisinin ikinci bölümüdür.

Bu bölümde:

* Veritabanındaki filmi temsil eden bir **sınıf** eklenir.
* **Entity Framework Core (EF Core)** servisleri ve araçları kullanılarak veritabanı bağlamı (DbContext) ve veritabanı oluşturulur.
* Ek araçlar sayesinde **Razor bileşen tabanlı kullanıcı arayüzü** otomatik olarak oluşturulur (scaffold edilir).

---

## 🎞️ Veri modeli ekleme

1. Projeye **Models** adlı bir klasör ekleyin.
2. Bu klasöre **Movie.cs** adlı bir sınıf dosyası oluşturun.
3. Dosyanın içeriğini aşağıdaki şekilde düzenleyin:

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorWebAppMovies.Models;

public class Movie
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public DateOnly ReleaseDate { get; set; }

    public string? Genre { get; set; }

    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }
}
```

### 🎬 Movie sınıfının özellikleri

* **Id:** EF Core ve veritabanı tarafından her kayıt için benzersiz kimlik olarak kullanılır (Primary Key).
* **Title:** Filmin adı
* **ReleaseDate:** Yayın tarihi
* **Genre:** Tür
* **Price:** Fiyat

❓ `string?` ifadesi, özelliğin **null** değer alabileceğini belirtir (nullable).

EF Core, model özelliklerinin .NET türlerine göre veritabanı sütun türlerini otomatik olarak belirler.

Ayrıca `System.ComponentModel.DataAnnotations` nitelikleriyle (annotations) sağlanan ek meta verileri de dikkate alır.

Bir nitelik, aşağıdaki biçimde tanımlanır:

```csharp
[{ANNOTATION}]
```

### 💰 Price özelliği için ek açıklamalar

```csharp
[DataType(DataType.Currency)]
[Column(TypeName = "decimal(18, 2)")]
public decimal Price { get; set; }
```

Bu açıklamalar şunları belirtir:

* Özelliğin bir **para birimi** türü olduğunu.
* Veritabanı sütununun **ondalık (decimal)** türünde, 18 haneli ve 2 ondalık basamaklı olduğunu.

> 💡 İlerleyen bölümlerde, veri doğrulaması için kullanılacak ek açıklamalar (validation attributes) da ele alınacaktır.

---

## 📦 NuGet paketlerini ve araçlarını ekleme

Proje kök dizininde bir komut kabuğu (terminal) açın ve aşağıdaki komutları çalıştırın:

```bash
dotnet tool install --global dotnet-aspnet-codegenerator
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.SQLite
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.AspNetCore.Components.QuickGrid
dotnet add package Microsoft.AspNetCore.Components.QuickGrid.EntityFrameworkAdapter
dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
```

Komutları yapıştırdığınızda, terminal bir uyarı gösterebilir (birden fazla komut çalışacak).

Uyarıyı onaylayın ve işlemi tamamlayın.

Son komut, **Enter** tuşuna bastığınızda çalışacaktır.

✅ Komutlar tamamlandıktan sonra proje dosyasını kaydedin.

### 📘 Eklenen paketlerin açıklaması

* **dotnet-ef:** EF Core CLI araçları
* **dotnet-aspnet-codegenerator:** Kod iskeleti oluşturma (scaffolding) aracı
* **Microsoft.EntityFrameworkCore.SQLite / SqlServer:** Veritabanı sağlayıcıları
* **Microsoft.EntityFrameworkCore.Tools:** Tasarım zamanı araçları
* **Microsoft.VisualStudio.Web.CodeGeneration.Design:** Scaffold desteği
* **Microsoft.AspNetCore.Components.QuickGrid:** Hızlı tablo bileşeni
* **Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore:** Hata ayıklama ve veritabanı hatası yakalama

---

## 🧱 Uygulamayı derleme

Proje kök klasöründe aşağıdaki komutu çalıştırın:

```bash
dotnet build
```

Derlemenin başarılı olduğunu doğrulayın.

---

## ⚙️ Modelin iskeletini oluşturma (Scaffold etme)

Bu adımda, **Movie** modeli kullanılarak:

* Veritabanı bağlamı (DbContext)
* CRUD (Create, Read, Update, Delete) işlemleri için kullanıcı arayüzü bileşenleri

otomatik olarak oluşturulur.

.NET iskelet oluşturma aracı (scaffolding), veri modelleriyle etkileşime geçmek için gerekli kodu hızlıca eklemenizi sağlar.

Aşağıdaki komutu proje kök klasöründe çalıştırın:

```bash
dotnet aspnet-codegenerator blazor CRUD -dbProvider sqlite -dc BlazorWebAppMovies.Data.BlazorWebAppMoviesContext -m Movie -outDir Components/Pages
```

### 🧩 Parametrelerin açıklaması

| Parametre       | Açıklama                                                                                        |
| --------------- | ------------------------------------------------------------------------------------------------- |
| `-dbProvider` | Veritabanı sağlayıcısı (sqlite, sqlserver, cosmos, postgres).                                |
| `-dc`         | Kullanılacak DbContext sınıfı (örnek:`BlazorWebAppMovies.Data.BlazorWebAppMoviesContext`). |
| `-m`          | Model adı (`Movie`).                                                                           |
| `-outDir`     | Oluşturulacak bileşenlerin hedef klasörü (`Components/Pages`).                              |

Bu komut,  **Create** ,  **Read** ,  **Update** ,  **Delete** , **Details** ve **Index** bileşenlerini oluşturur.

---

## ⚙️ appsettings.json dosyası

İskelet oluşturucu, yerel veritabanı bağlantı dizesini **appsettings.json** dosyasına ekler:

```json
"ConnectionStrings": {
  "BlazorWebAppMoviesContext": "{CONNECTION STRING}"
}
```

---

⚠️ **Uyarı:**

İstemci tarafı kodunda aşağıdaki bilgileri  **asla saklamayın** :

* Uygulama sırları
* Bağlantı dizeleri
* Parolalar, kimlik bilgileri, PIN’ler
* Özel anahtarlar veya token’lar

Üretim ortamlarında güvenli kimlik doğrulama yöntemleri kullanın.

Yerel geliştirme testlerinde gizli verileri yönetmek için **Secret Manager** aracını kullanın.

Daha fazla bilgi için:

🔗 **Gizli verileri ve kimlik bilgilerini güvenli şekilde yönetme** (Securely maintain sensitive data and credentials)


# 🎬 Blazor film veritabanı uygulaması oluşturma (Bölüm 2 - İskelet oluşturma sonucu dosyalar ve veritabanı işlemleri)

## 🗂️ İskelet oluşturma (Scaffolding) ile oluşturulan dosyalar

İskelet oluşturma işlemi aşağıdaki bileşen dosyalarını ve **film veritabanı bağlamı sınıfını** oluşturur:

📁 **Components/Pages/MoviePages**

* **Create.razor:** Yeni film kayıtları oluşturur.
* **Delete.razor:** Bir film kaydını siler.
* **Details.razor:** Film detaylarını gösterir.
* **Edit.razor:** Film kaydını günceller.
* **Index.razor:** Veritabanındaki film kayıtlarını listeler.

📄 **Data/BlazorWebAppMoviesContext.cs:** Veritabanı bağlamı (DbContext) sınıfı.

> 🎓 MoviePages klasöründeki bileşenler bir sonraki bölümde ayrıntılı olarak açıklanacaktır.
>
> Veritabanı bağlamı ise bu makalenin ilerleyen kısmında ele alınmaktadır.

---

## 💉 Bağımlılık enjeksiyonu (Dependency Injection)

ASP.NET Core, **Dependency Injection (DI)** ilkesine göre yapılandırılmıştır.

Bu, sınıflar ile onların bağımlılıkları arasında **tersine kontrol (Inversion of Control - IoC)** sağlar.

Servisler (örneğin EF Core veritabanı bağlamı), uygulama başlatılırken DI konteynerine kaydedilir ve Razor bileşenlerinde kullanılmak üzere **enjeksiyon yoluyla** alınır.

---

## ⚡ QuickGrid bileşeni

 **QuickGrid** , verileri tablo biçiminde verimli bir şekilde göstermek için kullanılan bir Razor bileşenidir.

İskelet oluşturucu, **Index.razor** bileşenine bir QuickGrid ekler.

EF Core sorgularını (IQueryable`<T>`) **asenkron** biçimde çözümleyebilmek için, `AddQuickGridEntityFrameworkAdapter` metodu ile EF Core uyarlayıcısı (adapter) hizmete eklenir.

---

## ⚙️ Geliştirici hata sayfası ve veritabanı hata filtresi

`AddDatabaseDeveloperPageExceptionFilter`, veritabanı hatalarını yakalayan bir filtre ekler.

Bu filtre, **UseDeveloperExceptionPage** ile birlikte kullanıldığında veritabanı hataları için ayrıntılı HTML hata sayfaları oluşturur.

Bu hatalar genellikle **Entity Framework Migrations** işlemleriyle çözülebilir.

---

## 🧩 Program.cs dosyasına eklenen kod

İskelet oluşturucu tarafından **Program.cs** dosyasına aşağıdaki kod eklenir:

```csharp
builder.Services.AddDbContextFactory<BlazorWebAppMoviesContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("BlazorWebAppMoviesContext") ?? 
        throw new InvalidOperationException(
            "Connection string 'BlazorWebAppMoviesContext' not found.")));

builder.Services.AddQuickGridEntityFrameworkAdapter();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
```

---

## 🏗️ EF Core ile ilk veritabanı şemasını oluşturma

### 🔹 EF Core Migration (geçiş) özelliği

EF Core’un **migrations** özelliği şunları yapar:

* İlk veritabanı şemasını oluşturur.
* Veritabanı şemasını, uygulamadaki veri modeliyle senkronize tutmak için artımlı (incremental) güncellemeler yapar.
* Mevcut veriler korunur.

### 🔹 Code-first yaklaşımı

EF Core, **code-first** yaklaşımını benimser.

Bu, veritabanı yapısının doğrudan uygulamadaki **model sınıflarından** üretilmesi anlamına gelir:

1. Önce model sınıfları oluşturulur veya güncellenir.
2. Daha sonra veritabanı bu modellere göre oluşturulur veya güncellenir.

Bu yöntem, veritabanı tasarımını manuel olarak yapma ihtiyacını ortadan kaldırır ve geliştirme sürecini hızlandırır.

---

## 🧱 İlk migration (geçiş) oluşturma

Proje kök klasöründe aşağıdaki komutu çalıştırın:

```bash
dotnet ef migrations add InitialCreate
```

Bu komut, **ilk veritabanı şemasını** oluşturmak için gerekli kodu üretir.

Şema,  **DbContext** ’te belirtilen model sınıflarına dayanır.

`InitialCreate` geçişin adıdır (isteğe bağlıdır, açıklayıcı bir ad seçilebilir).

---

## 💾 Veritabanını oluşturma

Migration tamamlandıktan sonra, aşağıdaki komutla veritabanını güncelleyin:

```bash
dotnet ef database update
```

Bu komut, oluşturulan migration dosyasındaki **Up()** metodunu çalıştırır ve veritabanını oluşturur.

Migration dosyası genellikle şu klasörde bulunur:

```
Migrations/{ZAMAN_DAMGASI}_InitialCreate.cs
```

---

## 🧠 BlazorWebAppMoviesContext.cs dosyası

Bu sınıf:

* `Microsoft.EntityFrameworkCore.DbContext` sınıfından türetilir.
* Uygulamanın **veri modeli** kapsamına alınan entity’leri (varlıkları) belirtir.
* EF Core’un CRUD işlevlerini yönetir.
* **DbSet** özelliği içerir (örneğin: `DbSet<Movie>`).
  * Her  **entity set** , veritabanında bir tabloya karşılık gelir.
  * Her  **entity** , o tablodaki bir satıra karşılık gelir.

Bağlantı dizesi (connection string), `DbContextOptions` üzerinden geçirilir ve yerel geliştirme sırasında **appsettings.json** dosyasından okunur.

---

⚠️ **Uyarı:**

İstemci tarafı kodunda aşağıdaki bilgileri  **asla saklamayın** :

* Uygulama sırları
* Bağlantı dizeleri
* Parolalar, kimlik bilgileri, PIN’ler
* Özel anahtarlar veya token’lar

Yerel geliştirme dışında, güvenli kimlik doğrulama yöntemleri kullanın.

Gizli veriler için **Secret Manager** aracını kullanmanız önerilir.

---

## 🧪 Uygulamayı test etme

1. Uygulamayı çalıştırın:

   ```bash
   dotnet watch
   ```
2. Tarayıcıda adres çubuğuna `/movies` ekleyin:

   ```
   http://localhost:{PORT}/movies
   ```
3. **Index** sayfası yüklendikten sonra **Create New** bağlantısını seçin.
4. Yeni bir film ekleyin.

   Örneğin:

   * 🎥 **Title:** The Matrix
   * 📅 **Release Date:** 1999
   * 🧬 **Genre:** Sci-Fi
   * 💰 **Price:** 9.99
5. **Create** butonuna tıkladığınızda film verileri sunucuya gönderilir ve veritabanına kaydedilir.
6. Uygulama **Index** sayfasına döndüğünde, eklenen film listede görünür.

🖊️ **Edit** sayfasından filmi düzenleyebilir,

🗑️ **Delete** sayfasından silebilirsiniz (henüz silmeyin, sonraki adımda kullanılacak).

Eğer filmi yanlışlıkla silerseniz, aynı filmi yeniden ekleyin.

---

## ⏹️ Uygulamayı durdurma

Uygulamayı kapatmak için:

* Tarayıcı penceresini kapatın.
* Komut satırında **Ctrl+C** tuşlarına basın.

---

## 🧩 Sorun giderme

Sorun yaşarsanız kodunuzu şu örnekle karşılaştırın:

**[Blazor örnekleri GitHub deposu (dotnet/blazor-samples)](https://github.com/dotnet/blazor-samples)**

Proje klasörü: **BlazorWebAppMovies**

---

## 📚 Ek kaynaklar

* [Entity Framework Core](https://learn.microsoft.com/ef/core)
* [EF Core CLI araç referansı](https://learn.microsoft.com/ef/core/cli/dotnet)
* [ASP.NET Core’da Dependency Injection](https://learn.microsoft.com/aspnet/core/fundamentals/dependency-injection)
* [ASP.NET Core Blazor QuickGrid bileşeni](https://learn.microsoft.com/aspnet/core/blazor/components/quickgrid)

---

© The Matrix, Warner Bros. Entertainment Inc.
