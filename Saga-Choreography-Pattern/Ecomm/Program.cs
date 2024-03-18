using CoreLibrary.RabbitMQ;
using Ecomm;
using Ecomm.DataAccess;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration["ConnectionString"];
builder.Services.AddSingleton<IOrderDetailsProvider, OrderDetailsProvider>();
builder.Services.AddSingleton<IInventoryProvider>(new InventoryProvider(connectionString));
builder.Services.AddSingleton<IProductProvider>(new ProductProvider(connectionString));
builder.Services.AddSingleton<IInventoryUpdator>(new InventoryUpdator(connectionString));

builder.Services.AddHttpClient("order", config =>
                config.BaseAddress = new Uri(builder.Configuration["OrderServiceAddress"]));

builder.Services.AddSingleton<IConnectionProvider>(new ConnectionProvider(builder.Configuration["RabbitMQUri"]));
builder.Services.AddSingleton<IPublisher>(x => new Publisher(x.GetService<IConnectionProvider>(),
        "inventory_exchange",
        ExchangeType.Topic));

builder.Services.AddSingleton<ISubscriber>(x => new Subscriber(x.GetService<IConnectionProvider>(),
        "order_exchange",
        "order_response",
        "order.created",
        ExchangeType.Topic));

builder.Services.AddHostedService<OrderCreatedListener>();

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
