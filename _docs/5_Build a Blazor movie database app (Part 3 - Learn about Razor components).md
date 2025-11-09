
# 🎬 Blazor Film Veritabanı Uygulaması (Bölüm 3 - Razor Bileşenleri Hakkında Bilgi Edinme)

## 🔧 Araç Seçimi

Bu makale, bir film veritabanını yönetme özelliklerine sahip bir ASP.NET Core Blazor Web Uygulaması oluşturmayı öğreten Blazor film veritabanı uygulaması eğitim serisinin üçüncü bölümüdür.

Bu bölüm, uygulamaya eklenen Razor bileşenlerini inceler ve film verilerinin görüntülenmesi için iyileştirmeler yapar.

---

## 🧩 Razor Bileşenleri

Blazor uygulamaları, genellikle yalnızca bileşenler olarak adlandırılan Razor bileşenlerine dayanır.

Bir bileşen, bir sayfa, iletişim kutusu veya veri giriş formu gibi bir kullanıcı arayüzü öğesidir.

Bileşenler, .NET derlemelerine yerleştirilmiş .NET C# sınıflarıdır.

 **Razor** , genellikle istemci tarafı UI mantığı ve bileşimi için Razor biçimlendirme sayfası (.razor dosya uzantısı) olarak yazılan bileşenleri ifade eder. Razor, HTML biçimlendirmesini geliştirici verimliliği için tasarlanmış C# koduyla birleştiren bir sözdizimidir.

Geliştiriciler ve çevrimiçi kaynaklar genellikle “Blazor bileşenleri” terimini kullanırken, belgelerde resmi ad olarak “Razor bileşenleri” (veya sadece “bileşenler”) kullanılır.

Bir Razor bileşeninin yapısı genellikle şu genel kalıbı izler:

* Bileşen tanımının (.razor dosyası) en üstünde, çeşitli Razor yönergeleri bileşen biçimlendirmenin nasıl derleneceğini veya çalışacağını belirtir.
* Ardından, HTML’in nasıl oluşturulacağını belirten Razor biçimlendirmesi gelir.
* Son olarak, bir `@code` bloğu, bileşen sınıfının üyelerini, bileşen parametrelerini ve olay işleyicilerini tanımlayan C# kodunu içerir.

### 🎉 Örnek: Welcome Bileşeni (Welcome.razor)

```razor
@page "/welcome"

<PageTitle>Welcome!</PageTitle>

<h1>Welcome to Blazor!</h1>

<p>@welcomeMessage</p>

@code {
    private string welcomeMessage = "We ❤️ Blazor!";
}
```

İlk satır, Razor bileşenlerinde önemli bir yapı olan bir **Razor yönergesini** temsil eder.

Bir Razor yönergesi, `@` önekiyle başlayan ve bileşen biçimlendirmesinin derlenme veya çalışma şeklini değiştiren bir ayrılmış anahtar kelimedir.

`@page` yönergesi, bileşenin rota şablonunu belirtir. Bu bileşene tarayıcıda `/welcome` göreli URL’siyle ulaşılır.

**PageTitle** bileşeni, sayfa başlığını belirleyen çerçeveye dahil bir bileşendir.

`<h1>` etiketi içinde “Welcome to Blazor!” ifadesi, bileşenin oluşturulmuş ilk gövde biçimlendirmesidir.

Sonrasında, `@welcomeMessage` değişkeni kullanılarak Razor sözdizimiyle bir karşılama mesajı görüntülenir.

`@code` bloğu, bileşenin C# kodunu içerir.

`welcomeMessage`, bir değerle başlatılmış özel (`private`) bir dizedir.

---

## 🧭 NavMenu Bileşeni (Gezinme Menüsü)

**NavMenu** bileşeni (`Components/Layout/NavMenu.razor`), diğer Razor bileşenlerine yönlendiren **NavLink** bileşenlerini kullanarak kenar çubuğu gezinmesini uygular.

Bir **NavLink** bileşeni, bir `<a>` etiketi gibi davranır, ancak `href` özelliği geçerli URL ile eşleştiğinde **aktif bir CSS sınıfını** değiştirir.

Bu aktif sınıf, kullanıcının hangi sayfanın aktif olduğunu anlamasına yardımcı olur.

`NavLinkMatch.All`, `Match` parametresine atandığında, bağlantının tam URL ile eşleştiğinde aktif CSS sınıfını görüntülemesini sağlar.

`NavLink` bileşeni Blazor çerçevesinde yerleşik olarak bulunur, ancak `NavMenu` yalnızca Blazor proje şablonlarının bir parçasıdır.

### 📄 `Components/Layout/NavMenu.razor`

```razor
<div class="top-row ps-3 navbar navbar-dark">
    <div class="container-fluid">
        <a class="navbar-brand" href="">BlazorWebAppMovies</a>
    </div>
</div>

<input type="checkbox" title="Navigation menu" class="navbar-toggler" />

<div class="nav-scrollable" onclick="document.querySelector('.navbar-toggler').click()">
    <nav class="flex-column">
        <div class="nav-item px-3">
            <NavLink class="nav-link" href="" Match="NavLinkMatch.All">
                <span class="bi bi-house-door-fill-nav-menu" aria-hidden="true"></span> Home
            </NavLink>
        </div>

        <div class="nav-item px-3">
            <NavLink class="nav-link" href="weather">
                <span class="bi bi-list-nested-nav-menu" aria-hidden="true"></span> Weather
            </NavLink>
        </div>
    </nav>
</div>
```

---

### 🪄 Marka Adını Değiştirme

İlk `<div>` etiketinde yer alan marka bağlantı metnini değiştirin:

`BlazorWebAppMovies` yerine **Sci-fi Movies** yazın.

```diff
- <a class="navbar-brand" href="">BlazorWebAppMovies</a>
+ <a class="navbar-brand" href="">Sci-fi Movies</a>
```

---

### 🎞️ Movies Sayfası Bağlantısı Ekleme

Kullanıcıların **Movies** dizin sayfasına ulaşabilmesi için, Weather bileşeninin NavLink’inden hemen sonra aşağıdaki kodu ekleyin:

```razor
<div class="nav-item px-3">
    <NavLink class="nav-link" href="movies">
        <span class="bi bi-list-nested-nav-menu" aria-hidden="true"></span> Movies
    </NavLink>
</div>
```

---

### ✅ Nihai NavMenu Bileşeni

```razor
<div class="top-row ps-3 navbar navbar-dark">
    <div class="container-fluid">
        <a class="navbar-brand" href="">Sci-fi Movies</a>
    </div>
</div>

<input type="checkbox" title="Navigation menu" class="navbar-toggler" />

<div class="nav-scrollable" onclick="document.querySelector('.navbar-toggler').click()">
    <nav class="nav flex-column">
        <div class="nav-item px-3">
            <NavLink class="nav-link" href="" Match="NavLinkMatch.All">
                <span class="bi bi-house-door-fill-nav-menu" aria-hidden="true"></span> Home
            </NavLink>
        </div>

        <div class="nav-item px-3">
            <NavLink class="nav-link" href="weather">
                <span class="bi bi-list-nested-nav-menu" aria-hidden="true"></span> Weather
            </NavLink>
        </div>

        <div class="nav-item px-3">
            <NavLink class="nav-link" href="movies">
                <span class="bi bi-list-nested-nav-menu" aria-hidden="true"></span> Movies
            </NavLink>
        </div>
    </nav>
</div>
```

---

## 🚀 Uygulamayı Çalıştırma

Uygulamayı çalıştırın ve kenar çubuğunun üst kısmındaki **Sci-fi Movies** markasını ve **Movies** sayfasına yönlendiren yeni bağlantıyı görün.


![1762701354592](image/5_BuildaBlazormoviedatabaseapp(Part3-LearnaboutRazorcomponents)/1762701354592.png)


# 🎬 Blazor Film Veritabanı Uygulaması (Bölüm 3 - Razor Bileşenleri Hakkında Bilgi Edinme)

## 🧱 MainLayout Bileşeni (Yerleşim)

**MainLayout** bileşeni, uygulamanın varsayılan yerleşimidir.

MainLayout bileşeni, bir yerleşimi temsil eden bileşenler için temel sınıf olan **LayoutComponentBase** sınıfından türetilmiştir.

Yerleşimi kullanan uygulama bileşenleri, biçimlendirmede **@Body** ifadesinin bulunduğu yerde işlenir.

### 📄 `Components/Layout/MainLayout.razor`

```razor
@inherits LayoutComponentBase

<div class="page">
    <div class="sidebar">
        <NavMenu />
    </div>

    <main>
        <div class="top-row px-4">
            <a href="https://learn.microsoft.com/aspnet/core/" target="_blank">About</a>
        </div>

        <article class="content px-4">
            @Body
        </article>
    </main>
</div>
```

### 🧩 Özellikler

* **NavMenu** bileşeni kenar çubuğunda işlenir.

  Razor biçimlendirmesinde bir bileşeni görüntülemek için yalnızca bileşen adını HTML etiketi olarak yazmak yeterlidir.

  Bu, bileşenleri birbirine ve HTML düzenlerine iç içe yerleştirmenizi sağlar.
* `<main>` öğesinin içeriği şunları içerir:

  * ASP.NET Core belgelerine yönlendiren bir **About** bağlantısı.
  * **@Body** parametresine sahip bir `<article>` öğesi, burada yerleşimi kullanan bileşenler işlenir.
  * **Hata arayüzü** (`<div id="blazor-error-ui" ...>`), işlenmemiş hatalar için bildirim gösterir.

Varsayılan yerleşim (`MainLayout` bileşeni), **Routes** bileşeninde belirtilmiştir:

```razor
<RouteView RouteData="routeData" DefaultLayout="typeof(Layout.MainLayout)" />
```

Bireysel bileşenler kendi yerleşimlerini belirleyebilir ve bir klasördeki tüm bileşenlere `_Imports.razor` dosyası aracılığıyla uygulanabilir.

Bu özellikler Blazor belgelerinde ayrıntılı olarak açıklanmıştır.

---

## 🔄 CRUD (Oluştur, Oku, Güncelle, Sil) Bileşenleri

### 📋 Index Bileşeni

`Components/Pages/Movies/Index.razor` dosyasını açın.

Dosyanın en üstündeki Razor yönergeleri şunları belirtir:

* `@page` yönergesi `/movies` URL’sini tanımlar.
* `@using` yönergeleri şu API’lere erişim sağlar:
  * `Microsoft.EntityFrameworkCore`
  * `Microsoft.AspNetCore.Components.QuickGrid`
  * `BlazorWebAppMovies.Models`
  * `BlazorWebAppMovies.Data`

Veritabanı bağlamı fabrikası `@inject` yönergesiyle bileşene eklenir:

`IDbContextFactory<BlazorWebAppMoviesContext>`

Bu yaklaşım, bağlamın imha edilmesini gerektirdiği için bileşen `IAsyncDisposable` arayüzünü uygular.

Sayfa başlığı **PageTitle** bileşeniyle ayarlanır, ardından `<h1>` etiketi gelir:

```razor
<PageTitle>Index</PageTitle>
<h1>Index</h1>
```

Yeni film ekleme bağlantısı:

```razor
<p>
    <a href="movies/create">Create New</a>
</p>
```

Film varlıklarını görüntülemek için **QuickGrid** bileşeni kullanılır:

```razor
<QuickGrid Class="table" Items="context.Movie">
    <PropertyColumn Property="movie => movie.Title" />
    <PropertyColumn Property="movie => movie.ReleaseDate" />
    <PropertyColumn Property="movie => movie.Genre" />
    <PropertyColumn Property="movie => movie.Price" />

    <TemplateColumn Context="movie">
        <a href="@($"movies/edit?id={movie.Id}")">Edit</a> |
        <a href="@($"movies/details?id={movie.Id}")">Details</a> |
        <a href="@($"movies/delete?id={movie.Id}")">Delete</a>
    </TemplateColumn>
</QuickGrid>

@code {
    private BlazorWebAppMoviesContext context = default!;

    protected override void OnInitialized()
    {
        context = DbFactory.CreateDbContext();
    }

    public async ValueTask DisposeAsync() => await context.DisposeAsync();
}
```

### 🧠 Açıklama

* `context`: Veritabanı bağlamını tutar.
* `OnInitialized()`: Bağlam örneğini oluşturur.
* `DisposeAsync()`: Bileşen imha edildiğinde bağlamı serbest bırakır.
* `Context="movie"` ifadesi, satır içindeki öğeler için okunabilirliği artırır.
* Razor ifadeleri `@($"movies/edit?id={movie.Id}")` bağlantılarda film kimliğini ekler.

### 🧩 Sütun Başlığını Güncelle

```diff
- <PropertyColumn Property="movie => movie.ReleaseDate" />
+ <PropertyColumn Property="movie => movie.ReleaseDate" Title="Release Date" />
```

---

## 🎞️ Details Bileşeni

`Components/Pages/Movies/Details.razor` dosyasını açın.

```razor
@page "/movies/details"
@using Microsoft.EntityFrameworkCore
@using BlazorWebAppMovies.Models
@inject IDbContextFactory<BlazorWebAppMovies.Data.BlazorWebAppMoviesContext> DbFactory
@inject NavigationManager NavigationManager
```

Film yüklenmemişse:

```razor
@if (movie is null)
{
    <p><em>Loading...</em></p>
}
```

Film yüklendiğinde:

```razor
<dl>
    <dt>Title</dt>
    <dd>@movie.Title</dd>
    <dt>Release Date</dt>
    <dd>@movie.ReleaseDate</dd>
    <dt>Genre</dt>
    <dd>@movie.Genre</dd>
    <dt>Price</dt>
    <dd>@movie.Price</dd>
</dl>
<div>
    <a href="@($"/movies/edit?id={movie.Id}")">Edit</a> |
    <a href="@($"/movies")">Back to List</a>
</div>
```

### 💻 Kod Bloğu

```csharp
private Movie? movie;

[SupplyParameterFromQuery]
private int Id { get; set; }

protected override async Task OnInitializedAsync()
{
    using var context = DbFactory.CreateDbContext();
    movie = await context.Movie.FirstOrDefaultAsync(m => m.Id == Id);

    if (movie is null)
    {
        NavigationManager.NavigateTo("notfound");
    }
}
```

---

## 🆕 Create Bileşeni

`Components/Pages/Movies/Create.razor` dosyasını açın.

Form bileşeni:

```razor
<EditForm method="post" Model="Movie" OnValidSubmit="AddMovie" FormName="create" Enhance>
    <DataAnnotationsValidator />
    <ValidationSummary role="alert" />
    <div>
        <label for="title">Title:</label>
        <InputText id="title" @bind-Value="Movie.Title" />
        <ValidationMessage For="() => Movie.Title" />
    </div>
    <div>
        <label for="releasedate">Release Date:</label>
        <InputDate id="releasedate" @bind-Value="Movie.ReleaseDate" />
        <ValidationMessage For="() => Movie.ReleaseDate" />
    </div>
    <div>
        <label for="genre">Genre:</label>
        <InputText id="genre" @bind-Value="Movie.Genre" />
        <ValidationMessage For="() => Movie.Genre" />
    </div>
    <div>
        <label for="price">Price:</label>
        <InputNumber id="price" @bind-Value="Movie.Price" />
        <ValidationMessage For="() => Movie.Price" />
    </div>
    <button type="submit">Create</button>
</EditForm>
```

### 💻 Kod Bloğu

```csharp
@code {
    [SupplyParameterFromForm]
    private Movie Movie { get; set; } = new();

    private async Task AddMovie()
    {
        using var context = DbFactory.CreateDbContext();
        context.Movie.Add(Movie);
        await context.SaveChangesAsync();
        NavigationManager.NavigateTo("/movies");
    }
}
```

---

## 🗑️ Delete Bileşeni

`Components/Pages/Movies/Delete.razor` dosyasını açın.

```diff
- <dt class="col-sm-2">ReleaseDate</dt>
+ <dt class="col-sm-2">Release Date</dt>
```

Buton:

```razor
<button type="submit" disabled="@(movie is null)">Delete</button>
```

C# kodu:

```csharp
private async Task DeleteMovie()
{
    using var context = DbFactory.CreateDbContext();
    context.Movie.Remove(movie!);
    await context.SaveChangesAsync();
    NavigationManager.NavigateTo("/movies");
}
```

---

## ✏️ Edit Bileşeni

```diff
- <label for="releasedate" class="form-label">ReleaseDate:</label>
+ <label for="releasedate" class="form-label">Release Date:</label>
```

Gizli kimlik alanı:

```razor
<input type="hidden" name="Movie.Id" value="@Movie.Id" />
```

C# kodu:

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

private bool MovieExists(int id)
{
    using var context = DbFactory.CreateDbContext();
    return context.Movie.Any(e => e.Id == id);
}
```

---

## 🛡️ Aşırı Gönderim (Overposting) Saldırılarını Önleme

Statik olarak oluşturulmuş sunucu tarafı formlar, kötü niyetli kullanıcılar tarafından **overposting** saldırısına uğrayabilir.

Bu saldırı, geliştiricinin izin vermediği ek özelliklerle form gönderimi yapıldığında gerçekleşir.

Bu öğreticideki **Movie** modeli için böyle bir risk yoktur.

Ancak gelecekte oluşturacağınız formlarda dikkat edilmelidir.

💡 **Öneri:**

Form işlemleri için ayrı bir **ViewModel/DTO** kullanın.

Bu şekilde yalnızca izin verilen alanlar işlenir, kötü niyetli gönderiler reddedilir.

---

## 🧰 Sorun Giderme

Bir sorunla karşılaşırsanız, tamamlanmış örnek projeyi inceleyin:

🔗 **[Blazor Samples GitHub Repository (dotnet/blazor-samples)](https://github.com/dotnet/blazor-samples)**

📂 Proje klasörü: **BlazorWebAppMovies**

---

## 📚 Ek Kaynaklar

* NavLink Component
* ASP.NET Core Blazor Layouts
* Razor Directives
* QuickGrid Component
* Blazor Forms Overview
* EF Core Concurrency
* Blazor Globalization & Localization
