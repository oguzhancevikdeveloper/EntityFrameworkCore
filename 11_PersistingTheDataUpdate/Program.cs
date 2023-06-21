#region Veri Nasıl Güncellenir?
using EntityFrameworkCore.Models;
using Microsoft.EntityFrameworkCore;

MasterContext _context = new();

Product? product = await _context.Products.FirstOrDefaultAsync(u => u.ProductId == 3);
product.ProductName = "H Ürünü";
product.UnitPrice = 999;

//_context.Products.Update(product); Bu olsa da olur olmasa ef core kendisi anlıyor.

await _context.SaveChangesAsync();
#endregion
#region ChangeTracker Nedir? Kısaca!
//ChangeTracker, context üzerinden gelen verilerin takibinden sorumlu bir mekanizmadır. Bu takip mekanizması sayesinde context üzerinden gelen verilerle ilgili işlemler neticesinde update yahut delete sorgularının oluşturulacağı anlaşılır!
#endregion
#region Takip Edilmeyen Nesneler Nasıl Güncellenir?
MasterContext _context1 = new();
Product product1 = new()
{
    ProductId = 3,
    ProductName = "Yeni Ürün",
    UnitPrice = 123
};

#region Update Fonksiyonu
//ChangeTracker mekanizması tarafından takip edilmeyen nesnelerin güncellenebilmesi için Update fonksiyonu kullanılır!
//Update fonksiyonunu kullanabilmek için kesinlikle ilgili nesnede Id değeri verilmelidir! Bu değer güncellenecek(update sorgusu oluşturulacak) verinin hangisi olduğunu ifade edecektir.
_context1.Products.Update(product1);
await _context1.SaveChangesAsync();
#endregion
#endregion

#region EntityState Nedir?
//Bir entity instance'ının durumunu ifade eden bir referanstır.
MasterContext _context2 = new();
Product product2 = new();
Console.WriteLine(_context2.Entry(product2).State);
#endregion

#region EF Core Açısından Bir Verinin Güncellenmesi Gerektiği Nasıl Anlaşılıyor?
MasterContext _context3 = new();
Product product3 = await _context3.Products.FirstOrDefaultAsync(u => u.ProductId == 3);
Console.WriteLine(_context3.Entry(product3).State);

product3.ProductName = "Hilmi";

Console.WriteLine(_context3.Entry(product3).State);

await _context3.SaveChangesAsync();

Console.WriteLine(_context3.Entry(product3).State);

product3.UnitPrice = 999;

Console.WriteLine(_context3.Entry(product3).State);
#endregion
#region Birden Fazla Veri Güncellenirken Nelere Dikkat Edilmelidir?
MasterContext _context4 = new();

var products = await _context4.Products.ToListAsync();
foreach (var item in products)
{
    item.ProductName += "*";
}

await _context4.SaveChangesAsync();  // !! savechanges foreach dışında kullanmak kritik.
#endregion