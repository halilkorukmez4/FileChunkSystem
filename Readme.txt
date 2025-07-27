FileChunkSystem Nasýl Çalýþtýrýlýr?
===================================

Bu proje, dosyalarý parçalara bölerek MongoDB ve dosya sistemine kaydeder. Metadata bilgilerini PostgreSQL üzerinde tutar. 
Yüklenen dosyalar daha sonra tekrar birleþtirilerek indirilebilir.

GEREKLÝ ARAÇLAR
---------------
- Docker Desktop (MongoDB ve PostgreSQL’i ayaða kaldýrmak için kullanýlýr)  

1. Docker ile MongoDB ve PostgreSQL’i ayaða kaldýr
--------------------------------------------------
Proje dizininde 'Src' klasörüne terminal ile girin ve aþaðýdaki komutu çalýþtýrýn:

    docker-compose up -d

Bu iþlem MongoDB ve PostgreSQL servislerini çalýþtýrýr. Uygulama bu servisleri kullanýr.

2. Migration iþlemleri (Sadece ilk kurulumda yapýlýr)
-----------------------------------------------------
Terminalde aþaðýdaki klasöre gidin:

    cd Src/Services/FileChunkSystem.ConsoleApp

Önce migration oluþturun:

    dotnet ef migrations add InitialCreate --project ../FileChunkSystem.Infrastructure --startup-project . --context ApplicationDbContext --output-dir Migrations

Ardýndan veritabanýna migration'ý uygulayýn:

    dotnet ef database update --project ../FileChunkSystem.Infrastructure --startup-project . --context ApplicationDbContext

3. Uygulamayý çalýþtýr
----------------------
Migration iþlemi bittikten sonra uygulamayý çalýþtýrmak için þu komutu yazýn:

    dotnet run --project Src/Services/FileChunkSystem.ConsoleApp

Uygulama baþladýðýnda örnek dosyalarý parçalayarak MongoDB veya dosya sistemine yükler. 
Ardýndan dosyalarý tekrar indirerek 'C:\Temp' klasörüne kaydeder.

Not:
----
Uygulama sadece konsol olarak lokal çalýþýr. 
Sadece veritabanlarý (Mongo ve PostgreSQL) Docker içinden yönetilir. Uygulamanýn kendisi Docker’a alýnmaz.