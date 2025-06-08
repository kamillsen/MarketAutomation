using System.Windows.Forms;

namespace BarcodeMarketApp.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        // UI bileşenleri
        private TextBox txtBarcode;
        private ListView lvCart;
        private Label lblTotal;
        private Button btnShowProducts;
        private Button btnCompleteSale;

        // Bellek temizliği
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        // Form üzerindeki tüm bileşenleri oluşturan method
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // 🔹 Barkod giriş kutusu
            this.txtBarcode = new TextBox
            {
                Location = new System.Drawing.Point(30, 30),
                Name = "txtBarcode",
                PlaceholderText = "Barkod okutun veya yazın...",
                Size = new System.Drawing.Size(250, 23),
                TabIndex = 0
            };
            this.txtBarcode.KeyDown += new KeyEventHandler(this.txtBarcode_KeyDown);

            // 🔹 Sepet/ürün ListView
            this.lvCart = new ListView
            {
                Location = new System.Drawing.Point(30, 70),
                Name = "lvCart",
                Size = new System.Drawing.Size(620, 250),
                TabIndex = 1,
                UseCompatibleStateImageBehavior = false,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true
            };
            this.lvCart.Columns.Add("Ürün Adı", 250);
            this.lvCart.Columns.Add("Adet", 80);
            this.lvCart.Columns.Add("Birim Fiyat", 120);
            this.lvCart.Columns.Add("Toplam", 120);
            this.lvCart.DoubleClick += new System.EventHandler(this.lvCart_DoubleClick);

            // 🔹 Toplam tutarı gösteren label
            this.lblTotal = new Label
            {
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(30, 340),
                Name = "lblTotal",
                Size = new System.Drawing.Size(130, 21),
                TabIndex = 2,
                Text = "Toplam: ₺0,00"
            };

            // 🔹 Tüm ürünleri gösteren buton
            this.btnShowProducts = new Button
            {
                Location = new System.Drawing.Point(300, 30),
                Name = "btnShowProducts",
                Size = new System.Drawing.Size(150, 23),
                TabIndex = 3,
                Text = "Tüm Ürünleri Göster",
                UseVisualStyleBackColor = true
            };
            this.btnShowProducts.Click += new System.EventHandler(this.btnShowProducts_Click);

            // 🔹 Satışı tamamla butonu
            this.btnCompleteSale = new Button
            {
                Location = new System.Drawing.Point(470, 30),
                Name = "btnCompleteSale",
                Size = new System.Drawing.Size(150, 23),
                TabIndex = 4,
                Text = "Satışı Tamamla",
                UseVisualStyleBackColor = true
            };
            this.btnCompleteSale.Click += new System.EventHandler(this.btnCompleteSale_Click);

            // 🔹 Form ayarları
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 400);
            this.Controls.Add(this.txtBarcode);
            this.Controls.Add(this.lvCart);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnShowProducts);
            this.Controls.Add(this.btnCompleteSale); // ❗️ BU SATIR doğru konumda olmalı!
            this.Name = "MainForm";
            this.Text = "Barkod Satış Sistemi";

            // ❗️ Her şey eklendikten sonra layout'u sonlandır
            this.ResumeLayout(false);
            this.PerformLayout();
        }

    }
}
