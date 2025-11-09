# 🎬 Blazor film veritabanı uygulaması (Bölüm 7 - Yeni bir alan ekleme)

## 🧰 Araç Seçimi

Bu makale, bir film veritabanını yönetme özelliklerine sahip bir **ASP.NET Core Blazor Web Uygulaması** oluşturmanın temellerini öğreten **Blazor film veritabanı öğreticisinin yedinci bölümü**dür.

Bu bölüm,  **Movie sınıfına** , **CRUD sayfalarına** ve **veritabanına yeni bir alan eklemeyi** kapsar.

---

## 🧩 Veritabanı güncellemesi ve EF Core göçleri (migrations)

Veritabanı güncellemesi, **EF Core migrations** tarafından gerçekleştirilir.

EF Core, veritabanındaki değişiklikleri bir **göç geçmişi tablosunda (migration history table)** izler ve **model sınıflarıyla veritabanı tabloları senkronize olmadığında hata** verir.

Bu sistem, veritabanı tutarlılığıyla ilgili sorunları hızlıca çözmeyi sağlar.

> ⚠️ **Önemli:**
>
> Aşağıdaki adımlara geçmeden önce uygulamanın **çalışmadığından emin olun.**

---

## 🎞️ Modele film derecelendirmesi (Rating) ekleme

`Models/Movie.cs` dosyasını aç ve **Rating** özelliğini ekle.

Bu özellik, **Motion Picture Association** film derecelendirmelerine göre değerleri sınırlandırır:

```csharp
[Required]
[RegularExpression(@"^(G|PG|PG-13|R|NC-17)$")]
public string? Rating { get; set; }
```

---

## 🧱 CRUD bileşenlerine Rating alanını ekleme

### ➕ Create bileşeni

`Components/Pages/MoviePages/Create.razor` dosyasını aç.

**Price** bloğu ile **create** düğmesi arasına aşağıdaki kodu ekle:

```razor
<div class="mb-3">
    <label for="rating" class="form-label">Rating:</label> 
    <InputText id="rating" @bind-Value="Movie.Rating" class="form-control" /> 
    <ValidationMessage For="() => Movie.Rating" class="text-danger" /> 
</div>
```

---

### ❌ Delete bileşeni

`Components/Pages/MoviePages/Delete.razor` dosyasını aç.

**Price** için olan açıklama listesi (`<dl>`) bloğu ile **EditForm** bileşeni arasına şu bloğu ekle:

```razor
<dl class="row">
    <dt class="col-sm-2">Rating</dt>
    <dd class="col-sm-10">@movie.Rating</dd>
</dl>
```

---

### 🔍 Details bileşeni

`Components/Pages/MoviePages/Details.razor` dosyasını aç.

**Price** alanının hemen altına, kapanış `</dl>` etiketinden önce şunları ekle:

```razor
<dt class="col-sm-2">Rating</dt>
<dd class="col-sm-10">@movie.Rating</dd>
```

---

### ✏️ Edit bileşeni

`Components/Pages/MoviePages/Edit.razor` dosyasını aç.

**Price** bloğu ile **Save** düğmesi arasına şu kodu ekle:

```razor
<div class="mb-3">
    <label for="rating" class="form-label">Rating:</label>
    <InputText id="rating" @bind-Value="Movie.Rating" class="form-control" />
    <ValidationMessage For="() => Movie.Rating" class="text-danger" />
</div>
```

---

### 📋 Index bileşeni

`Components/Pages/MoviePages/Index.razor` dosyasını aç.

**Price** sütunundan hemen sonra aşağıdaki sütunu ekle:

```razor
<PropertyColumn Property="movie => movie.Rating" />
```

---

## 🌱 SeedData sınıfını güncelle

`Data/SeedData.cs` dosyasındaki **Mad Max** film bloğuna `Rating = "R"` satırını ekle:

```diff
new Movie
{
    Title = "Mad Max",
    ReleaseDate = DateOnly.Parse("1979-4-12"),
    Genre = "Sci-fi (Cyberpunk)",
    Price = 2.51M,
+   Rating = "R",
},
```

Aynı şekilde diğer filmlere de derecelendirmeleri ekle:

| Film                        | Rating |
| --------------------------- | ------ |
| The Road Warrior            | R      |
| Mad Max: Beyond Thunderdome | PG-13  |
| Mad Max: Fury Road          | R      |
| Furiosa: A Mad Max Saga     | R      |

Tüm dosyaları kaydet.

---

## 🧱 Uygulamayı derle (henüz çalıştırma)

Komut satırında, proje kök dizininde şu komutu çalıştır:

```bash
dotnet build
```

Herhangi bir hata varsa düzelt ve bir sonraki adıma geç.

---

## 🗄️ Veritabanını güncelle

Şu anda uygulamayı çalıştırırsan, **SQL hatası** oluşur çünkü veritabanında **Rating sütunu** yoktur.

Veritabanı ile model arasındaki bu farkı çözmek için 3 yaklaşım vardır:

1. **Veritabanı şemasını elle güncellemek**

   → Verileri korur ama karmaşıktır.
2. **Veritabanını silip yeniden oluşturmak**

   → Hızlı ama veriler kaybolur.
3. **EF Core Migration kullanmak** ✅ *(Bu öğreticide tercih edilen yöntem)*

---

## 🧭 Migration oluştur

Proje kök dizininde aşağıdaki komutu çalıştır:

```bash
dotnet ef migrations add AddRatingField
```

Bu komut:

* Movie modeli ile veritabanı tablosunu karşılaştırır,
* Eksik sütunlar için kod üretir.

---

## ⚙️ Migration dosyasını düzenle

`Migrations` klasöründeki dosyayı aç (`{ZAMAN}_AddRatingField.cs`).

**AddColumn** bloğundaki son satırı aşağıdaki gibi değiştir:

```diff
migrationBuilder.AddColumn<string>(
    name: "Rating",
    table: "Movie",
    type: "nvarchar(max)",
    nullable: false,
-   defaultValue: "");
+   defaultValue: "R");
```

Bu değişiklik, yeni sütuna varsayılan değer olarak **“R”** atar.

---

## 🗃️ Veritabanını güncelle

Aşağıdaki komutu çalıştırarak veritabanını güncelle:

```bash
dotnet ef database update
```

Bu işlem, mevcut verileri koruyarak **Rating sütununu** ekler.

---

## 🖋️ Verisi farklı olan filmi düzenle

Uygulamayı çalıştır ve:

1. **Mad Max: Beyond Thunderdome** filmini düzenle.
2. **Rating** alanını  **R** ’den  **PG-13** ’e değiştir.
3. Kaydet.

> 💡 Alternatif:
>
> Migration dosyasını düzenlemek yerine veritabanını silebilir ve uygulamayı yeniden çalıştırarak yeniden tohumlama (reseeding) yaptırabilirsin.

---

## 🧩 Uygulamayı test et

Uygulamayı çalıştır, yeni film ekle veya düzenle.

Artık her film için **Rating** alanı oluşturulduğunu, düzenlenebildiğini ve görüntülenebildiğini doğrula.

---

## 🛠️ Sorun giderme

Eğer veritabanı bozulursa:

1. Veritabanını sil (veritabanı aracında bağlantıyı kapat).
2. Aşağıdaki komutla mevcut migration’ları yeniden çalıştır:

```bash
dotnet ef database update
```

---

## 🧹 Tüm kayıtları silip veritabanını yeniden tohumlama

Yeni alan için varsayılan değer eklemenin bir alternatifi olarak:

1. Tüm film kayıtlarını sil:

   * Tarayıcıda Delete bağlantılarını kullanabilir,
   * Ya da SQL sorgusu çalıştırabilirsin:
     ```sql
     DELETE FROM dbo.Movie;
     ```
2. Uygulamayı yeniden çalıştır.

   Tohumlama kodu (SeedData) veritabanını otomatik olarak doğru **Rating** değerleriyle doldurur.

---

## 🧩 Tamamlanmış örnekle karşılaştır

Sorun yaşarsan, projenin tamamlanmış halini kontrol et:

🔗 **Blazor örnekleri GitHub deposu (dotnet/blazor-samples)**

Örnek klasör adı: **BlazorWebAppMovies**

---

## 📚 Ek kaynaklar

* [Migrations (EF Core belgeleri)](https://learn.microsoft.com/ef/core/managing-schemas/migrations)
* [Migration kodunu özelleştirme](https://learn.microsoft.com/ef/core/managing-schemas/migrations/customize)
