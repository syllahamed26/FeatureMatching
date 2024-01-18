using System.Text.Json;

namespace hamed.sylla.FeatureMatching.Console;

public class FeatureMatchingConsole
{
    public async Task RunAsync(string[] args)
    {
        if (args.Length != 2)
        {
            throw new ArgumentException("You must provide two arguments: first is the path to the image you want to detect, second is the directory containing the images you want to match.");
        }
        
        if (!File.Exists(args[0]))
        {
            throw new ArgumentException("The first argument must be a file.");
        }
        
        if (!Directory.Exists(args[1]))
        {
            throw new ArgumentException("The second argument must be a directory.");
        }
        
        var imageBytes = File.ReadAllBytes(args[0]);
        var images = new List<byte[]>();
        foreach (var path in Directory.EnumerateFiles(args[1]))
        {
            var sceneBytes = File.ReadAllBytes(path);
            images.Add(sceneBytes);
        }
        
        var detectObjectInScenesResults = await new ObjectDetection().DetectObjectInScenesAsync(imageBytes, images);
        foreach (var detectObjectInScenesResult in detectObjectInScenesResults)
        {
            System.Console.WriteLine($"Points: {JsonSerializer.Serialize(detectObjectInScenesResult.Points)}");
        }
    }
}