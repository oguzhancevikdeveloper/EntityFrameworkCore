

using EntityFrameworkCore.Models;
 MasterContext _context = new();

foreach (var category in _context.Categories)
{
    Console.WriteLine(category.CategoryName+"  :  "+category.Description);
}
Console.ReadLine();