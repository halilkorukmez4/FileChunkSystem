FileChunkSystem Nas�l �al��t�r�l�r?
===================================

Bu proje, dosyalar� par�alara b�lerek MongoDB ve dosya sistemine kaydeder. Metadata bilgilerini PostgreSQL �zerinde tutar. 
Y�klenen dosyalar daha sonra tekrar birle�tirilerek indirilebilir.

GEREKL� ARA�LAR
---------------
- Docker Desktop (MongoDB ve PostgreSQL�i aya�a kald�rmak i�in kullan�l�r)  

1. Docker ile MongoDB ve PostgreSQL�i aya�a kald�r
--------------------------------------------------
Proje dizininde 'Src' klas�r�ne terminal ile girin ve a�a��daki komutu �al��t�r�n:

    docker-compose up -d

Bu i�lem MongoDB ve PostgreSQL servislerini �al��t�r�r. Uygulama bu servisleri kullan�r.

2. Migration i�lemleri (Sadece ilk kurulumda yap�l�r)
-----------------------------------------------------
Terminalde a�a��daki klas�re gidin:

    cd Src/Services/FileChunkSystem.ConsoleApp

�nce migration olu�turun:

    dotnet ef migrations add InitialCreate --project ../FileChunkSystem.Infrastructure --startup-project . --context ApplicationDbContext --output-dir Migrations

Ard�ndan veritaban�na migration'� uygulay�n:

    dotnet ef database update --project ../FileChunkSystem.Infrastructure --startup-project . --context ApplicationDbContext

3. Uygulamay� �al��t�r
----------------------
Migration i�lemi bittikten sonra uygulamay� �al��t�rmak i�in �u komutu yaz�n:

    dotnet run --project Src/Services/FileChunkSystem.ConsoleApp

Uygulama ba�lad���nda �rnek dosyalar� par�alayarak MongoDB veya dosya sistemine y�kler. 
Ard�ndan dosyalar� tekrar indirerek 'C:\Temp' klas�r�ne kaydeder.

Not:
----
Uygulama sadece konsol olarak lokal �al���r. 
Sadece veritabanlar� (Mongo ve PostgreSQL) Docker i�inden y�netilir. Uygulaman�n kendisi Docker�a al�nmaz.