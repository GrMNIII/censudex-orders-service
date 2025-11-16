using OrderService.Data;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// 1. Obtener la cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("MySQLConnection");

// 2. Configurar el DbContext
builder.Services.AddDbContext<OrderServiceDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString), // Detección automática de la versión de MySQL
        mySqlOptions => mySqlOptions.SchemaBehavior(MySqlSchemaBehavior.Ignore) // Previene que EF intente alterar la BD que ya creaste en Workbench
    )
);

builder.Services.AddControllers();