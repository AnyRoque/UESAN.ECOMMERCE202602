using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using OllamaSharp;
using UESAN.ECOMMERCE.CORE.Core.Interfaces;
using UESAN.ECOMMERCE.CORE.Core.Services;
using UESAN.ECOMMERCE.CORE.Infrastructure.Data;
using UESAN.ECOMMERCE.CORE.Infrastructure.Repositories;
using UESAN.ECOMMERCE.CORE.Infrastructure.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var _config = builder.Configuration;
var cnx = _config.GetConnectionString("DevConnection");
builder
    .Services
    .AddDbContext<StoreDbContext>(options =>
        options.UseSqlServer(cnx));

builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<UESAN.ECOMMERCE.CORE.Core.Interfaces.IFavoriteRepository, UESAN.ECOMMERCE.CORE.Infrastructure.Repositories.FavoriteRepository>();
builder.Services.AddTransient<UESAN.ECOMMERCE.CORE.Core.Interfaces.IFavoriteService, UESAN.ECOMMERCE.CORE.Core.Services.FavoriteService>();
builder.Services.AddTransient<UESAN.ECOMMERCE.CORE.Core.Interfaces.IOrderRepository, UESAN.ECOMMERCE.CORE.Infrastructure.Repositories.OrderRepository>();
builder.Services.AddTransient<UESAN.ECOMMERCE.CORE.Core.Interfaces.IOrderDetailRepository, UESAN.ECOMMERCE.CORE.Infrastructure.Repositories.OrderDetailRepository>();
builder.Services.AddTransient<UESAN.ECOMMERCE.CORE.Core.Interfaces.IOrderService, UESAN.ECOMMERCE.CORE.Core.Services.OrderService>();

builder.Services.AddSharedInfrastructure(_config);
// IChatClient
IChatClient chatClient = _config["AI:Provider"] == "Gemini"
    ? new Google.GenAI.Client(apiKey: _config["AI:Gemini:ApiKey"]).AsIChatClient(_config["AI:Gemini:Model"])
    : new OllamaApiClient(new Uri(_config["AI:Ollama:Endpoint"]!), _config["AI:Ollama:Model"]!);

builder.Services.AddChatClient(chatClient);


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Registrar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
