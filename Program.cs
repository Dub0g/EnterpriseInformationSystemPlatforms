using Lab_6.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<Client>("Clients");
modelBuilder.EntitySet<Services>("Services");
modelBuilder.EntitySet<Orders>("Orders");

modelBuilder.EntitySet<OrderDetails>("OrderDetails");

builder.Services.AddControllers().AddOData(
    options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100)
                      .AddRouteComponents("odata", modelBuilder.GetEdmModel())
);

builder.Services.AddOpenApi();

var app = builder.Build();


app.UseDefaultFiles(); 
app.UseStaticFiles();  



    if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();