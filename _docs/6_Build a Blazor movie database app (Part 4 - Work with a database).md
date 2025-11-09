
# 🎬 Blazor film veritabanı uygulaması oluşturun (Bölüm 4 - Veritabanı ile çalışın)

## 🧰 Araçlarınızı seçin

Bu makale, bir film veritabanını yönetme özelliklerine sahip bir ASP.NET Core Blazor Web Uygulaması oluşturmanın temellerini öğreten Blazor film veritabanı uygulaması eğitim serisinin dördüncü bölümüdür.

Bu bölüm, veritabanı bağlamına ve veritabanının şema ve verileriyle doğrudan çalışmaya odaklanır. Veritabanının verilerle tohumlanması (seeding) da ele alınır.

## 🔐 Üretim uygulamaları için güvenli kimlik doğrulama akışı gereklidir

Bu eğitim, kullanıcı kimlik doğrulaması gerektirmeyen yerel bir veritabanı kullanır. Üretim uygulamaları, mevcut en güvenli kimlik doğrulama akışını kullanmalıdır. Dağıtılmış test ve üretim Blazor Web Uygulamaları için kimlik doğrulaması hakkında daha fazla bilgi için aşağıdaki kaynaklara bakın:

ASP.NET Core Blazor kimlik doğrulama ve yetkilendirme

Sunucu güvenliği düğümündeki aşağıdaki makalelerle birlikte ASP.NET Core Blazor kimlik doğrulama ve yetkilendirme

OpenID Connect (OIDC) ile bir ASP.NET Core Blazor Web Uygulamasını güvenceye alın

Microsoft Entra ID ile bir ASP.NET Core Blazor Web Uygulamasını güvenceye alın

Microsoft Azure hizmetleri için, yönetilen kimliklerin kullanılmasını öneririz. Yönetilen kimlikler, kimlik bilgilerini uygulama kodunda depolamadan Azure hizmetlerine güvenli bir şekilde kimlik doğrular. Daha fazla bilgi için aşağıdaki kaynaklara bakın:

Azure kaynakları için yönetilen kimlikler nelerdir? (Microsoft Entra belgeleri)

Azure hizmetleri belgeleri

Azure SQL için Microsoft Entra’da yönetilen kimlikler

App Service ve Azure Functions için yönetilen kimlikler nasıl kullanılır

## 🗄️ Veritabanı bağlamı

Veritabanı bağlamı, BlazorWebAppMoviesContext, veritabanına bağlanır ve model nesnelerini veritabanı kayıtlarına eşler. Veritabanı bağlamı, bu serinin ikinci bölümünde oluşturuldu. İskelet kodu (scaffolded) Program dosyasında görünür:

```csharp
builder.Services.AddDbContextFactory<BlazorWebAppMoviesContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("BlazorWebAppMoviesContext") ?? 
        throw new InvalidOperationException(
            "Connection string 'BlazorWebAppMoviesContext' not found.")));
```

AddDbContextFactory, verilen bağlam için bir fabrika kaydeder ve bunu uygulamanın hizmet koleksiyonuna bir hizmet olarak ekler.

UseSqlServer veya UseSqlite, bağlamı bir Microsoft SQL Server ya da SQLite veritabanına bağlanacak şekilde yapılandırır. Ek veritabanı türlerine bağlanmak için başka sağlayıcılar da mevcuttur.

GetConnectionString, ASP.NET Core Yapılandırma sistemini kullanarak, sağlanan bağlantı dizesi adı için ConnectionStrings anahtarını okur; yukarıdaki örnekte bu ad BlazorWebAppMoviesContext’tir.

Yerel geliştirme için, yapılandırma veritabanı bağlantı dizesini uygulama ayarları dosyasından (appsettings.json) alır. Aşağıdaki örnekteki {CONNECTION STRING} yer tutucusu bağlantı dizesidir:

```json
"ConnectionStrings": {
  "BlazorWebAppMoviesContext": "{CONNECTION STRING}"
}
```

Aşağıda örnek bir bağlantı dizesi verilmiştir:

```
Server=(localdb)\mssqllocaldb;Database=BlazorWebAppMoviesContext-00001111-aaaa-2222-bbbb-3333cccc4444;Trusted_Connection=True;MultipleActiveResultSets=true
```

Uygulama bir test/ön hazırlık veya üretim sunucusuna dağıtıldığında, bağlantı dizesini proje yapılandırma dosyalarının dışında güvenli bir şekilde saklayın.

## ⚠️ Uyarı

İstemci tarafı kodunda uygulama sırlarını, bağlantı dizelerini, kimlik bilgilerini, parolaları, kişisel kimlik numaralarını (PIN), özel C#/.NET kodunu veya özel anahtarları/jetonları depolamayın; bu her zaman güvensizdir. Test/ön hazırlık ve üretim ortamlarında, sunucu tarafı Blazor kodu ve web API’leri, projede kimlik bilgilerini veya yapılandırma dosyalarını tutmaktan kaçınan güvenli kimlik doğrulama akışları kullanmalıdır. Yerel geliştirme testleri dışında, hassas verileri depolamak için ortam değişkenlerinin kullanımından kaçınmanızı öneririz; çünkü ortam değişkenleri en güvenli yaklaşım değildir. Yerel geliştirme testleri için, Secret Manager aracı hassas verilerin güvence altına alınması için önerilir. Daha fazla bilgi için bkz. Hassas verileri ve kimlik bilgilerini güvenli şekilde sürdürme.

## 🧱 Veritabanı teknolojisi

Bu eğitimin VS Code sürümü, genel, kendi kendine yeten, tam özellikli bir SQL veritabanı motoru olan SQLite’ı kullanır.

SQLite veritabanlarını yönetmek ve görüntülemek için kullanabileceğiniz birçok üçüncü taraf araç vardır. Aşağıdaki görsel, SQLite için DB Browser’ı göstermektedir:


![1762701713103](image/6_BuildaBlazormoviedatabaseapp(Part4-Workwithadatabase)/1762701713103.png)


# 🌱 Veritabanını Tohumlama (Seed Etme)

Bu eğitimde, **EF Core migrations** (göçler) kullanılır. Bir migration, veri modelindeki değişikliklerle eşleşmesi için veritabanı şemasını günceller. Ancak, migration’lar yalnızca EF Core sağlayıcısının desteklediği değişiklikleri yapabilir. Daha fazla okuma için kaynaklar bu makalenin sonunda listelenmiştir.

## 🌾 Veritabanını tohumlama (Seed the database)

Tohumlama kodu, geliştirme testi için bir dizi kayıt oluşturabilir veya yeni bir üretim veritabanı için başlangıç verilerini oluşturmak amacıyla kullanılabilir.

**Data** klasöründe, **SeedData** adında yeni bir sınıf oluşturun ve aşağıdaki kodu ekleyin:

📄 **Data/SeedData.cs:**

```csharp
using Microsoft.EntityFrameworkCore;
using BlazorWebAppMovies.Models;

namespace BlazorWebAppMovies.Data;

public class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new BlazorWebAppMoviesContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<BlazorWebAppMoviesContext>>());

        if (context == null || context.Movie == null)
        {
            throw new NullReferenceException(
                "Null BlazorWebAppMoviesContext or Movie DbSet");
        }

        if (context.Movie.Any())
        {
            return;
        }

        context.Movie.AddRange(
            new Movie
            {
                Title = "Mad Max",
                ReleaseDate = new DateOnly(1979, 4, 12),
                Genre = "Sci-fi (Cyberpunk)",
                Price = 2.51M,
            },
            new Movie
            {
                Title = "The Road Warrior",
                ReleaseDate = new DateOnly(1981, 12, 24),
                Genre = "Sci-fi (Cyberpunk)",
                Price = 2.78M,
            },
            new Movie
            {
                Title = "Mad Max: Beyond Thunderdome",
                ReleaseDate = new DateOnly(1985, 7, 10),
                Genre = "Sci-fi (Cyberpunk)",
                Price = 3.55M,
            },
            new Movie
            {
                Title = "Mad Max: Fury Road",
                ReleaseDate = new DateOnly(2015, 5, 15),
                Genre = "Sci-fi (Cyberpunk)",
                Price = 8.43M,
            },
            new Movie
            {
                Title = "Furiosa: A Mad Max Saga",
                ReleaseDate = new DateOnly(2024, 5, 24),
                Genre = "Sci-fi (Cyberpunk)",
                Price = 13.49M,
            });

        context.SaveChanges();
    }
}
```

Bu kod, bağımlılık enjeksiyonu ( **DI** ) kapsayıcısından bir **veritabanı bağlamı** örneği alır. Eğer veritabanında film kayıtları varsa, **return** çağrılır ve veritabanı tohumlanmaz. Veritabanı boşsa, **Mad Max** serisine ait filmler (©Warner Bros. Entertainment) veritabanına eklenir.

## 🧩 Seed başlatıcısını çalıştırmak

Seed başlatıcısını yürütmek için, **Program** dosyasında uygulama oluşturulduktan hemen sonraki satıra (yani `var app = builder.Build();` sonrasına) aşağıdaki kodu ekleyin.

`using` ifadesi, tohumlama işlemi tamamlandıktan sonra veritabanı bağlamının yok edilmesini (dispose edilmesini) sağlar.

```csharp
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}
```

## 🧹 Eski test verilerini temizleme

Eğer veritabanında önceki testlerden kalan kayıtlar varsa:

1. Uygulamayı çalıştırın.
2. Veritabanındaki oluşturduğunuz varlıkları silin.
3. Uygulamayı durdurmak için tarayıcı penceresini kapatın ve komut isteminde **Ctrl+C** (Windows) tuşlarına basın.

## ▶️ Uygulamayı çalıştırma

Veritabanı boş olduğunda uygulamayı yeniden çalıştırın.

**Movies Index** sayfasına giderek tohumlanmış (seed edilmiş) filmleri görüntüleyin.



![1762701766305](image/6_BuildaBlazormoviedatabaseapp(Part4-Workwithadatabase)/1762701766305.png)


# 🧩 Bir formu bir modele bağlama

**Edit bileşenini** inceleyin ( **Components/Pages/MoviePages/Edit.razor** ).

Bir **HTTP GET** isteği, Edit bileşen sayfasına yapıldığında (örneğin: `/movies/edit?id=6` adresine):

* `OnInitializedAsync` yöntemi, **Id’si 6 olan** filmi veritabanından getirir ve **Movie** özelliğine atar.
* `EditForm.Model` parametresi, form için en üst düzey model nesnesini belirtir. Atanan model kullanılarak form için bir düzenleme bağlamı ( **edit context** ) oluşturulur.
* Form, filmden alınan değerlerle görüntülenir.

Edit sayfası sunucuya gönderildiğinde ( **post edildiğinde** ), formdaki değerler **[SupplyParameterFromForm]** özniteliği sayesinde **Movie** özelliğine bağlanır:

```csharp
[SupplyParameterFromForm]
private Movie? Movie { get; set; }
```

Eğer model durumu ( **model state** ) hatalar içeriyorsa, örneğin **ReleaseDate** bir tarihe dönüştürülemezse, form gönderilen değerlerle yeniden görüntülenir.

Model hatası yoksa, film formdan gönderilen değerlerle kaydedilir.

---

## ⚙️ Eşzamanlılık (Concurrency) istisnası yönetimi

**Edit** bileşeninin **UpdateMovie** yöntemini inceleyin

( **Components/Pages/MoviePages/Edit.razor** ):

```csharp
private async Task UpdateMovie()
{
    using var context = DbFactory.CreateDbContext();
    context.Attach(Movie!).State = EntityState.Modified;

    try
    {
        await context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!MovieExists(Movie!.Id))
        {
            NavigationManager.NavigateTo("notfound");
        }
        else
        {
            throw;
        }
    }

    NavigationManager.NavigateTo("/movies");
}
```

Eşzamanlılık istisnaları, bir istemci filmi silerken başka bir istemcinin aynı film üzerinde değişiklik göndermesi durumunda tespit edilir.

---

## 🧪 Eşzamanlılık işlemini test etme

1. Bir film için **Edit** seçeneğini tıklayın, değişiklik yapın ama  **Save** ’e basmayın.
2. Farklı bir tarayıcı penceresinde uygulamayı açın ve **Index** sayfasında aynı filmi **Delete** bağlantısıyla silin.
3. İlk pencerede **Save** tuşuna basarak değişiklikleri gönderin.

Tarayıcı, mevcut olmayan **notfound** uç noktasına yönlendirilir ve **404 (Not Found)** sonucu döner.

Blazor uygulamalarında EF Core ile eşzamanlılık yönetimi hakkında ek bilgiler Blazor belgelerinde mevcuttur.

---

## 🛑 Uygulamayı durdurma

Uygulama çalışıyorsa, tarayıcı penceresini kapatarak ve komut satırında **Ctrl+C** tuşlarına basarak uygulamayı kapatın.

---

## 🧭 Tamamlanmış örnekle sorun giderme

Eğitim sırasında çözemediğiniz bir sorunla karşılaşırsanız, kodunuzu **Blazor örnek deposundaki (samples repository)** tamamlanmış proje ile karşılaştırın:

🔗 **Blazor samples GitHub repository (dotnet/blazor-samples)**

En son sürüm klasörünü seçin.

Bu eğitimin proje örneği klasörü **BlazorWebAppMovies** olarak adlandırılmıştır.

---

## 📚 Ek kaynaklar

### ⚙️ Yapılandırma makaleleri:

* Configuration in ASP.NET Core (ASP.NET Core yapılandırma sistemi)
* ASP.NET Core Blazor configuration (Blazor belgeleri)
* Data seeding (EF Core belgeleri)
* Concurrency with EF Core in Blazor apps

### 🗄️ Veritabanı sağlayıcısı kaynakları:

* EF Core documentation
* SQLite EF Core Database Provider Limitations
* Customize migration code
* SQLite ALTER TABLE statement (SQLite belgeleri)

### 🔐 Blazor Web App güvenliği:

* ASP.NET Core Blazor authentication and authorization
* ASP.NET Core Blazor authentication and authorization ve Sunucu güvenliği bölümü
* Secure an ASP.NET Core Blazor Web App with OpenID Connect (OIDC)
* Secure an ASP.NET Core Blazor Web App with Microsoft Entra ID

---

## ⚖️ Yasal

 **Mad Max** ,  **The Road Warrior** ,  **Mad Max: Beyond Thunderdome** , **Mad Max: Fury Road** ve  **Furiosa: A Mad Max Saga** ,

 *Warner Bros. Entertainment* ’ın ticari markaları ve telif haklarıdır.
