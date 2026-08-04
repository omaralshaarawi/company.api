using company.api.Data;
using Microsoft.EntityFrameworkCore;
using company.api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<CompanyContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDepartmentsService, DepartmentsService>();
builder.Services.AddScoped<IAssetTypesService, AssetTypesService>();
builder.Services.AddScoped<IAssetsService, AssetsService>();
builder.Services.AddScoped<IFingerprintsService, FingerprintsService>();
builder.Services.AddScoped<IEmployeeAssetsService, EmployeeAssetsService>();
builder.Services.AddScoped<IAttendanceLogsService, AttendanceLogsService>();
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
