using Test_Scanner.IServices;
using Test_Scanner.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<PortDetector>();

builder.Services.AddScoped<IPassportReaderService, PassportReaderService>();
builder.Services.AddSingleton<IScannerService, ScannerService>();
builder.Services.AddSingleton<ICardDispenserService, K100CardDispenserService>();



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(builder => builder
.AllowAnyHeader()
.AllowAnyMethod()
.SetIsOriginAllowed(_ => true)
.AllowCredentials()
);
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
