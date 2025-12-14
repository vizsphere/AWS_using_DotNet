using Speakers.Models;

var builder = WebApplication.CreateBuilder(args);

IAppSettings appSettings = new AppSettings();
builder.Configuration.GetSection("AppSettings").Bind(appSettings);

builder.Services.AddControllers();
// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);


builder.Services.AddSingleton<IAppSettings>(appSettings);
builder.Services.AddSingleton<SpeakerDataService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});

app.MapGet("/", () => "Welcome to running ASP.NET Core Speakers API(s) on AWS Lambda");

app.Run();
