# Fatura oluşturma ve bilgilendirme projesi
Bu uygulama kullanıcıya satın aldığı ürünlerin faturasını oluşturan ve işlenen faturaları belli bir saatte kullanıcı mailine mail göndermesini sağlayan **.NET Core 8 Web API** projesidir.

# Proje derleme ve ayağa kaldırma
1- dotnet restore
2- dotnet ef migrations add InitialCreate
3- dotnet ef database update
4- dotnet run
5- http://localhost:[port]/swagger

# Proje mimarisi

Projede Hexagonal mimari kullanlamıştır:

### 1. **Domain**
- Entity ve domain kuralarnın bulunduğu katmandır.

### 2. **Application**
- İş mantıklarının ve kurallarının yazıldığı katmandır.

### 3. **Infrastructor**
- Veri tabanı işlemlerininn ve diğer dış dünya işlerinin yapldığı katmandır.

## Teknik Detaylar

### Backend
- **Dil**: C#
- **Framework**: .NET Core 8 Web API
- **Mimari**: Hexagonal mimarri
- **Veritabanı**: MSSQL

### Kullanılan Teknolojiler
- **Entity Framework Core**: Veritabanı işlemleri için.
- **Serilog**: Log işlemleri için.
- **MailKit-MimeKit**: Mail göndermek için.
- **NCrontab** Zamanlanmış görevler için.

## Notlar

- Zamanlanmış görevin çalıştığını ve mail gönderrildiğini test etmek için InvoiceNotificationBackgroundService içindeki
this._schedule = CrontabSchedule.Parse("* * * * *");
this._nextRun = DateTime.Now.AddSeconds(-1); 
kullanınız
- Test için SeedData eklenmmiştir.

