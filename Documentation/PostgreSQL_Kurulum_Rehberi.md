# PostgreSQL Kurulumu ve pgAdmin Bağlantısı (ZIP Yöntemi - Windows)

Bu rehber, PostgreSQL'i Windows üzerinde **ZIP dosyası** ile manuel olarak kurmak ve **pgAdmin 4** üzerinden görsel yönetim sağlamak isteyenler için hazırlanmıştır.

---

## ✅ GEREKLİLER

- Windows işletim sistemi
- Yönetici yetkisine sahip kullanıcı
- Kurulum yapılacak boş bir klasör (örneğin: `C:\PostgreSQL`)
- PostgreSQL ZIP binary dosyası
- pgAdmin 4 kurulumu

---

## 🧩 ADIM 1 — PostgreSQL ZIP Dosyasını İndir

- PostgreSQL 16 (ZIP):  
  [https://get.enterprisedb.com/postgresql/postgresql-16.2-1-windows-x64-binaries.zip](https://get.enterprisedb.com/postgresql/postgresql-16.2-1-windows-x64-binaries.zip)

- ZIP dosyasını çıkar ve şu klasöre yerleştir:

```bash
C:\PostgreSQL
```

---

## 🛠️ ADIM 2 — `data` Klasörü Oluştur ve `initdb`

### PowerShell’i yönetici olarak aç:

```powershell
cd C:\PostgreSQL\bin
New-Item -ItemType Directory -Path "C:\PostgreSQL\data"
```

### Veritabanı başlat:

```powershell
.\initdb.exe -D "C:\PostgreSQL\data" -U postgres -W --locale=C
```

> Şifre olarak örneğin: `postgres123!` ver

---

## 🚀 ADIM 3 — PostgreSQL Sunucusunu Başlat

```powershell
.\pg_ctl.exe -D "C:\PostgreSQL\data" -l logfile start
```

---

## 🔌 ADIM 4 — pgAdmin 4 Kurulumu

1. pgAdmin 4 indir:  
   [https://www.pgadmin.org/download/pgadmin-4-windows/](https://www.pgadmin.org/download/pgadmin-4-windows/)

2. “Install for me only” seçeneğiyle kurulumu tamamla

---

## 🔐 ADIM 5 — pgAdmin'de Sunucu Ekle

1. `Servers` sağ tık → `Register > Server...`
2. **General sekmesi**:  
   - Name: `PostgreSQL`
3. **Connection sekmesi**:

| Alan             | Değer         |
|------------------|---------------|
| Host             | localhost     |
| Port             | 5432          |
| Maintenance DB   | postgres      |
| Username         | postgres      |
| Password         | postgres123!  |
| Save Password    | ✅             |

---

## 🧱 ADIM 6 — Veritabanı ve Tablo Oluşturma

### Yeni Veritabanı:

```sql
CREATE DATABASE chatbotdb;
```

### Örnek Tablo:

```sql
CREATE TABLE chatlog (
    id SERIAL PRIMARY KEY,
    message TEXT NOT NULL,
    response TEXT NOT NULL,
    timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

---

## 🔁 PostgreSQL Sunucusu Çalışmazsa

Eğer bilgisayar yeniden başlatıldıysa ve PostgreSQL sunucusu kapanmışsa, tekrar manuel başlatmak için:

```powershell
cd C:\PostgreSQL\bin
.\pg_ctl.exe -D "C:\PostgreSQL\data" -l logfile start
```

### Sunucunun Durumunu Kontrol Et:

```powershell
.\pg_ctl.exe -D "C:\PostgreSQL\data" status
```

---

## 🎉 TAMAMLANDI

Artık:

- PostgreSQL çalışıyor
- pgAdmin bağlantılı
- Tablolar görsel olarak yönetilebilir
- .NET projeleriyle bağlanmaya hazır
