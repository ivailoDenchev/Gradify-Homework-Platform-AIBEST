using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure logging
var logger = LoggerFactory.Create(config =>
{
    config.AddConsole();
}).CreateLogger("Startup");

// Google auth
var credentialPath = Path.Combine(AppContext.BaseDirectory, "Credentials", "GoogleCredentials.json");

if(File.Exists(credentialPath))
{
    var googleCredential = GoogleCredential.FromFile(credentialPath);
    var storageClient = StorageClient.Create(googleCredential);
    builder.Services.AddSingleton(storageClient);
}
else
{
    logger.LogWarning("Google credentials not found, hosting apis will not be available.");
}

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
