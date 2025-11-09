# ✅ Blazor film veritabanı uygulaması oluşturun (Bölüm 5 - Doğrulama ekleme)

## 🧰 Araçlarınızı seçin

Bu makale, film veritabanı yönetim özelliklerine sahip bir **ASP.NET Core Blazor Web Uygulaması** oluşturmayı öğreten serinin  **beşinci bölümüdür** .

Bu bölüm, **Movie modeli** üzerindeki meta verilerin, filmleri oluşturma ve düzenleme formlarında kullanıcı girdilerini doğrulamak için nasıl kullanıldığını açıklar.

---

## 🧾 Veri ek açıklamaları (Data Annotations) ile doğrulama

Doğrulama kuralları, model sınıfı üzerinde **veri ek açıklamaları (data annotations)** kullanılarak tanımlanır.

Aşağıda, bir form modelinin genel özelliklerinde kullanıcı girişini doğrulamak için kullanılabilen bazı `System.ComponentModel.DataAnnotations` öznitelikleri verilmiştir:

* **[Required]** : Kullanıcının bir değer girmesini zorunlu kılar.
* **[StringLength]** : Minimum ve maksimum karakter uzunluğunu belirtir. (Not: `MinimumLength` değeri, alanı zorunlu yapmaz; bunun için ayrıca `[Required]` eklenmelidir.)
* **[RegularExpression]** : Kullanıcı girişinin belirli bir desenle eşleşmesini sağlar.
* **[Range]** : Minimum ve maksimum sayısal değer aralığını belirtir.

> 💡 Not: `decimal`, `int`, `float`, `DateOnly`, `TimeOnly` ve `DateTime` gibi değer türleri zaten zorunludur; bu nedenle `[Required]` eklemeye gerek yoktur.

Daha fazla veri ek açıklaması örneği için Blazor belgelerine bakabilirsiniz.

---

## 🎬 Movie modeline doğrulama ekleme

Aşağıdaki  **data annotation** ’ları `Movie` sınıfına ekleyin.

Tüm özellikleri güncellemek için aşağıdaki örnekte gösterilen `Models/Movie.cs` dosyasını kopyalayıp yapıştırabilirsiniz.

**Eklenen öznitelikler:**

```diff
+ [Required]
+ [StringLength(60, MinimumLength = 3)]
  public string? Title { get; set; }

+ [Required]
* [StringLength(30)]
+ [RegularExpression(@"^[A-Z]+[a-zA-Z()\s-]*$")]
  public string? Genre { get; set; }

+ [Range(0, 100)]
  [DataType(DataType.Currency)]
  [Column(TypeName = "decimal(18, 2)")]
  public decimal Price { get; set; }
```

📄 **Models/Movie.cs (tam hali):**

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorWebAppMovies.Models;

public class Movie
{
    public int Id { get; set; }

    [Required]
    [StringLength(60, MinimumLength = 3)]
    public string? Title { get; set; }

    public DateOnly ReleaseDate { get; set; }

    [Required]
    [StringLength(30)]
    [RegularExpression(@"^[A-Z]+[a-zA-Z()\s-]*$")]
    public string? Genre { get; set; }

    [Range(0, 100)]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }
}
```

> ⚠️ Yukarıdaki doğrulama kuralları yalnızca **örnek amaçlıdır** ve üretim sistemleri için optimize edilmemiştir.
>
> Örneğin, bu doğrulama bir veya iki karakterli film adlarını reddeder ve tür (genre) alanında özel karakterlere izin vermez.

---

## 🧩 EF Core Migration oluşturma ve veritabanını güncelleme

Bir  **veri modeli şeması** , verilerin ilişkisel veritabanında nasıl düzenlendiğini ve birbirine bağlandığını tanımlar.

Model sınıfına ek açıklamalar eklemek, veritabanı şemasını  **otomatik olarak değiştirmez** .

Örneğin `Title` özelliğine uygulanan özniteliklere bakalım:

```csharp
[Required]
[StringLength(60, MinimumLength = 3)]
public string? Title { get; set; }
```

| Kısıtlama      | Model Title özelliği | Veritabanı Title sütunu         |
| ---------------- | ---------------------- | --------------------------------- |
| Maksimum uzunluk | 60 karakter            | ~2 GB byte çifti (NVARCHAR(MAX)) |
| Zorunluluk       | ✔️`[Required]`     | ❌ NULL değere izin verir        |

> 💡 Veritabanındaki `NVARCHAR(MAX)` sütunu yaklaşık 2 GB veri saklayabilir, bu da modeldeki 60 karakter sınırını çok aşar. Bu fark, model ile veritabanı arasında **uyumsuzluk** yaratır.

**Doğru eşleşme için:**

Veritabanındaki sütun `NVARCHAR(60)` ve `NOT NULL` olmalıdır.

Model ve veritabanı şeması farklı olduğunda:

* Eğer model sınırı  **daha küçükse** , veritabanı fazla uzun veriyi kabul eder ama uygulama bunu denetleyemez.
* Eğer model sınırı  **daha büyükse** , veritabanı hata atabilir veya veriyi  **kesebilir (truncate)** .

Bu yüzden model ve veritabanı şeması  **her zaman uyumlu olmalıdır** .

---

## ⚙️ EF Core Migration komutları

Modelle veritabanı şemasını uyumlu hale getirmek için yeni bir **EF Core migration** oluşturun.

Migration isimleri, versiyon kontrol sistemlerindeki commit mesajlarına benzer bir şekilde tanımlayıcı olmalıdır.

Burada örnek olarak `"NewMovieDataAnnotations"` kullanılacaktır.

> ⚠️ Devam etmeden önce uygulamanın çalışmadığından emin olun.

### 🔴 Uygulamayı durdurma yolları:

* **Visual Studio:** Tarayıcı penceresini kapatın.
* **VS Code:** Tarayıcıyı kapatın ve `Shift+F5` veya **Run > Stop Debugging** seçeneğini kullanın.
* **.NET CLI:** Tarayıcıyı kapatın ve komut satırında  **Ctrl+C** ’ye basın.

---

### 🛠️ Migration ekleme

Proje kök dizininde aşağıdaki komutu çalıştırın:

```bash
dotnet ef migrations add NewMovieDataAnnotations
```

### 💾 Migration’ı veritabanına uygulama

```bash
dotnet ef database update
```

---

## 🧮 Model ve veritabanı şema uyumu

Migration tamamlandıktan sonra, model özellikleri ve veritabanı sütunları aşağıdaki gibi eşleşir:

| Kısıtlama      | Model Title özelliği | Veritabanı Title sütunu |
| ---------------- | ---------------------- | ------------------------- |
| Maksimum uzunluk | 60 karakter            | NVARCHAR(60)              |
| Zorunluluk       | ✔️`[Required]`     | ✔️`NOT NULL`          |

> 💡 `NVARCHAR(60)` sütunu, Unicode aralığı 0–65.535 arasındaki karakterleri kullanıyorsanız 60 karakter depolayabilir.

---

## 🧭 Sorun giderme

Eğer eğitim boyunca çözümleyemediğiniz bir sorunla karşılaşırsanız, kodunuzu **Blazor örnek deposundaki tamamlanmış proje** ile karşılaştırabilirsiniz:

🔗 [Blazor samples GitHub repository (dotnet/blazor-samples)](https://github.com/dotnet/blazor-samples)

Bu eğitimin örnek proje klasörü: **BlazorWebAppMovies**

---

## 📚 Ek kaynaklar

* Tag Helpers in forms in ASP.NET Core
* Globalization and localization in ASP.NET Core
* Author Tag Helpers in ASP.NET Core
* EF Core Migrations Overview
* nchar and nvarchar (Transact-SQL)
* Blazor enhanced forms

---

✨ Böylece Movie modeli artık veri doğrulama kurallarına sahip hale geldi ve EF Core migration ile veritabanı şeması güncellendi.
