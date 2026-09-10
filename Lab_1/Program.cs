using Microsoft.Data.SqlClient;

string connectionString = "Server=localhost\\SQLEXPRESS01;Database=Photo studio;Trusted_Connection=True;TrustServerCertificate=True;";

string query = @"
    SELECT o.ID, c.LastName, c.FirstName, s.Name, 
           od.Quantity, od.UnitPrice, o.Date, o.DeliveryDate
    FROM OrderDetails od
    JOIN Orders o ON od.OrderID = o.ID
    JOIN Clients c ON o.ClientID = c.ID
    JOIN Services s ON od.ServiceID = s.ID
    ORDER BY o.ID;";

using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    Console.WriteLine("Підключення до бази даних успішне!\n");

    using (SqlCommand command = new SqlCommand(query, connection))
    using (SqlDataReader reader = command.ExecuteReader())
    {
        Console.WriteLine($"{"OrderID",-8}{"Прізвище",-15}{"Ім'я",-12}{"Послуга",-25}{"К-сть",-7}{"Ціна",-10}{"Дата прийому",-15}{"Дата видачі",-15}");
        Console.WriteLine(new string('-', 110));

        while (reader.Read())
        {
            Console.WriteLine(
                $"{reader["ID"],-8}" +
                $"{reader["LastName"],-15}" +
                $"{reader["FirstName"],-12}" +
                $"{reader["Name"],-25}" +
                $"{reader["Quantity"],-7}" +
                $"{reader["UnitPrice"],-10}" +
                $"{Convert.ToDateTime(reader["Date"]).ToShortDateString(),-15}" +
                $"{Convert.ToDateTime(reader["DeliveryDate"]).ToShortDateString(),-15}"
            );
        }
    }
}

Console.WriteLine("\nНатисніть будь-яку клавішу для виходу...");
Console.ReadKey();

