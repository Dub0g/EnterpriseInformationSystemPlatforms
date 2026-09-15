using Microsoft.EntityFrameworkCore;
using PhotoStudioApp.Data;

using var context = new PhotoStudioContext();

Console.WriteLine("Підключення до бази даних через Entity Framework Core...\n");

var query =
    from od in context.OrderDetails
    join o in context.Orders on od.OrderID equals o.ID
    join c in context.Clients on o.ClientID equals c.ID
    join s in context.Services on od.ServiceID equals s.ID
    orderby o.ID
    select new
    {
        o.ID,
        c.LastName,
        c.FirstName,
        ServiceName = s.Name,
        od.Quantity,
        od.UnitPrice,
        o.Date,
        o.DeliveryDate
    };

var results = query.AsNoTracking().ToList();

Console.WriteLine(
    $"{"OrderID",-8}{"Прізвище",-15}{"Ім'я",-12}{"Послуга",-25}{"К-сть",-7}{"Ціна",-10}{"Дата прийому",-15}{"Дата видачі",-15}");
Console.WriteLine(new string('-', 110));

foreach (var row in results)
{
    Console.WriteLine(
        $"{row.ID,-8}" +
        $"{row.LastName,-15}" +
        $"{row.FirstName,-12}" +
        $"{row.ServiceName,-25}" +
        $"{row.Quantity,-7}" +
        $"{row.UnitPrice,-10}" +
        $"{row.Date.ToShortDateString(),-15}" +
        $"{row.DeliveryDate.ToShortDateString(),-15}"
    );
}

Console.WriteLine("\nНатисніть будь-яку клавішу для виходу...");
Console.ReadKey();