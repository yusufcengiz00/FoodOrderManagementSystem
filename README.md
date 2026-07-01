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
Proje, MS SQL Server LocalDB altyapısını kullanmaktadır. Sunucu adı `(localdb)\MSSQLLocalDB` olarak bağlanıp `FoodOrderDB` adında bir veritabanının, tabloların ve stored procedure'lerin tanımlanmış olması gerekmektedir.

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
- **Models/**: Dapper modelleri ve veritabanı bağlantı metotlarını barındıran `Context.cs` sınıfı.
- **Views/**: Razor sayfaları (.cshtml) ve tasarım şablonları.
  - `Account/`: Giriş ve Kayıt ekranları.
  - `Admin/`: Dashboard gösterge paneli.
  - `OrderDetail/Receipt.cshtml`: Sipariş fişi ve PDF/Excel indirme butonları.


      ---


## 📸 Uygulama Görselleri

### 👤 Kullanıcı Portalı

<p align="center">
  <img src="https://github.com/user-attachments/assets/dcd74207-9b18-43d6-9462-a5387038b8a1" width="900" alt="Kullanıcı Portalı" />
</p>

---


### 🔐 Admin Panel

<p align="center">
  <img src="https://github.com/user-attachments/assets/10f7d7c5-f5ec-4b75-9d59-37d481a2aacc" width="49%" alt="Login" />
  <img src="https://github.com/user-attachments/assets/d5d3f2db-2a16-439b-ad5e-7f066af56e9a" width="49%" alt="Register" />
</p>

<p align="center">
  <img src="https://github.com/user-attachments/assets/7a219e3c-899a-46fc-bd33-af7ebc9804f7" width="100%" alt="Dashboard" />
</p>

---

### 🛒 Sipariş Yönetim Ekranı

<p align="center">
  <img src="https://github.com/user-attachments/assets/94e00fc5-28c6-4389-8b50-09c4d6b74494" width="900" alt="Sipariş Yönetimi" />
</p>

---

### 🧾 Sipariş Fişleri Ekranı

<p align="center">
  <img src="https://github.com/user-attachments/assets/65cc5e7f-b48a-44a4-a31d-f8b07e20a763" width="900" alt="Sipariş Fişleri" />
</p>

<p align="center">
  <img src="https://github.com/user-attachments/assets/56cc2346-0b5b-4e35-bcd3-cb940e4d913d" width="600" alt="Fiş Detayı" />
</p>

---

### 📦 Ürün Yönetim Ekranı

<p align="center">
  <img src="https://github.com/user-attachments/assets/4d72500b-a4f8-4d42-b5b2-ace6a61267fd" width="900" alt="Ürün Yönetimi" />
</p>

---

### 👥 Kullanıcı Yönetim Ekranı

<p align="center">
  <img src="https://github.com/user-attachments/assets/14474a6c-c485-4c18-88a7-003c5c36ccfb" width="900" alt="Kullanıcı Yönetimi" />
</p>
```

