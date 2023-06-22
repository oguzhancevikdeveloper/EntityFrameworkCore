using EntityFrameworkCore.Models;
using Microsoft.EntityFrameworkCore;


MasterContext context = new();

#region En Temel Basit Bir Sorgulama Nasıl Yapılır?
#region Method Syntax
var products = await context.Products.ToListAsync();
#endregion
#region Query Syntax
var products2 = await (from urun7 in context.Products
                       select urun7).ToListAsync();
#endregion
#endregion

#region Sorguyu Execute Etmek İçin Ne Yapmamız Gerekmektedir?
#region ToListAsync 
#region Method Syntax
var products3 = await context.Products.ToListAsync();
#endregion
#region Query Syntax
var products4 = await (from urun2 in context.Products
                       select urun2).ToListAsync();
#endregion






#endregion

int urunId = 5;
string urunAdi = "2";

var products5 = from urun3 in context.Products
                where urun3.ProductId > urunId && urun3.ProductName.Contains(urunAdi)
                select urun3;

urunId = 200;
urunAdi = "4";

foreach (Product urun4 in products5)
{
    Console.WriteLine(urun4.ProductName);
}

await products5.ToListAsync();

#region Foreach

foreach (Product product in products5)
{
    Console.WriteLine(product.ProductName);
}

#region Deferred Execution(Ertelenmiş Çalışma)
//IQueryable çalışmalarında ilgili kod yazıldığı noktada tetiklenmez/çalıştırılmaz yani ilgili kod yazıldığı noktada sorguyu generate etmez! Nerede eder? Çalıştırıldığı/execute edildiği noktada tetiklenir! İşte bu durumada ertelenmiş çalışma denir!
#endregion
#endregion
#endregion

#region IQueryable ve IEnumerable Nedir? Basit Olarak!

var urunler = await (from product in context.Products
                     select product).ToListAsync();

#region IQueryable
//Sorguya karşılık gelir.
//Ef core üzerinden yapılmış olan sorgunun execute edilmemiş halini ifade eder.
#endregion
#region IEnumerable
//Sorgunun çalıştırılıp/execute edilip verilerin in memorye yüklenmiş halini ifade eder.
#endregion
#endregion

#region Çoğul Veri Getiren Sorgulama Fonksiyonları
#region ToListAsync
//Üretilen sorguyu execute ettirmemizi sağlayan fonksiyondur.

#region Method Syntax
var products6 = context.Products.ToListAsync();
#endregion
#region Query Syntax
var products7 = (from product in context.Products
                 select product).ToListAsync();
var products8 = from product in context.Products
                select product;
Thread.Sleep(3000);
var datas = await products8.ToListAsync();
#endregion
#endregion

#region Where
//Oluşturulan sorguya where şartı eklememizi sağlayan bir fonksiyondur.

#region Method Syntax
var productId = 15;
var products9 = context.Products.FirstOrDefaultAsync(u => u.ProductId.Equals(productId));
Console.WriteLine(products9.Result.ProductName);
productId = 18;
Console.WriteLine(products9.Result.ProductName);

var _products9 = context.Products.Where(u => u.ProductId > 10);
Console.WriteLine(_products9.FirstOrDefault(x => x.ProductId.Equals(productId)).ProductName);
productId = 22;
Console.WriteLine(_products9.FirstOrDefault(x => x.ProductId.Equals(productId)).ProductName);
// EĞER Kİ BURAYI ANLADIYSAN IQUERYABLE İLE IENUMARBLE ARASINDAKİ FARKI ÇOK İYİ ANLADIN BENCE SADECE BU ÖRNEK İLE ANALAŞILIRDI ZATEN. 

var products10 = await context.Products.Where(u => u.ProductName.StartsWith("a")).ToListAsync();
Console.WriteLine();
#endregion
#region Query Syntax
var products11 = from urun5 in context.Products
                 where urun5.ProductId > 500 && urun5.ProductName.EndsWith("A")
                 select urun5;
var data = await products11.ToListAsync();
Console.WriteLine();
#endregion
#endregion

#region OrderBy
//Sorgu üzerinde sıralama yapmamızı sağlayan bir fonksiyondur. (Ascending)

#region Method Syntax
var products12 = context.Products.Where(u => u.ProductId > 500 || u.ProductName.EndsWith("2")).OrderBy(u => u.ProductName);
#endregion
#region Query Syntax
var products13 = from product in context.Products
                 where product.ProductId > 500 || product.ProductName.StartsWith("2")
                 orderby product.ProductName
                 select product;
#endregion

await products12.ToListAsync();
await products13.ToListAsync();
#endregion

#region ThenBy
//OrderBy üzerinde yapılan sıralama işlemini farklı kolonlarada uygulamamızı sağlayan bir fonksiyondur. (Ascending) 
// örnek verecek olursak ProductNameler aynı mesela bu sefer sıralama için unitPrice referans al diyoruz. Biz bunu uzatabilirirz eğer unitPrice aynı ise Id leri referans al gibi.

var products14 = context.Products.Where(u => u.ProductId > 500 || u.ProductName.EndsWith("2")).OrderBy(u => u.ProductName).ThenBy(u => u.UnitPrice).ThenBy(u => u.ProductName);

await products14.ToListAsync();
#endregion

#region OrderByDescending
//Descending olarak sıralama yapmamızı sağlayan bir fonksiyondur.

#region Method Syntax
var products15 = await context.Products.OrderByDescending(u => u.UnitPrice).ToListAsync();
#endregion
#region Query Syntax
var products16 = await (from product in context.Products
                        orderby product.ProductName descending
                        select product).ToListAsync();
#endregion
#endregion

#region ThenByDescending
//OrderByDescending üzerinde yapılan sıralama işlemini farklı kolonlarada uygulamamızı sağlayan bir fonksiyondur. (Ascending)
var products17 = await context.Products.OrderByDescending(u => u.ProductId).ThenByDescending(u => u.UnitPrice).ThenBy(u => u.ProductName).ToListAsync();
#endregion
#endregion

#region Tekil Veri Getiren Sorgulama Fonksiyonları
//Yapılan sorguda sade ve sadece tek bir verinin gelmesi amaçlanıyorsa Single ya da SingleOrDefault fonksiyonları kullanılabilir.
#region SingleAsync
//Eğer ki, sorgu neticesinde birden fazla veri geliyorsa ya da hiç gelmiyorsa her iki durumda da exception fırlatır.
#region Tek Kayıt Geldiğinde
var product18 = await context.Products.SingleAsync(u => u.ProductId == 55);
#endregion
#region Hiç Kayıt Gelmediğinde
var product19 = await context.Products.SingleAsync(u => u.ProductId == 5555);
#endregion
#region Çok Kayıt Geldiğinde
var urun = await context.Products.SingleAsync(u => u.ProductId > 55);
#endregion
#endregion

#region SingleOrDefaultAsync
//Eğer ki, sorgu neticesinde birden fazla veri geliyorsa exception fırlatır, hiç veri gelmiyorsa null döner.
#region Tek Kayıt Geldiğinde
var product20 = await context.Products.SingleOrDefaultAsync(u => u.ProductId == 55);
#endregion
#region Hiç Kayıt Gelmediğinde
var product21 = await context.Products.SingleOrDefaultAsync(u => u.ProductId == 5555);
#endregion
#region Çok Kayıt Geldiğinde
var product22 = await context.Products.SingleOrDefaultAsync(u => u.ProductId > 55);
#endregion
#endregion

//Yapılan sorguda tek bir verinin gelmesi amaçlanıyorsa First ya da FirstOrDefault fonksiyonları kullanılabilir.
#region FirstAsync
//Sorgu neticesinde elde edilen verilerden ilkini getirir. Eğer ki hiç veri gelmiyorsa hata fırlatır.
#region Tek Kayıt Geldiğinde
var product23 = await context.Products.FirstAsync(u => u.ProductId == 55);
#endregion
#region Hiç Kayıt Gelmediğinde
var product24 = await context.Products.FirstAsync(u => u.ProductId == 5555);
#endregion
#region Çok Kayıt Geldiğinde
var product25 = await context.Products.FirstAsync(u => u.ProductId > 55);
#endregion
#endregion

#region FirstOrDefaultAsync
//Sorgu neticesinde elde edilen verilerden ilkini getirir. Eğer ki hiç veri gelmiyorsa null değerini döndürür.
#region Tek Kayıt Geldiğinde
var product26 = await context.Products.FirstOrDefaultAsync(u => u.ProductId == 55);
#endregion
#region Hiç Kayıt Gelmediğinde
var product27 = await context.Products.FirstOrDefaultAsync(u => u.ProductId == 5555);
#endregion
#region Çok Kayıt Geldiğinde
var product28 = await context.Products.FirstAsync(u => u.ProductId > 55);
#endregion
#endregion

#region SingleAsync, SingleOrDefaultAsync, FirstAsync, FirstOrDefaultAsync Karşılaştırması

#endregion

#region FindAsync
//Find fonksiyonu, primary key kolonuna özel hızlı bir şekilde sorgulama yapmamızı sağlayan bir fonksiyondur. 
// Hızlı yapıyor çünkü contexten önce memorye bakıyor.
Product? product29 = await context.Products.FirstOrDefaultAsync(u => u.ProductId == 55);
Product? product32 = await context.Products.FindAsync(keyValues: 55);

#region Composite Primary key Durumu
Product? product33 = await context.Products.FindAsync(4, 2);

#endregion
#endregion

#region FindAsync İle SingleAsync, SingleOrDefaultAsync, FirstAsync, FirstOrDefaultAsync Fonksiyonlarının Karşılaştırması

#endregion

#region LastAsync
//Sorgu neticesinde gelen verilerden en sonuncusunu getirir. Eğer ki hiç veri gelmiyorsa hata fırlatır. OrderBy kullanılması mecburidir.
var product34 = await context.Products.OrderBy(u => u.UnitPrice).LastAsync(u => u.ProductId > 55);
#endregion

#region LastOrDefaultAsync
//Sorgu neticesinde gelen verilerden en sonuncusunu getirir. Eğer ki hiç veri gelmiyorsa null döner. OrderBy kullanılması mecburidir.
var product35 = await context.Products.OrderBy(u => u.UnitPrice).LastOrDefaultAsync(u => u.ProductId > 55);
#endregion
#endregion

#region Diğer Sorgulama Fonksiyonları
#region CountAsync
//Oluşturulan sorgunun execute edilmesi neticesinde kaç adet satırın elde edileceğini sayısal olarak(int) bizlere bildiren fonksiyondur.
var product36 = (await context.Products.ToListAsync()).Count();
var product37 = await context.Products.CountAsync();
#endregion

#region LongCountAsync
//Oluşturulan sorgunun execute edilmesi neticesinde kaç adet satırın elde edileceğini sayısal olarak(long) bizlere bildiren fonksiyondur. Büyük datalar için aslında, temel olarak count ile aynı.
var product38 = await context.Products.LongCountAsync(u => u.UnitPrice > 5000);
#endregion

#region AnyAsync
//Sorgu neticesinde verinin gelip gelmediğini bool türünde dönen fonksiyondur. 
var product39 = await context.Products.Where(u => u.ProductName.Contains("1")).AnyAsync();
var product40 = await context.Products.AnyAsync(u => u.ProductName.Contains("1"));
#endregion

#region MaxAsync
//Verilen kolondaki max değeri getirir.
var product41 = await context.Products.MaxAsync(u => u.UnitPrice);
#endregion

#region MinAsync
//Verilen kolondaki min değeri getirir.
var product42 = await context.Products.MinAsync(u => u.UnitPrice);
#endregion

#region Distinct
//Sorguda mükerrer kayıtlar varsa bunları tekilleştiren bir işleve sahip fonksiyondur.
var product43 = await context.Products.Distinct().ToListAsync();
#endregion

#region AllAsync
//Bir sorgu neticesinde gelen verilerin, verilen şarta uyup uymadığını kontrol etmektedir. Eğer ki tüm veriler şarta uyuyorsa true, uymuyorsa false döndürecektir.
var m = await context.Products.AllAsync(u => u.UnitPrice < 15000);
var m1 = await context.Products.AllAsync(u => u.ProductName.Contains("a"));
#endregion

#region SumAsync
//Vermiş olduğumuz sayısal proeprtynin toplamını alır.
var fiyatToplam = await context.Products.SumAsync(u => u.UnitPrice);
#endregion

#region AverageAsync
//Vermiş olduğumuz sayısal proeprtynin aritmatik ortalamasını alır.
var aritmatikOrtalama = await context.Products.AverageAsync(u => u.UnitPrice);
#endregion

#region Contains
//Like '%...%' sorgusu oluşturmamızı sağlar.
var product44 = await context.Products.Where(u => u.ProductName.Contains("7")).ToListAsync();
#endregion

#region StartsWith
//Like '...%' sorgusu oluşturmamızı sağlar.
var product45 = await context.Products.Where(u => u.ProductName.StartsWith("7")).ToListAsync();
#endregion

#region EndsWith
//Like '%...' sorgusu oluşturmamızı sağlar.
var product46 = await context.Products.Where(u => u.ProductName.EndsWith("7")).ToListAsync();
#endregion
#endregion

#region Sorgu Sonucu Dönüşüm Fonksiyonları
//Bu fonksiyonlar ile sorgu neticesinde elde edilen verileri isteğimiz doğrultusunda farklı türlerde projecsiyon edebiliyoruz.

#region ToDictionaryAsync
//Sorgu neticesinde gelecek olan veriyi bir dictionary olarak elde etmek/tutmak/karşılamak istiyorsak eğer kullanılır!
var product47 = await context.Products.ToDictionaryAsync(u => u.ProductName, u => u.UnitPrice);

//ToList ile aynı amaca hizmet etmektedir. Yani, oluşturulan sorguyu execute edip neticesini alırlar.
//ToList : Gelen sorgu neticesini entity türünde bir koleksiyona(List<TEntity>) dönüştürmekteyken,
//ToDictionary ise : Gelen sorgu neticesini Dictionary türünden bir koleksiyona dönüştürecektir.
#endregion

#region ToArrayAsync
//Oluşturulan sorguyu dizi olarak elde eder.
//ToList ile muadil amaca hizmet eder. Yani sorguyu execute eder lakin gelen sonucu entity dizisi  olarak elde eder.
var product48 = await context.Products.ToArrayAsync();
#endregion

#region Select
//Select fonksiyonunun işlevsel olarak birden fazla davranışı söz konusudur,
//1. Select fonksiyonu, generate edilecek sorgunun çekilecek kolonlarını ayarlamamızı sağlamaktadır. 

var product49 = await context.Products.Select(u => new Product
{
    ProductId = u.ProductId,
    UnitPrice = u.UnitPrice
}).ToListAsync();

//2. Select fonksiyonu, gelen verileri farklı türlerde karşılamamızı sağlar. T, anonim

var product50 = await context.Products.Select(u => new
{
    Id = u.ProductId,
    Fiyat = u.UnitPrice
}).ToListAsync();


var product51 = await context.Products.Select(u => new ProductDetail
{
    Id = u.ProductId,
    Price = Convert.ToDecimal(u.UnitPrice)
}).ToListAsync();

#endregion

#region SelectMany
//Select ile aynı amaca hizmet eder. Lakin, ilişkisel tablolar neticesinde gelen koleksiyonel verileri de tekilleştirip projeksiyon etmemizi sağlar.
// !! BAYAĞI ÖNEMLİ İYİ ÖĞREN İYİ ÖĞREN!!
var product52 = await context.Products.Include(u => u.OrderDetails).SelectMany(u => u.OrderDetails, (u, o) => new
{
    u.ProductId,
    u.UnitPrice,
    o.Discount
}).ToListAsync();

#endregion
#endregion

#region GroupBy Fonksiyonu
//Gruplama yapmamızı sağlayan fonksiyondur.
#region Method Syntax
var product53 = await context.Products.GroupBy(u => u.UnitPrice).Select(group => new
{
    Count = group.Count(),
    Fiyat = group.Key,
}).ToListAsync();
#endregion
#region Query Syntax
var _datas = await (from _product54 in context.Products
                       group _product54 by _product54.UnitPrice
            into @group
                       select new
                       {
                           Fiyat = @group.Key,
                           Count = @group.Count()
                       }).ToListAsync();
#endregion
#endregion

#region Foreach Fonksiyonu
//Bir sorgulama fonksiyonu felan değildir!
//Sorgulama neticesinde elde edilen koleksiyonel veriler üzerinde iterasyonel olarak dönmemizi ve teker teker verileri elde edip işlemler yapabilmemizi sağlayan bir fonksiyondur. foreach döngüsünün metot halidir!

foreach (var item in _datas)
{

}
_datas.ForEach(x =>
{

});
#endregion
public class ProductDetail
{
    public int Id { get; set; }
    public decimal Price { get; set; }
}