namespace Benjamin.Trolais.ObjectDetection;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;

public record ObjectDetectionResult
{
    public byte[] ImageData { get; set; }
    public IList<BoundingBox> Box { get; set; }
}
