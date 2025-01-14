using System;
using System.IO;
using System.Text.Json;
using Benjamin.Trolais.ObjectDetection;

class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Veuillez fournir le chemin du répertoire contenant les images de scène.");
            return;
        }

        // Vérification du chemin d'accès
        string sceneDirectory = args[0];
        
        // Résolution du chemin si nécessaire (pour les chemins relatifs sous Linux)
        // Exemple d'utilisation linux perso:
        // dotnet run '/home/benji/Documents/Cours/DotnetTROLAIS/Benjamin.Trolais.ObjectDetection/Scenes'
        sceneDirectory = Path.GetFullPath(sceneDirectory);

        if (!Directory.Exists(sceneDirectory))
        {
            Console.WriteLine($"Le répertoire '{sceneDirectory}' est introuvable.");
            return;
        }

        var objectDetection = new ObjectDetection();
        var imageScenesData = new List<byte[]>();

        foreach (var imagePath in Directory.EnumerateFiles(sceneDirectory))
        {
            var imageBytes = await File.ReadAllBytesAsync(imagePath);
            imageScenesData.Add(imageBytes);
        }

        var detectObjectInScenesResults = await objectDetection.DetectObjectInScenesAsync(imageScenesData);

        foreach (var objectDetectionResult in detectObjectInScenesResults)
        {
            Console.WriteLine($"Box: {JsonSerializer.Serialize(objectDetectionResult.Box)}");
        }
    }
}
