using TravelInspiration.API;
using TravelInspiration.API.Features.Destinations;
using TravelInspiration.API.Features.Itineraries;
using TravelInspiration.API.Features.Stops;
using TravelInspiration.API.Shared.Slices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddProblemDetails(); // Add handling for problem details RFC spec
builder.Services.AddHttpClient();
   
builder.Services.RegisterApplicationServices();
builder.Services.RegisterPersistenceServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler();
}
app.UseStatusCodePages();

app.MapSliceEndpoints();

app.Run();