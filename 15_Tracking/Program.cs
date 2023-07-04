using EntityFrameworkCore.Models;
using Microsoft.EntityFrameworkCore;

MasterContext context = new();

#region AsNoTracking Metodu
//Context üzerinden gelen tüm datalar Change Tracker mekanizması tarafından takip edilmektedir.

//Change Tracker, takip ettiği nesnelerin sayısıyla doğru orantılı olacak şekilde bir maliyete sahiptir. O yüzden üzerinde işlem yapılmayacak verilerin takip edilmesi bizlere lüzumsuz yere bir maliyet ortaya çıkaracaktır.

//AsNoTracking metodu, context üzerinden sorgu neticesinde gelecek olan verilerin Change Tracker tarafından takip edilmesini engeller.

//AsNoTracking metodu ile Change Tracker'ın ihtiyaç olmayan verilerdeki maliyetini törpülemiş oluruz.

//AsNoTracking fonksiyonu ile yapılan sorgulamalarda, verileri elde edebilir, bu verileri istenilen noktalarda kullanabilir lakin veriler üzerinde herhangi bir değişiklik/update işlemi yapamayız.

var products = await context.Products.AsNoTracking().ToListAsync(); // IQueryable iken asnotracking yapacaksın yani tolistten önce.
foreach (var product in products)
{
    Console.WriteLine(product.ProductName);
    product.ProductName = $"yeni-{product.ProductName}";
    //context.Products.Update(product); // burada ama güncelleme yapar çünkü context üzerinden çekildiği için arka planda ıd eşleştimesi yapıp güncelleme sağlar.
}
await context.SaveChangesAsync();
#endregion

#region AsNoTrackingWithIdentityResolution
//CT(Change Tracker) mekanizması yinelenen verileri tekil instance olarak getirir. Buradan ekstradan bir performans kazancı söz konusudur.

//Bizler yaptığımız sorgularda takip mekanizmasının AsNoTracking metodu ile maliyetini kırmak isterken bazen maliyete sebebiyet verebiliriz.(Özellikle ilişkisel tabloları sorgularken bu duruma dikkat etmemiz gerekyior)

//AsNoTracking ile elde edilen veriler takip edilmeyeceğinden dolayı yinelenen verilerin ayrı instancelarda olmasına sebebiyet veriyoruz. Çünkü CT mekanizması takip ettiği nesneden bellekte varsa eğer aynı nesneden birdaha oluşturma gereği duymaksızın o nesneye ayrı noktalardaki ihtiyacı aynı instance üzerinden gidermektedir.

//Böyle bir durumda hem takip mekanizmasının maliyeitni ortadan kaldırmak hemide yinelenen dataları tek bir instance üzerinde karşılamak için AsNoTrackingWithIdentityResolution fonksiyonunu kullanabiliriz.

var products2 = await context.Products.Include(k => k.Category).AsNoTrackingWithIdentityResolution().ToListAsync();
var t = 0;

//AsNoTrackingWithIdentityResolution fonksiyonu AsNoTracking fonksiyonuna nazaran görece yavaştır/maliyetlidir lakin CT'a nazaran daha performanslı ve az maliyetlidir.

#endregion

#region AsTracking
//Context üzerinden gelen dataların CT tarafından takip ewdilmesini iradeli bir şekilde ifade etmemizi sağlayan fonksiyondur.
//Peki hoca niye kullanalım ?
//Bir sonraki inceleyeceğimiz UseQueryTrackingBehavior metodunun davranışı gereği uygulama seviyesinde CT'ın default olarak devrede olup olmamasını ayarlıyor olacağız. Eğer ki default olarak pasif hale getirilirse böyle durumda takip mekanizmasının ihtiyaç olduğu sorgularda AsTracking fonksiyonunu kullanabilir ve böylece takip mekanizmasını iradeli bir şekilde devreye sokmuş oluruz.
var products3 = await context.Products.AsTracking().ToListAsync();
#endregion

#region UseQueryTrackingBehavior
//EF Core seviyesinde/uygulama seviyesinde ilgili contextten gelen verilerin üzerinde CT mekanizmasının davranışı temel seviyede belirlememizi sağlayan fonksiyondur. Yani konfigürasyon fonksiyonudur.
#endregion

