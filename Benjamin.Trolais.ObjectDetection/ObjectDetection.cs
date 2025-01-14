namespace Benjamin.Trolais.ObjectDetection;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ObjectDetection
{
    public async Task<IList<ObjectDetectionResult>> DetectObjectInScenesAsync(IList<byte[]> imagesSceneData)
    {
        IList<ObjectDetectionResult> results = new List<ObjectDetectionResult>();

        results.Add(new ObjectDetectionResult
        {
            ImageData = imagesSceneData[0], 
            Box = new List<BoundingBox>
            {
                new BoundingBox
                {
                    Confidence = 0.5f,
                    Label = "Car",
                    Dimensions = new BoundingBoxDimensions
                    {
                        Height = 2,
                        Width = 2,
                        Y = 0,
                        X = 0
                    }
                }
            }
        });

        results.Add(new ObjectDetectionResult
        {
            ImageData = imagesSceneData[1], 
            Box = new List<BoundingBox>
            {
                new BoundingBox
                {
                    Confidence = 0.9f,
                    Label = "Flower",
                    Dimensions = new BoundingBoxDimensions
                    {
                        Height = 1,
                        Width = 1,
                        Y = 1,
                        X = 1
                    }
                }
            }
        });

        await Task.Delay(1000);

        return results;
    }
}

