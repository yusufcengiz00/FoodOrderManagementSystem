# FoodOrder - Yemek Sipariş Yönetim Sistemi

FoodOrder, .NET 10 MVC ve Dapper ORM kullanılarak geliştirilmiş, LocalDB MS SQL Server veritabanı altyapısına sahip şık, modern ve premium tasarımlı bir yemek sipariş yönetim sistemidir.

---

## 🚀 Teknolojiler ve Altyapı

- **Framework**: .NET 10 (ASP.NET Core MVC)
- **ORM / Veri Tabanı**: Dapper & Microsoft.Data.SqlClient (Stored Procedure tabanlı mimari)
- **Veri Tabanı Sunucusu**: MS SQL Server LocalDB (`(localdb)\MSSQLLocalDB`)
- **Yetkilendirme (Auth)**: ASP.NET Core Session tabanlı güvenli oturum yönetimi
- **Tasarım & Stil**: Özelleştirilmiş modern koyu tema (Dark Mode CSS)
- **Belge Çıktıları**:
  - **PDF**: `html2pdf.js` ile istemci tarafında doğrudan PDF indirme
  - **Excel**: Noktalı virgül (`;`) ayraçlı, UTF-8 BOM kodlamalı (Türkçe karakter uyumlu) CSV dışa aktarımı

---

## ✨ Ana Özellikler

1. **Giriş Yap & Kayıt Ol (Authentication)**:
   - Kullanıcıların sisteme kaydolmasını ve güvenli şekilde giriş yapmasını sağlayan Session korumalı yetkilendirme sistemi.
2. **Yönetici Paneli (Dashboard)**:
   - Toplam Ciro, Toplam Sipariş, Toplam Kullanıcı ve Toplam Ürün sayılarının anlık gösterimi.
   - Son eklenen 5 siparişin hızlı görünümü ve hızlı işlem butonları.
3. **Sipariş ve Ürün Yönetimi**:
   - Müşterilere yeni sipariş oluşturma ve siparişlere ürün/adet ekleme arayüzü.
   - Ürün ekleme, silme, düzenleme ve listeleme paneli.
4. **Sipariş Fişi Arayüzü**:
   - Ürün adetlerinin net gösterildiği ("3 adet") şık ve minimalist fiş görünümü.
   - Fiş detaylarını tek tıkla **PDF olarak indirme** seçeneği.
   - Fiş detaylarını Excel uyumlu **CSV formatında dışa aktarma** seçeneği.

---

## 🛠️ Kurulum ve Çalıştırma

### 1. Veri Tabanının Hazırlanması
Proje, MS SQL Server LocalDB altyapısını kullanmaktadır. Veri tabanını oluşturmak için:
1. SQL Server Management Studio (SSMS) veya Visual Studio veritabanı aracını açın.
2. Sunucu adı olarak `(localdb)\MSSQLLocalDB` adresine bağlanın.
3. `FoodOrderManagementSystem/Database/FoodOrderDB.sql` dosyasının içeriğini kopyalayarak yeni bir sorgu penceresinde çalıştırın.
4. Bu işlem `FoodOrderDB` adında veritabanını, tabloları, stored procedure'leri ve örnek test verilerini otomatik olarak oluşturacaktır.

### 2. Projenin Çalıştırılması
Terminal veya PowerShell üzerinden projenin ana dizinine gidin ve aşağıdaki komutları çalıştırın:

```bash
# Projeyi derleyin
dotnet build

# Projeyi çalıştırın
dotnet run
```

Uygulama çalıştıktan sonra tarayıcınızda terminalde belirtilen adrese (örneğin `https://localhost:7001` veya `http://localhost:5001`) giderek giriş yapabilir veya yeni bir hesap oluşturabilirsiniz.

---

## 📂 Proje Dizin Yapısı

- **Controllers/**: Oturum yönetimi, sipariş, ürün, kullanıcı ve dashboard işlemlerini yöneten sınıflar.
  - `BaseController.cs`: Yetkisiz girişleri önleyen temel güvenlik sınıfı.
  - `AccountController.cs`: Giriş ve kayıt işlemlerini yürütür.
  - `AdminController.cs`: Kontrol paneli istatistiklerini hesaplar.
- **Database/**: `FoodOrderDB.sql` veritabanı kurulum scriptini barındırır.
- **Models/**: Dapper modelleri ve veritabanı bağlantı metotlarını barındıran `Context.cs` sınıfı.
- **Views/**: Razor sayfaları (.cshtml) ve tasarım şablonları.
  - `Account/`: Giriş ve Kayıt ekranları.
  - `Admin/`: Dashboard gösterge paneli.
  - `OrderDetail/Receipt.cshtml`: Sipariş fişi ve PDF/Excel indirme butonları.
