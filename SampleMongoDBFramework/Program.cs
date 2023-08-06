using MongoDB.Driver;
using MongoFramework;
using SampleMongoDBFramework.DataAccess;
using SampleMongoDBFramework.Repository;
using SampleMongoDBFramework.Repository.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add MongoDB Connection and transient
builder.Services.AddTransient<IMongoDbConnection>(sp => MongoDbConnection.FromConnectionString(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<DbContext>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Interface and implementation
builder.Services.AddScoped<IDbRepository, DbRepository>();

// Add Interface and Implementation

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