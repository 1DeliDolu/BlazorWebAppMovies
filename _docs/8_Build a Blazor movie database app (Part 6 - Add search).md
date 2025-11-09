# 🎬 Blazor film veritabanı uygulaması (Bölüm 6 - Arama ekleme)

## 🧰 Araç Seçimi

Bu makale, bir film veritabanını yönetme özelliklerine sahip bir ASP.NET Core Blazor Web Uygulaması oluşturmanın temellerini öğreten Blazor film veritabanı uygulaması öğreticisinin altıncı bölümüdür.

Bu bölüm, filmleri başlığa göre filtrelemek için **arama özelliğinin** Movies Index bileşenine eklenmesini kapsar.

---

## 🔍 QuickGrid bileşeni için filtre özelliği ekleme

QuickGrid bileşeni, veritabanından filmleri görüntülemek için **Movies Index** bileşeni (Components/MoviePages/Index.razor) tarafından kullanılır:

```razor
<QuickGrid Class="table" Items="context.Movie">
    ...
</QuickGrid>
```

**Items** parametresi, her satırda temsil edilen veri türünü (Movie) ifade eden `IQueryable<TGridItem>` alır.

Items, oluşturulan veritabanı bağlamından (`CreateDbContext`) elde edilen film varlıkları koleksiyonuna (`DbSet<Movie>`) atanır. Bu bağlam, eklenen veritabanı bağlamı fabrikasından (`DbFactory`) alınır.

---

## 🧩 QuickGrid’in film başlığına göre filtrelenmesi için

Index bileşeni aşağıdaki adımları gerçekleştirmelidir:

1. **Sorgu dizesinden (query string)** bir filtre dizesi parametresi ayarla.
2. Eğer bu parametre bir değere sahipse, döndürülen filmleri bu değere göre filtrele.
3. Kullanıcının filtre girmesi ve bu filtreyi kullanarak sayfayı yeniden yüklemesi için bir **giriş alanı ve düğme** sağla.

---

## 💻 Kod ekleme

Aşağıdaki kodu **Index bileşeninin @code bloğuna** ekle (MoviePages/Index.razor):

```csharp
[SupplyParameterFromQuery]
private string? TitleFilter { get; set; }

private IQueryable<Movie> FilteredMovies => 
    context.Movie.Where(m => m.Title!.Contains(TitleFilter ?? string.Empty));
```

* **TitleFilter** , filtre dizesidir.
* `[SupplyParameterFromQuery]` niteliği, Blazor’a **TitleFilter** değerinin sorgu dizesinden alınacağını belirtir.

  Örneğin, `?titleFilter=road+warrior` sorgu dizesi, `TitleFilter` değişkenine **road warrior** değerini atar.

  (Sorgu dizesi alan adları  **büyük/küçük harfe duyarlı değildir** .)
* **FilteredMovies** özelliği `IQueryable<Movie>` türündedir ve QuickGrid’in **Items** parametresine atanır.

  Bu özellik, verilen  **TitleFilter** ’a göre film listesini filtreler.

  Eğer **TitleFilter** değeri null ise, `string.Empty` kullanılır ve hiçbir film filtrelenmez (tüm filmler görüntülenir).

---

## 🧾 QuickGrid güncellemesi

QuickGrid bileşenindeki Items parametresini aşağıdaki şekilde değiştir:

```diff
- <QuickGrid Class="table" Items="context.Movie">
+ <QuickGrid Class="table" Items="FilteredMovies">
```

---

## 🧠 Lambda ifadeleri ve LINQ açıklaması

`movie => movie.Title!.Contains(...)` kodu bir  **lambda ifadesidir** .

Lambdalar,  **Where** , **Contains** veya **OrderBy** gibi standart sorgu operatörlerine argüman olarak verilen **LINQ sorgularında** kullanılır.

LINQ sorguları tanımlandığında veya  **Where** , **Contains** gibi metodlar çağrıldığında hemen çalıştırılmaz.

Sorgunun  **çalıştırılması ertelenir** ; sorgu ancak gerçek değerine erişildiğinde (örneğin, döngüyle üzerinden geçildiğinde) yürütülür.

---

## ⚙️ Veritabanı davranışı

* **Where** metodu C# kodunda değil, **veritabanı üzerinde** çalışır.
* Sorgunun  **büyük/küçük harf duyarlılığı** , kullanılan veritabanına ve **collation** ayarına bağlıdır.
  * SQL Server’da  **Contains** , SQL **LIKE** ifadesine karşılık gelir ve  **büyük/küçük harf duyarsızdır** .
  * SQLite varsayılan ayarlarla bazen duyarlı, bazen duyarsız şekilde davranır.

---

## ▶️ Uygulamayı çalıştır

Uygulamayı çalıştır ve `/movies` adresine git.

Veritabanındaki filmler yüklenecektir.


![1762702148662](image/8_BuildaBlazormoviedatabaseapp(Part6-Addsearch)/1762702148662.png)


# 🌐 URL'ye sorgu dizesi ekleme

Adres çubuğundaki URL'ye şu sorgu dizesini ekle:

```
?titleFilter=Road+Warrior
```

Örneğin, tam URL aşağıdaki gibi görünür (bağlantı noktası numarasının 7073 olduğunu varsayarsak):

```
https://localhost:7073/movies?titleFilter=Road+Warrior
```

Bu şekilde, yalnızca **“Road Warrior”** başlığıyla eşleşen film filtrelenmiş olarak görüntülenir.



![1762702163490](image/8_BuildaBlazormoviedatabaseapp(Part6-Addsearch)/1762702163490.png)


# 🔎 Kullanıcıların arama yapabilmesi için arayüz ekleme

Şimdi, kullanıcıların **titleFilter** filtre dizesini bileşenin arayüzü üzerinden girebilmesini sağlayalım.

Bunun için **`<h1>Index</h1>`** başlığının altına aşağıdaki **HTML kodunu** ekle:

```html
<div>
    <form action="/movies" data-enhance>
        <input type="search" name="titleFilter" />
        <input type="submit" value="Search" />
    </form>
</div>
```

---

## ⚙️ Açıklama

* **`data-enhance`** niteliği, bileşene gelişmiş gezinme davranışı uygular.

  Blazor, **GET** isteğini yakalayarak tam sayfa yenileme yerine **fetch isteği** gönderir.

  Ardından, yanıt içeriğini sayfaya “patch” eder.

Bu sayede:

* Sayfa  **tamamen yeniden yüklenmez** ,
* **Kullanıcı durumu** (örneğin kaydırma konumu) korunur,
* Sayfa **daha hızlı** yüklenir.

---

## 🚀 Uygulamayı test et

Uygulama hâlihazırda **`dotnet watch`** ile çalıştığı için yapılan değişiklikler otomatik olarak algılanır ve tarayıcı penceresine yansıtılır.

Artık arama kutusuna **“Road Warrior”** yazıp **Search** düğmesine tıklayarak filmleri filtreleyebilirsin.


![1762702195522](image/8_BuildaBlazormoviedatabaseapp(Part6-Addsearch)/1762702195522.png)

🔍 Road Warrior aramasından sonraki sonuç:

![1762702203148](image/8_BuildaBlazormoviedatabaseapp(Part6-Addsearch)/1762702203148.png)



# 💾 Arama değerini koruma

Filmler filtrelendiğinde, arama kutusunun içeriği ( **"Road Warrior"** ) kaybolur.

Aranan değerin korunmasını istiyorsan, forma **data-permanent** niteliğini ekle:

```diff
- <form action="/movies" data-enhance>
+ <form action="/movies" data-enhance data-permanent>
```

---

## 🛑 Uygulamayı durdurma

Uygulamayı durdurmak için tarayıcı penceresini kapat ve komut satırında **Ctrl+C** tuşlarına bas.

---

## 🧩 Sorun giderme

Eğer öğreticide ilerlerken çözemediğin bir sorunla karşılaşırsan, kodunu tamamlanmış örnek proje ile karşılaştır:

🔗 **Blazor örnekleri GitHub deposu (dotnet/blazor-samples)**

En son sürüm klasörünü seç.

Bu öğreticiye ait örnek projenin klasör adı: **BlazorWebAppMovies**

---

## 📚 Ek kaynaklar

* [LINQ belgeleri](https://learn.microsoft.com/dotnet/csharp/programming-guide/concepts/linq/)
* [Veri sorgulamak için C# LINQ sorguları yazma (C# belgeleri)](https://learn.microsoft.com/dotnet/csharp/programming-guide/concepts/linq/write-linq-queries)
* [Lambda ifadeleri (C# belgeleri)](https://learn.microsoft.com/dotnet/csharp/lambda-expressions)
