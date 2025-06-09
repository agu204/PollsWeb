using backend;
using backend.Models.Database;
using backend.Models.Database.Repositories.Implementations;
using backend.Models.Mappers;
using backend.Services;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// configuracion directorio
Directory.SetCurrentDirectory(AppContext.BaseDirectory);

// Leer la configuración
builder.Services.Configure<Settings>(builder.Configuration.GetSection(Settings.SECTION_NAME));

// Inyectamos el DbContext
builder.Services.AddScoped<PollsContext>();
builder.Services.AddScoped<UnitOfWork>();

// Inyección de todos los repositorios
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<PollRepository>();

// Inyección de Mappers
builder.Services.AddScoped<UserMapper>();
builder.Services.AddScoped<PollMapper>();

// Inyección de Servicios
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PollService>();
builder.Services.AddScoped<VoteService>();


// Add services to the container.

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de CORS
builder.Services.AddCors(options =>
{
        options.AddDefaultPolicy(builder =>
        {
            builder.AllowAnyOrigin() // Permitir cualquier origen
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
    });

    builder.Services.AddControllers();
    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Configuración de autenticacin
builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        Settings settings = builder.Configuration.GetSection(Settings.SECTION_NAME).Get<Settings>();
        string key = settings.JwtKey;

        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Permite CORS
app.UseCors();


string wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
Directory.CreateDirectory(wwwrootPath);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(wwwrootPath)
});


app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

await SeedDataBaseAsync(app.Services);


app.Run();


// metodo para el seeder
static async Task SeedDataBaseAsync(IServiceProvider serviceProvider)
{
    using IServiceScope scope = serviceProvider.CreateScope();
    using PollsContext dbContext = scope.ServiceProvider.GetService<PollsContext>();

    // Si no existe la base de datos, la creamos y ejecutamos el seeder
    if (dbContext.Database.EnsureCreated())
    {
        Seeder seeder = new Seeder(dbContext);
        await seeder.SeedAsync();
    }
}