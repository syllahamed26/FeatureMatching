using System.Reflection;
using System.Text.Json;

namespace hamed.sylla.FeatureMatching.Tests;

public class FeatureMatchingUnitTest
{
    [Fact]
    public async Task ObjectShouldBeDetectedCorrectly() {
        var executingPath = GetExecutingPath();
        var imageScenesData = new List<byte[]>();
        foreach (var imagePath in Directory.EnumerateFiles(Path.Combine(executingPath, "Scenes")))
        {
            var imageBytes = await File.ReadAllBytesAsync(imagePath);
            imageScenesData.Add(imageBytes);
        }
        
        var objectImageData = await File.ReadAllBytesAsync(Path.Combine(executingPath, "Sylla-Hamed-object.jpg"));
        var detectObjectInScenesResults = await new ObjectDetection().DetectObjectInScenesAsync(objectImageData, imageScenesData);
        Assert.Equal(
            "[{\"X\":26,\"Y\":27}]",
            JsonSerializer.Serialize(detectObjectInScenesResults[0].Points)
            );
        Assert.Equal(
            "[{\"X\":28,\"Y\":29}]",
            JsonSerializer.Serialize(detectObjectInScenesResults[1].Points)
            );
    }
    private static string GetExecutingPath() {
        var executingAssemblyPath = Assembly.GetExecutingAssembly().Location; 
        var executingPath = Path.GetDirectoryName(executingAssemblyPath); 
        return executingPath;
    }
}