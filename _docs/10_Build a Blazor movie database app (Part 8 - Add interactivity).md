# 🎬 Blazor Film Veritabanı Uygulaması (Bölüm 8 - Etkileşim Ekleme)

## ⚙️ Araç Seçimi

Bu makale, film veritabanı yönetim özelliklerine sahip bir ASP.NET Core Blazor Web App oluşturmanın temellerini öğreten Blazor film veritabanı uygulaması öğreticisinin sekizinci bölümüdür.

Bu noktaya kadar uygulamanın tamamı etkileşim için etkinleştirilmişti, ancak yalnızca **Counter** örnek bileşeninde etkileşim benimsenmişti. Bu bölüm, film **Index** bileşeninde etkileşimi nasıl benimseyeceğinizi açıklar.

> **Önemli:**
>
> Sonraki adımlara geçmeden önce uygulamanın çalışmadığından emin olun.

---

## ⚡ Etkileşimi Benimseme

Etkileşim, bir bileşenin C# kodu aracılığıyla **UI olaylarını** (örneğin, bir düğmeye tıklama) işleme yeteneğine sahip olması anlamına gelir. Olaylar, ASP.NET Core çalışma zamanı tarafından **sunucuda** veya Blazor WebAssembly tabanlı çalışma zamanı tarafından **tarayıcıda** işlenebilir.

Bu öğreticide  **etkileşimli sunucu tarafı işleme (interactive SSR)** , yani **Interactive Server (InteractiveServer)** işleme benimsenir.

İstemci tarafı işleme (CSR) doğal olarak etkileşimlidir ve Blazor referans belgelerinde ele alınmıştır.

Etkileşimli SSR, bir istemci uygulamasından beklenen zengin kullanıcı deneyimini sağlar ancak sunucu kaynaklarına erişmek için API uç noktaları oluşturmanıza gerek kalmaz.  **UI etkileşimleri** , tarayıcı ile sunucu arasında **gerçek zamanlı SignalR bağlantısı** üzerinden işlenir.

Sayfa içeriği, sunucuda önceden oluşturulur (prerendering), yani sayfa istemciye gönderilmeden önce HTML çıktı üretilir, ancak etkileşim henüz etkin değildir. Bu sayede uygulama kullanıcıya daha hızlı yanıt verir.

---

## 🧩 Program.cs Dosyasında Etkileşimli SSR API’sini İnceleyin

Razor bileşen hizmetleri, **statik olarak sunucudan render edilmesini** (AddRazorComponents) ve **etkileşimli SSR olarak çalıştırılmasını** (AddInteractiveServerComponents) etkinleştirir:

```csharp
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
```

 **MapRazorComponents** , kök App bileşeninde tanımlanan bileşenleri eşler ve yönlendirilebilir bileşenleri render eder.

 **AddInteractiveServerRenderMode** , uygulamanın SignalR hub’ını etkileşimli SSR desteğiyle yapılandırır:

```csharp
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
```

Önceki bölümlerde bu çağrılar gerekmedi çünkü film sayfaları yalnızca **statik SSR** özelliklerini kullanıyordu.

Bu makalede, film **Index** bileşenine etkileşimli özellikler eklenecektir.

---

## 🧭 Render Modları

Blazor bir bileşen için render türünü belirlediğinde buna **render mode (işleme modu)** denir.

| Ad                                | Açıklama                                          | Render Konumu      | Etkileşim |
| --------------------------------- | --------------------------------------------------- | ------------------ | ---------- |
| **Static Server**           | Statik sunucu tarafı işleme (SSR)                 | Sunucu             | ❌ Hayır  |
| **Interactive Server**      | Etkileşimli sunucu tarafı işleme (Blazor Server) | Sunucu             | ✔️ Evet  |
| **Interactive WebAssembly** | İstemci tarafı işleme (CSR - Blazor WebAssembly) | İstemci           | ✔️ Evet  |
| **Interactive Auto**        | Başta SSR, sonrasında CSR                         | Sunucu → İstemci | ✔️ Evet  |

---

## 🧱 Bileşen Düzeyinde Render Modu Uygulama

Bir bileşene render modu uygulamak için `@rendermode` yönergesi veya yönerge özniteliği kullanılır.

### 🗨️ Örnek 1 — Bileşen Örneğinde

`Components/Pages/Chat.razor` dosyasında:

```razor
<Dialog @rendermode="InteractiveServer" />
```

### 📈 Örnek 2 — Bileşen Tanımında

`Components/Pages/SalesForecast.razor` dosyasının başında:

```razor
@page "/sales-forecast"
@rendermode InteractiveServer
```

---

## 🌐 Global Etkileşim

Bir uygulamanın tamamı için tek bir render modu belirlemek mümkündür.

Bu, kök bileşen (genellikle `App` bileşeni) üzerinden **global interactivity** olarak adlandırılır.

Çoğu bileşen etkileşimliyse global yaklaşım uygundur.

Bu öğretici yalnızca **bileşen düzeyinde** etkileşimi ele alır.

Ancak sonrasında, global modları denemekte özgürsünüz.

---

## 🎞️ Film Index Bileşenine Etkileşim Ekleme

`Components/Pages/MoviePages/Index.razor` dosyasını açın ve `@page` yönergesinden hemen sonra aşağıdaki satırı ekleyin:

```razor
@rendermode InteractiveServer
```

---

## 🧮 QuickGrid’e Sayfalama (Pagination) Ekleme

QuickGrid bileşeni veritabanından verileri sayfalandırabilir.

1. **Index bileşenini açın** (`Components/Pages/Movies/Index.razor`).
2. `@code` bloğuna bir `PaginationState` örneği ekleyin.

```csharp
private PaginationState pagination = new PaginationState { ItemsPerPage = 2 };
```

> Bu örnekte yalnızca 5 film bulunduğundan, sayfalandırmayı göstermek için **2 öğe** seçilmiştir.

3. **QuickGrid** bileşenini güncelleyin:

```diff
- <QuickGrid Class="table" Items="FilteredMovies">
+ <QuickGrid Class="table" Items="FilteredMovies" Pagination="pagination">
```

4. **Paginator** bileşenini, QuickGrid’in altına ekleyin:

```razor
<Paginator State="pagination" />
```

---

## ▶️ Uygulamayı Çalıştırma

Uygulamayı başlatın ve **Movies Index** sayfasına gidin.

Artık filmleri, sayfa başına iki öğe olacak şekilde sayfalandırabilirsiniz.

![1762703170005](image/10_BuildaBlazormoviedatabaseapp(Part8-Addinteractivity)/1762703170005.png)

# ⚡ Etkileşimli Bileşen ve Canlı Sayfalama

Bileşen artık **etkileşimli** hale geldi.

Sayfalama işlemi gerçekleştiğinde  **sayfa yeniden yüklenmez** .

Sayfalama işlemi, **tarayıcı ile sunucu arasındaki SignalR bağlantısı** üzerinden **canlı olarak** gerçekleştirilir.

Sayfalama işlemi sunucuda yapılır ve sonuç, tarayıcıda görüntülenmek üzere istemciye geri gönderilir.

---

## 📄 Sayfa Başına Öğe Sayısını Güncelleme

Aşağıdaki kod satırında, her sayfada gösterilecek öğe sayısını **5** olarak ayarlayın:

```diff
- private PaginationState pagination = new PaginationState { ItemsPerPage = 2 };
+ private PaginationState pagination = new PaginationState { ItemsPerPage = 5 };
```

---

# 🔤 QuickGrid’i Sıralanabilir Hale Getirme

`Index` bileşenini açın (`Components/Pages/Movies/Index.razor`).

**Title** sütununu sıralanabilir yapmak için `PropertyColumn<TGridItem,TProp>` öğesine `Sortable="true"` özniteliğini ekleyin:

```diff
- <PropertyColumn Property="movie => movie.Title" />
+ <PropertyColumn Property="movie => movie.Title" Sortable="true" />
```

---

Artık **Title (Başlık)** sütununa tıklayarak filmleri başlığa göre sıralayabilirsiniz.

Sıralama işlemi sırasında sayfa yeniden yüklenmez.

İşlem, **SignalR bağlantısı üzerinden canlı olarak** sunucuda gerçekleştirilir ve sıralanmış sonuçlar istemciye gönderilir.



![1762703206652](image/10_BuildaBlazormoviedatabaseapp(Part8-Addinteractivity)/1762703206652.png)


# 🔍 Başlığa Göre Arama için C# Kodu ve Etkileşim Kullanma

Öğretici serisinin önceki bölümlerinde, **Index** bileşeni kullanıcının **filmleri başlığa göre filtrelemesine** olanak tanıyacak şekilde değiştirilmişti.

Bu işlem şu şekilde gerçekleştirilmişti:

* Kullanıcının arama ifadesini sorgu dizesiyle (`?titleFilter=road+warrior` gibi) sunucuya gönderen bir **HTML formu** eklenmişti.
* Bileşene, bu sorgu dizesini okuyup veritabanı kayıtlarını filtreleyen kod eklenmişti.

Bu yaklaşım, yalnızca **statik SSR** kullanan bileşenlerde etkiliydi — yani istemci ve sunucu arasındaki tek etkileşim **HTTP istekleri**yle sınırlıydı.

SignalR bağlantısı yoktu ve uygulama, kullanıcının bileşen arayüzündeki eylemlerine **canlı C# kodu** ile tepki veremiyordu.

Artık bileşen **etkileşimli** olduğundan, Blazor’un **veri bağlama (binding)** ve **olay işleme (event handling)** özellikleriyle gelişmiş bir kullanıcı deneyimi sağlanabilir.

---

## 🧩 Olay İşleyici Ekleme

Kullanıcının tetikleyeceği bir **delegate event handler** ekleyin.

Bu yöntem, `TitleFilter` özelliğinin değerini kullanarak veritabanındaki film kayıtlarını filtreleyecek.

Kullanıcı `TitleFilter`’ı temizleyip arama yaparsa, tüm film listesi yeniden yüklenecektir.

Aşağıdaki satırları `@code` bloğundan  **silin** :

```diff
- [SupplyParameterFromQuery]
- private string? TitleFilter { get; set; }
  
- private IQueryable<Movie> FilteredMovies =>
-     context.Movie.Where(m => m.Title!.Contains(TitleFilter ?? string.Empty));
```

---

### 🔁 Yerine aşağıdaki kodu ekleyin:

```csharp
private string titleFilter = string.Empty;

private IQueryable<Movie> FilteredMovies => 
    context.Movie.Where(m => m.Title!.Contains(titleFilter));
```

---

## 🧷 Giriş Alanına Veri Bağlama

Şimdi bileşen, `titleFilter` alanını bir `<input>` öğesine bağlamalıdır.

Kullanıcı giriş yaptığında, değer **titleFilter değişkeninde** saklanır.

Bu bağlama işlemi Blazor’da `@bind` yönergesiyle yapılır.

Aşağıdaki **HTML formunu** bileşenden kaldırın:

```diff
- <form action="/movies" data-enhance>
-     <input type="search" name="titleFilter" />
-     <input type="submit" value="Search" />
- </form>
```

---

### 🔄 Yerine aşağıdaki Razor işaretlemesini ekleyin:

```razor
<input type="search" @bind="titleFilter" @bind:event="oninput" />
```

`@bind:event="oninput"`, kullanıcının arama kutusuna her karakter girdiğinde **oninput** olayı tetiklendiğinde bağlama işlemini gerçekleştirir.

QuickGrid, `FilteredMovies`’a bağlı olduğu için, `titleFilter` değeri değiştikçe bileşen yeniden render edilir ve filtreleme otomatik olarak uygulanır.

---

## ▶️ Uygulamayı Çalıştırma

Uygulamayı başlatın ve arama alanına **“Road Warrior”** yazın.

Her karakter girdiğinizde  **QuickGrid** ’in filtrelendiğini göreceksiniz.

Arama kutusu **“Road ”** (boşluk dahil) haline geldiğinde yalnızca **The Road Warrior** filmi listede kalacaktır.


![1762703277743](image/10_BuildaBlazormoviedatabaseapp(Part8-Addinteractivity)/1762703277743.png)


# ⚡ Etkileşimli Filtreleme ve Sunucu Tarafı İşleme

Filtreleme işlemi **sunucuda** gerçekleştirilir ve sunucu, **SignalR bağlantısı** üzerinden **HTML çıktısını etkileşimli olarak** istemciye gönderir.

Sayfa  **yeniden yüklenmez** .

Kullanıcı, sanki kod tarayıcıda çalışıyormuş gibi bir deneyim yaşar — ancak gerçekte kod  **sunucuda çalışmaktadır** .

---

## 🧠 JavaScript Yerine Blazor Kullanımı

Bu senaryoda, **HTML formu gönderimi** yerine **JavaScript** de kullanılabilirdi.

Örneğin, **Fetch API** veya **XMLHttpRequest API** kullanılarak istek sunucuya gönderilebilirdi.

Ancak çoğu durumda, bu tarz etkileşimler **JavaScript kullanmadan** yalnızca **Blazor ve C#** ile yapılabilir.

Bu, Blazor’un **etkileşimli bileşen** mimarisi sayesinde mümkündür.

---

# 🎨 QuickGrid Bileşenini Stilleme

**QuickGrid** bileşenine özel stiller uygulamak için **CSS izolasyonu (CSS isolation)** kullanabilirsiniz.

CSS izolasyonu, bileşene özel bir stil dosyası ekleyerek uygulanır.

Dosya adı şu biçimdedir:

```
{BİLEŞEN_ADI}.razor.css
```

---

## 🧾 Örnek: Index Bileşeni için CSS Dosyası

`MoviePages` klasörüne aşağıdaki dosyayı ekleyin:

**Components/Pages/MoviePages/Index.razor.css**

```css
::deep tr {
    height: 3em;
}

::deep tr > td {
    vertical-align: middle;
}
```

> `::deep` pseudo-element yalnızca **alt öğelerde** (descendant elements) çalışır.
>
> Bu nedenle, QuickGrid bileşeni bir `<div>` veya benzeri **blok düzeyinde (block-level)** bir öğe içine alınmalıdır.

---

## 🧱 Index.razor Dosyasını Güncelleme

`Components/Pages/MoviePages/Index.razor` dosyasında, **QuickGrid** bileşenini `<div>` etiketleriyle sarın:

```diff
+ <div>
    <QuickGrid ...>
        ...
    </QuickGrid>
+ </div>
```

---

## ⚙️ Blazor ve CSS Entegrasyonu

Blazor, CSS seçicilerini bileşenin render ettiği HTML yapısına göre otomatik olarak yeniden yazar.

Yeniden yazılmış bu CSS kuralları **paketlenir** ve **statik varlık (static asset)** olarak sunulur.

Bu nedenle, stillerin **QuickGrid** bileşenine uygulanması için ek bir işlem yapmanız gerekmez.

![1762703506662](image/10_BuildaBlazormoviedatabaseapp(Part8-Addinteractivity)/1762703506662.png) 


# 🧹 Temizlik

Öğreticiyi tamamladıktan ve örnek uygulamayı yerel sisteminizden sildikten sonra, **BlazorWebAppMovies** veritabanını da manuel olarak silebilirsiniz.

Veritabanının konumu, kullanılan **platforma ve işletim sistemine** bağlı olarak değişir.

Ancak, **appsettings.json** dosyasındaki **veritabanı bağlantı dizesinde (connection string)** belirtilen dosya adını arayarak konumunu bulabilirsiniz.

---

# 🎉 Tebrikler!

Blazor öğretici serisini tamamladığınız için **tebrikler!** 👏

Bu seride Blazor’un temel özelliklerini öğrendiniz.

Blazor, burada ele alınandan çok daha fazla özelliğe sahiptir.

Daha fazlasını öğrenmek için  **Blazor belgelerini** , **örnek uygulamaları** ve **kaynak kodlarını** keşfetmenizi öneririz.

💻 Blazor ile mutlu kodlamalar! 🚀
