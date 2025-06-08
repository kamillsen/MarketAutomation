# Barkodlu Satış Sistemi - Teknik Dokümantasyon

Bu dökümantasyon, sistemin şu ana kadar tamamlanan işlevlerini, ilgili sınıfları, veritabanı işlemlerini ve UI etkileşimlerini profesyonel formatta açıklar.

## 🔧 Kullanılan Teknolojiler
- .NET 6 (Windows Forms)
- PostgreSQL (barcode_db veritabanı)
- Npgsql (PostgreSQL bağlantısı)
- C#

## 📂 Ana Dosya ve Sınıflar

### 1. `DatabaseService.cs`
Veritabanı işlemlerinden sorumlu katmandır. PostgreSQL bağlantısını yönetir, ürün ve satış işlemleri burada gerçekleştirilir.

#### Görevleri:
| Fonksiyon | Açıklama |
|----------|----------|
| `GetProductByBarcode(barcode)` | Barkod numarasına göre ürün getirir |
| `GetAllProducts()` | Tüm ürünleri listeler |
| `ReduceStock(barcode, quantity)` | Satış sonrası stoktan ürün düşer |
| `InsertProduct(product)` | Yeni ürün veritabanına eklenir |
| `InsertSale(totalPrice)` | `sales` tablosuna toplam fiyatla satış kaydı ekler |
| `InsertSaleItem(saleId, barcode, qty, price)` | `sale_items` tablosuna her ürün için detaylı satış satırı ekler |

---

### 2. `MainForm.cs` (UI)
Kullanıcı arayüzüdür. Form üzerinden ürün okutma, sepet, satış işlemleri yönetilir.

#### Önemli Olaylar (Events) ve Görevleri:
| Event | Açıklama |
|-------|----------|
| `txtBarcode_KeyDown` | Enter ile barkod okutulduğunda ürün sepete eklenir |
| `UpdateCartView()` | Sepetteki ürünler ekrana yansıtılır |
| `btnShowProducts_Click()` | Tüm ürünler listelenir |
| `lvCart_DoubleClick()` | Kullanıcı sepetten ürünün adetini günceller |
| `btnCompleteSale_Click()` | Satışı tamamlar: stok düşer, veritabanına kayıt yapılır, sepet temizlenir |

---

### 3. `CartService.cs`
Sepet yönetiminden sorumludur. Ürünleri geçici olarak bellekte tutar.

#### Fonksiyonlar:
- `AddProduct(product)` - Sepete ürün ekler
- `UpdateQuantity(name, qty)` - Adedi değiştirir
- `GetTotal()` - Toplam tutarı hesaplar
- `ClearCart()` - Sepeti temizler

---

## 🗃 Veritabanı Tabloları

### `product` tablosu
| Alan | Tip | Açıklama |
|------|-----|----------|
| id | INTEGER | Otomatik ID |
| barcode | TEXT | Barkod numarası |
| name | TEXT | Ürün adı |
| price | NUMERIC | Birim fiyat |
| stock | INTEGER | Stok miktarı |

### `sales` tablosu
| Alan | Tip | Açıklama |
|------|-----|----------|
| id | INTEGER | Satış ID |
| total_price | NUMERIC | Satış toplam tutarı |

### `sale_items` tablosu
| Alan | Tip | Açıklama |
|------|-----|----------|
| id | INTEGER | Otomatik ID |
| sale_id | INTEGER | `sales.id` dış anahtar |
| barcode | TEXT | Ürünün barkodu |
| quantity | INTEGER | Satılan adet |
| price | NUMERIC | Birim fiyat |

---

## 🔗 Katmanlar Arası Etkileşim

```
MainForm (UI)  <---->  CartService (Sepet)  <---->  DatabaseService (Veritabanı)
        |                                          ↑
        |____________________ kullanıcı etkileşimi |
```

- UI katmanı, `DatabaseService` üzerinden ürün ve satış verisini alır/gönderir.
- `CartService`, UI ile birlikte geçici sepeti yönetir.
- `btnCompleteSale_Click`, bu 3 katmanın iş birliğiyle stok düşer, satış kaydedilir.

---

## 📌 Notlar ve İyileştirme Önerileri

- `sales` tablosuna tarih-saat sütunu eklenebilir.
- Satış geçmişini gösteren sayfa (raporlama) geliştirilebilir.
- Yazıcıdan fiş alma veya PDF çıktısı entegrasyonu yapılabilir.
- Giriş ekranı / kullanıcı yetkilendirme modülü eklenebilir.

---

Bu dokümantasyon, sistemin mevcut tüm işlevlerini kapsamaktadır. Yeni geliştirme adımlarında sürekli güncellenebilir.