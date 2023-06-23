using EntityFrameworkCore.Models;
using Microsoft.EntityFrameworkCore;

MasterContext context = new();
#region Change Tracking Neydi?
//Context nesnesi üzerinden gelen tüm nesneler/veriler otomatik olarak bir takip mekanizması tarafından izlenirler. İşte bu takip mekanizmasına Change Tracker denir. Change Traker ile nesneler üzerindeki değişiklikler/işlemler takip edilerek netice itibariyle bu işlemlerin fıtratına uygun sql sorgucukları generate edilir. İşte bu işleme de Change Tracking denir. 
#endregion

#region ChangeTracker Propertysi
//Takip edilen nesnelere erişebilmemizi sağlayan ve gerektiği taktirde işlemler gerçekşetirmemizi sağlayan bir propertydir.
//Context sınıfının base class'ı olan DbContext sınıfının bir member'ıdır.

var products = await context.Products.ToListAsync();


products[6].UnitPrice = 123; //Update
//context.Products.Remove(products[7]); //Delete
products[8].ProductName = "asdasd"; //Update


var datas = context.ChangeTracker.Entries(); // Yani burada analatılmak istenen context ile hangi datalar muhattap olmuş ve günün sonunda neler değişmiş bu muhattap oluanna datalardan.

await context.SaveChangesAsync();
Console.WriteLine();
#endregion

#region DetectChanges Metodu
//EF Core, context nesnesi tarafından izlenen tüm nesnelerdeki değişiklikleri Change Tracker sayesinde takip edebilmekte ve nesnelerde olan verisel değişiklikler yakalanarak bunların anlık görüntüleri(snapshot)'ini oluşturabilir.
//Yapılan değişikliklerin veritabanına gönderilmeden önce algılandığından emin olmak gerekir. SaveChanges fonksiyonu çağrıldığı anda nesneler EF Core tarafından otomatik kontrol edilirler.
//Ancak, yapılan operasyonlarda güncel tracking verilerinden emin olabilmek için değişişiklerin algıulanmasını opsiyonel olarak gerçekleştirmek isteyebiliriz. İşte bunun için DetectChanges fonksiyonu kullanılabilir ve her ne kadar EF Core değişikleri otomatik algılıyor olsa da siz yine de iradenizle kontrole zorlayabilirsiniz.

var product1 = await context.Products.FirstOrDefaultAsync(u => u.ProductId == 3);
product1.UnitPrice = 123;

context.ChangeTracker.DetectChanges();
await context.SaveChangesAsync(); // Aslında savechanges changeTarcker.DetectChanges arka planda çalıştırıyor ama bazen öyle duurmlar oluyor ki async yapılardan veya dağıtık sistemlerden kaynaklı bu duurm algılanamayabiliyor bu yüzden context.ChangeTracker.DetectChanges(); bunu kullanrak garantiye alıyoruz. Baktın db ye bişeyler kayıt olmuyor savechanges çağırmana rağmen bu şekilde müdahale edip yönetebilrisin. Ama bu durum şu anda gereksiz ama böyle daha hassas yapılarda kullanılır.

#endregion

#region AutoDetectChangesEnabled Property'si
//İlgili metotlar(SaveChanges, Entries) tarafından DetectChanges metodunun otomatik olarak tetiklenmesinin konfigürasyonunu yapmamızı sağlayan proeportydir.
//SaveChanges fonksiyonu tetiklendiğinde DetectChanges metodunu içerisinde default olarak çağırmaktadır. Bu durumda DetectChanges fonksiyonunun kullanımını irademizle yönetmek ve maliyet/performans optimizasyonu yapmak istediğimiz durumlarda AutoDetectChangesEnabled özelliğini kapatabiliriz.

// context.ChangeTracker.AutoDetectChangesEnabled = false;
#endregion

#region Entries Metodu
//Context'te ki Entry metodunun koleksiyonel versiyonudur.
//Change Tracker mekanizması tarafından izlenen her entity nesnesinin bigisini EntityEntry türünden elde etmemizi sağlar ve belirli işlemler yapabilmemize olanak tanır.
//Entries metodu, DetectChanges metodunu tetikler. Bu durum da tıpkı SaveChanges'da olduğu gibi bir maliyettir. Buradaki maliyetten kaçınmak için AuthoDetectChangesEnabled özelliğine false değeri verilebilir.

var product2 = await context.Products.ToListAsync();
product2.FirstOrDefault(u => u.ProductId == 7).UnitPrice = 123; //Update
//context.Products.Remove(product2.FirstOrDefault(u => u.ProductId == 8)); //Delete
product2.FirstOrDefault(u => u.ProductId == 9).ProductName = "asdasd"; //Update

context.ChangeTracker.Entries().ToList().ForEach(e =>
{

    if (e.State == EntityState.Unchanged)
    {
        //:..
    }
    else if (e.State == EntityState.Deleted)
    {
        //...
    }
    //...
});
#endregion

#region AcceptAllChanges Metodu
//SaveChanges() veya SaveChanges(true) tetiklendiğinde EF Core herşeyin yolunda olduğunu varsayarak track ettiği verilerin takibini keser yeni değişikliklerin takip edilmesini bekler. Böyle bir durumda beklenmeyen bir durum/olası bir hata söz konusu olursa eğer EF Core takip ettiği nesneleri brakacağı için bir düzeltme mevzu bahis olamayacaktır.

//Haliyle bu durumda devreye SaveChanges(false) ve AcceptAllChanges metotları girecektir.

//SaveChanges(False), EF Core'a gerekli veritabanı komutlarını yürütmesini söyler ancak gerektiğinde yeniden oynatılabilmesi için değişikleri beklemeye/nesneleri takip etmeye devam eder. Taa ki AcceptAllChanges metodunu irademizle çağırana kadar!

//SaveChanges(false) ile işlemin başarılı olduğundan emin olursanız AcceptAllChanges metodu ile nesnelerden takibi kesebilirsiniz.

var product3 = await context.Products.ToListAsync();
product3.FirstOrDefault(u => u.ProductId == 7).UnitPrice = 123; //Update
//context.Products.Remove(product3.FirstOrDefault(u => u.ProductId == 8)); //Delete
product3.FirstOrDefault(u => u.ProductId == 9).ProductName = "asdasd"; //Update

await context.SaveChangesAsync(false);
context.ChangeTracker.AcceptAllChanges();

#endregion

#region HasChanges Metodu
//Takip edilen nesneler arasından değişiklik yapılanların olup olmadığının bilgisini verir.
//Arkaplanda DetectChanges metodunu tetikler.
var result = context.ChangeTracker.HasChanges();

#endregion

#region Entity States
//Entity nesnelerinin durumlarını ifade eder.

#region Detached
//Nesnenin change tracker mekanizması tarafıdnan takip edilmediğini ifade eder.
Product product4 = new();
Console.WriteLine(context.Entry(product4).State);

context.Entry(product4).State = EntityState.Modified;
context.Products.AddAsync(product4);

Console.WriteLine(context.Entry(product4).State);
product4.ProductName = "asdasd";
await context.SaveChangesAsync();
#endregion

#region Added
//Veritabanına eklenecek nesneyi ifade eder. Adeed henüz veritabanına işlenmeyen veriyi ifade eder. SaveChanges fonksiyonu çağrıldığında insert sorgusu oluşturucalşığı anlamını gelir.
Product product5 = new() { UnitPrice = 123, ProductName = "Ürün 1001" };
Console.WriteLine(context.Entry(product5).State); // Detached
await context.Products.AddAsync(product5);
Console.WriteLine(context.Entry(product5).State); // Added
//await context.SaveChangesAsync();
product5.UnitPrice = 321;
Console.WriteLine(context.Entry(product5).State); // Modified
//await context.SaveChangesAsync();
#endregion

#region Unchanged
//Veritabanından sorgulandığından beri nesne üzerinde herhangi bir değişiklik yapılmadığını ifade eder. Sorgu neticesinde elde edilen tüm nesneler başlangıçta bu state değerindedir.
var products6 = await context.Products.ToListAsync();

var data = context.ChangeTracker.Entries();
Console.WriteLine(context.ChangeTracker.Entries().ToString());
Console.WriteLine();
#endregion

#region Modified
//Nesne üzerinde değşiiklik/güncelleme yapıldığını ifade eder. SaveChanges fonksiyonu çağrıldığında update sorgusu oluşturulacağı anlamına gelir.
var products7 = await context.Products.FirstOrDefaultAsync(u => u.ProductId == 3);
Console.WriteLine(context.Entry(products7).State); // Unchanged
products7.ProductName = "asdasdasdasdasd";
Console.WriteLine(context.Entry(products7).State); // Modified
await context.SaveChangesAsync(false);
Console.WriteLine(context.Entry(products7).State);// Unchanged çünkü savechanges(false)

// Eğer ki aynı ismi tekraradan verirsende unchanged olrak algılanır çünkü günün sonunda isim değişmedi.
#endregion

#region Deleted
//Nesnenin silindiğini ifade eder. SaveChanges fonksiyonu çağrıldığında delete sorgusu oluşturuculağı anlamına gelir.
var products8 = await context.Products.FirstOrDefaultAsync(u => u.ProductId == 4);
//context.Products.Remove(products8);
Console.WriteLine(context.Entry(products8).State);
context.SaveChangesAsync();
#endregion
#endregion

#region Context Nesnesi Üzerinden Change Tracker
var products9 = await context.Products.FirstOrDefaultAsync(u => u.ProductId == 5);
products9.UnitPrice = 123;
products9.ProductName = "Silgi"; //Modified | Update

#region Entry Metodu55
#region OriginalValues Property'si
var urunAdi = context.Entry(products9).OriginalValues.GetValue<string>(nameof(Product.ProductName));
var fiyat = context.Entry(products9).OriginalValues.GetValue<float>(nameof(Product.UnitPrice));

Console.WriteLine();
#endregion

#region CurrentValues Property'si
var products10 = context.Entry(products9).CurrentValues.GetValue<string>(nameof(Product.ProductName));
#endregion

#region GetDatabaseValues Metodu
var _urun = await context.Entry(products10).GetDatabaseValuesAsync();
#endregion
#endregion
#endregion

#region Change Tracker'ın Interceptor Olarak Kullanılması

#endregion
