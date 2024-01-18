using hamed.sylla.FeatureMatching;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ObjectDetection>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Disable for macos
//app.UseHttpsRedirection();

app.MapPost("/FeatureMatching", async ([FromForm] IFormFileCollection files) => {
    if (files.Count != 2)
        return Results.BadRequest();
    
    using var objectSourceStream = files[0].OpenReadStream(); using var objectMemoryStream = new MemoryStream(); objectSourceStream.CopyTo(objectMemoryStream);
    var imageObjectData = objectMemoryStream.ToArray();
    using var sceneSourceStream = files[1].OpenReadStream(); using var sceneMemoryStream = new MemoryStream(); sceneSourceStream.CopyTo(sceneMemoryStream);
    var imageSceneData = sceneMemoryStream.ToArray();
    
    var result = await new ObjectDetection().DetectObjectInScenesAsync(imageObjectData, new List<byte[]> { imageSceneData });
    
    return Results.File(result[0].ImageData, "image/png"); }
    
).DisableAntiforgery();

app.Run();
