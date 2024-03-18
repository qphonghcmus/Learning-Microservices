using CoreLibrary.RabbitMQ;
using Ecomm.DataAccess;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IOrderDetailsProvider, OrderDetailsProvider>();
builder.Services.AddSingleton<IInventoryProvider>(new InventoryProvider(builder.Configuration["ConnectionString"]));
builder.Services.AddSingleton<IProductProvider>(new ProductProvider(builder.Configuration["ConnectionString"]));
builder.Services.AddHttpClient("order", config =>
                config.BaseAddress = new Uri(builder.Configuration["OrderServiceAddress"]));

builder.Services.AddSingleton<IConnectionProvider>(new ConnectionProvider(builder.Configuration["RabbitMQUri"]));
builder.Services.AddScoped<IPublisher>(x => new Publisher(x.GetService<IConnectionProvider>(),
        "report_exchange",
        ExchangeType.Topic));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
