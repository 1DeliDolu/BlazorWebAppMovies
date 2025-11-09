
# 🎬 Blazor film veritabanı uygulaması oluşturma (Genel Bakış)

Bu eğitim, veritabanı, Entity Framework (EF) Core ve kullanıcı etkileşimi içeren bir Blazor Web Uygulaması oluşturmanın temellerini açıklar.

Bu serinin bölümleri şunları içerir:

* Bir Blazor Web Uygulaması oluşturun
* Bir modeli ekleyin ve iskeletini çıkarın
* Razor bileşenlerini öğrenin
* Bir veritabanıyla çalışın
* Doğrulama ekleyin
* Arama ekleyin
* Yeni bir alan ekleyin
* Etkileşim ekleyin

Eğitimin sonunda, bir film veritabanındaki filmleri görüntüleyip yönetebilen bir Blazor Web Uygulamasına sahip olacaksınız.

## 🔐 Üretim uygulamaları için güvenli kimlik doğrulama akışı gerekiyor

Bu eğitim, kullanıcı kimlik doğrulaması gerektirmeyen yerel bir veritabanı kullanır. Üretim uygulamaları, mevcut en güvenli kimlik doğrulama akışını kullanmalıdır. Dağıtılmış test ve üretim Blazor Web Uygulamaları için kimlik doğrulama hakkında daha fazla bilgi için aşağıdaki kaynaklara bakın:

* ASP.NET Core Blazor kimlik doğrulaması ve yetkilendirme
* ASP.NET Core Blazor kimlik doğrulaması ve yetkilendirme ve Sunucu güvenliği düğümündeki aşağıdaki makaleler
* OpenID Connect (OIDC) ile bir ASP.NET Core Blazor Web Uygulamasını güvenceye alın
* Microsoft Entra ID ile bir ASP.NET Core Blazor Web Uygulamasını güvenceye alın

Microsoft Azure hizmetleri için yönetilen kimliklerin kullanılmasını öneririz. Yönetilen kimlikler, uygulama kodunda kimlik bilgilerini depolamadan Azure hizmetlerine güvenli bir şekilde kimlik doğrulaması yapar. Daha fazla bilgi için aşağıdaki kaynaklara bakın:

* Azure kaynakları için yönetilen kimlikler nelerdir? (Microsoft Entra belgeleri)
* Azure hizmetleri belgeleri
* Azure SQL için Microsoft Entra’daki yönetilen kimlikler
* App Service ve Azure Functions için yönetilen kimlikleri nasıl kullanılır

## 🧪 Örnek uygulama

Makaleyi okurken örnek uygulamayı oluşturmayı düşünmüyorsanız, Blazor örnekleri GitHub deposundaki (dotnet/blazor-samples) tamamlanmış örnek uygulamaya başvurabilirsiniz. Depodaki en son sürüm klasörünü seçin. Bu eğitimin projesi için örnek klasörün adı  **BlazorWebAppMovies** ’tir.

## 🧾 Makale kod örnekleri

ASP.NET Core belgelerinde gösterilen kod örneklerinin satır sonları, bir uygulama için araçlar tarafından oluşturulan iskelet kodundaki satır sonlarıyla çoğu zaman eşleşmez. Bu, bir makale yayınlama sınırlamasından kaynaklanır. Makalelerdeki kod satırları genel olarak 85 karakter uzunlukla sınırlıdır ve yayınlama yönergelerimizi karşılamak için satır uzunluğunu satır sonları ekleyerek manuel olarak ayarlarız.

Bu eğitimi çalışırken veya başka herhangi bir ASP.NET Core makalesinin kod örneklerini kullanırken, uygulamanızdaki iskelet kodunu makaledeki kod örneklerinin satır sonlarına uyacak şekilde asla ayarlamanız gerekmez.

## 🐞 Eğitimle ilgili bir sorun bildirin

Serinin bir makalesi için bir GitHub dokümantasyon sorunu açmak üzere, makalenin altındaki **Open a documentation issue** bağlantısını kullanın. Sorununuzu oluşturmak için bağlantıyı kullanmak, soruna önemli izleme meta verileri ekler ve makalenin yazarını otomatik olarak bilgilendirir.

## 🆘 Destek istekleri

Eğitimin makaleleri hakkında hata raporları ve metinle ilgili yorumlar gibi geri bildirimleri memnuniyetle karşılarız, ancak çoğu zaman ürün desteği sağlayamayız. Eğitimi takip ederken bir sorunla karşılaşırsanız, hemen bir dokümantasyon sorunu açmayın. Çünkü birçok sorun adım atlanmasına veya bir adımın doğru şekilde izlenmemesine kadar izlenebilir; sorun açmadan önce attığınız adımları makaleyle karşılaştırın ve kodunuzu örnek uygulamayla kıyaslayın.

Eğitimin ve başvuru belgelerinin ötesinde .NET ve Blazor hakkında genel sorular için veya .NET topluluğundan yardım almak için, herkese açık forumlarda geliştiricilerle iletişim kurun.
