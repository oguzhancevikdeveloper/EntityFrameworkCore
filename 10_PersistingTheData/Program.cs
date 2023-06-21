using EntityFrameworkCore.Models;

#region Veri Nasıl Eklenir?
MasterContext _context = new();

Product product = new()
{
    ProductName = "Oziii",
    SupplierId = 1,
    CategoryId = 1,
    QuantityPerUnit = "12 - 550 ml bottles",
    UnitPrice = 18,
    UnitsInStock = 39,
    ReorderLevel = 25,
    Discontinued = false,
};

#region context.AddAsync Fonksiyonu
await _context.AddAsync(product);
#endregion
#region context.DbSet.AddAsync Fonksiyonu
await _context.Products.AddAsync(product);
#endregion
//await _context.SaveChangesAsync();
#endregion

#region SaveChanges Nedir?
//SaveChanges; insert, update ve delete sorgularını oluşturup bir transaction eşliğinde veritabanına gönderip execute eden fonksiyodur. Eğer ki oluşturulan sorgulardan herhangi biri başarısız olursa tüm işlemleri geri alır(rollback)
#endregion

#region EF Core Açısından Bir Verinin Eklenmesi Gerektiği Nasıl Anlaşılıyor?
MasterContext _context1 = new();

Product product_1 = new()
{
    ProductName = "Oziii",
    SupplierId = 1,
    CategoryId = 1,
    QuantityPerUnit = "12 - 550 ml bottles",
    UnitPrice = 18,
    UnitsInStock = 39,
    ReorderLevel = 25,
    Discontinued = false,
};

Console.WriteLine(_context1.Entry(product_1).State);

await _context1.AddAsync(product_1);

Console.WriteLine(_context1.Entry(product_1).State);

await _context1.SaveChangesAsync();

Console.WriteLine(_context1.Entry(product_1).State);

#endregion

#region Birden Fazla Veri Eklerken Nelere Dikkat Edilmelidir?
#region SaveChanges'ı Verimli Kullanalım!
//SaveChanges fonksiyonu her tetiklendiğinde bir transaction oluşituracağından dolayı EF Core ile yapılan her bir işleme özel kullanmaktan kaçınmalıyız! Çünkü her işleme özel transaction veritabanı açısından ekstradan maliyet demektir. O yüzden mümkün mertebe tüm işlemlerimizi tek bir transaction eşliğinde veritabanına gönderebilmek için savechanges'ı aşağıdaki gibi tek seferde kullanmak hem maliyet hem de yönetilebilirlik açısından katkıda bulunmuş olacaktır.

MasterContext _context2 = new();

Product product_2 = new()
{
    ProductName = "Oziii",
    SupplierId = 1,
    CategoryId = 1,
    QuantityPerUnit = "12 - 550 ml bottles",
    UnitPrice = 18,
    UnitsInStock = 39,
    ReorderLevel = 25,
    Discontinued = false,
};

Product product_3 = new()
{
    ProductName = "Oziii",
    SupplierId = 1,
    CategoryId = 1,
    QuantityPerUnit = "12 - 550 ml bottles",
    UnitPrice = 18,
    UnitsInStock = 39,
    ReorderLevel = 25,
    Discontinued = false,
};

Product product_4 = new()
{
    ProductName = "Oziii",
    SupplierId = 1,
    CategoryId = 1,
    QuantityPerUnit = "12 - 550 ml bottles",
    UnitPrice = 18,
    UnitsInStock = 39,
    ReorderLevel = 25,
    Discontinued = false,
};

await _context2.AddAsync(product_2);

await _context2.AddAsync(product_3);

await _context2.AddAsync(product_4);

await _context2.SaveChangesAsync();
#endregion
#region AddRange
MasterContext _context3 = new();

Product product_5 = new()
{
    ProductName = "Oziii",
    SupplierId = 1,
    CategoryId = 1,
    QuantityPerUnit = "12 - 550 ml bottles",
    UnitPrice = 18,
    UnitsInStock = 39,
    ReorderLevel = 25,
    Discontinued = false,
};

Product product_6 = new()
{
    ProductName = "Oziii",
    SupplierId = 1,
    CategoryId = 1,
    QuantityPerUnit = "12 - 550 ml bottles",
    UnitPrice = 18,
    UnitsInStock = 39,
    ReorderLevel = 25,
    Discontinued = false,
};

Product product_7 = new()
{
    ProductName = "Oziii",
    SupplierId = 1,
    CategoryId = 1,
    QuantityPerUnit = "12 - 550 ml bottles",
    UnitPrice = 18,
    UnitsInStock = 39,
    ReorderLevel = 25,
    Discontinued = false,
};

await _context3.Products.AddRangeAsync(product_5, product_6, product_7);
await _context3.SaveChangesAsync();
#endregion
#endregion
