using EntityFrameworkCore.Models;
using Microsoft.EntityFrameworkCore;
Console.WriteLine();

#region Veri Nasıl Silinir?
MasterContext context = new();
Product product = await context.Products.FirstOrDefaultAsync(u => u.ProductId == 5);
context.Products.Remove(product);
await context.SaveChangesAsync();
#endregion

#region Silme İşleminde ChangeTracker'ın Rolü
//ChangeTracker, context üzerinden gelen verilerin takibinden sorumlu bir mekanizmadır. Bu takip mekanizması sayesinde context üzerinden gelen verilerle ilgili işlemler neticesinde update yahut delete sorgularının oluşturulacağı anlaşılır!
#endregion
#region Takip Edilmeyen Nesneler Nasıl Silinir?
MasterContext context2 = new();
Product product2 = new()
{
    ProductId = 2
};
context.Products.Remove(product2);
await context.SaveChangesAsync();

#region EntityState İle Silme İşlemi
Product product3 = new() { ProductId = 1 };
context.Entry(product3).State = EntityState.Deleted;
await context.SaveChangesAsync();
#endregion
#endregion
#region Birden Fazla Veri Silinirken Nelere Dikkat Edilmelidir?
#region SaveChanges'ı Verimli Kullanalım

#endregion
#region RemoveRange
MasterContext context3 = new();
List<Product> products = await context.Products.Where(u => u.ProductId >= 7 && u.ProductId <= 9).ToListAsync();
context.Products.RemoveRange(products);
await context.SaveChangesAsync();
#endregion
#endregion
