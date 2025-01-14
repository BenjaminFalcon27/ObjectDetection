using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;
using Benjamin.Trolais.ObjectDetection;
using SixLabors.ImageSharp.Processing;

var builder = WebApplication.CreateBuilder(args);

// Ajout des services nécessaires pour Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization(); // Ajout de l'autorisation

var app = builder.Build();

// Activer Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// Route POST pour l'ObjectDetection
app.MapPost("/ObjectDetection", async (IFormFile imageFile) =>
{
    if (imageFile == null || imageFile.Length == 0)
        return Results.BadRequest("No file uploaded");

    // Lire l'image téléchargée
    using var stream = imageFile.OpenReadStream();
    using var image = await Image.LoadAsync(stream);

    // Simuler la détection d'objet
    var detectionService = new ObjectDetection();
    var detectionResults = await detectionService.DetectObjectInScenesAsync(new List<byte[]> { await File.ReadAllBytesAsync(imageFile.FileName) });

    // Dessiner la zone de détection sur l'image
    foreach (var result in detectionResults)
    {
        var box = result.Box.First();
        var points = new PointF[]
        {
            new PointF(box.Dimensions.X, box.Dimensions.Y),
            new PointF(box.Dimensions.X + box.Dimensions.Width, box.Dimensions.Y),
            new PointF(box.Dimensions.X + box.Dimensions.Width, box.Dimensions.Y + box.Dimensions.Height),
            new PointF(box.Dimensions.X, box.Dimensions.Y + box.Dimensions.Height),
        };

        image.Mutate(x => x.DrawPolygon(Color.Red, 3, points));
    }

    // Retourner l'image avec la détection
    var ms = new MemoryStream();
    await image.SaveAsJpegAsync(ms);
    ms.Position = 0;

    return Results.File(ms.ToArray(), "image/jpeg");
});

app.Run();
