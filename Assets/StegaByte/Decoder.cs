using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
namespace StegaByte
{
    public class Decoder
    {
        public static object Decode(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Image file not found.", filePath);

            byte[] combinedBytes;
            using (Image<Rgba32> image = Image.Load<Rgba32>(filePath))
            {
                int width = image.Width;
                int height = image.Height;
                combinedBytes = new byte[width * height * 3];

                Parallel.For(0, height, new ParallelOptions { MaxDegreeOfParallelism = Math.Max(1, Math.Min(Environment.ProcessorCount / 2, Environment.ProcessorCount)) }, y =>
                {
                    for (int x = 0; x < width; x++)
                    {
                        int index = (y * width + x) * 3;
                        Rgba32 pixel = image[x, y];

                        if (index < combinedBytes.Length) combinedBytes[index] = pixel.R;
                        if (index + 1 < combinedBytes.Length) combinedBytes[index + 1] = pixel.G;
                        if (index + 2 < combinedBytes.Length) combinedBytes[index + 2] = pixel.B;
                    }
                });
            }

            int typeLength = BitConverter.ToInt32(combinedBytes, 0);
            byte[] typeBytes = new byte[typeLength];
            byte[] dataBytes = new byte[combinedBytes.Length - 4 - typeLength];

            Array.Copy(combinedBytes, 4, typeBytes, 0, typeLength);
            Array.Copy(combinedBytes, 4 + typeLength, dataBytes, 0, dataBytes.Length);

            string typeName = Encoding.UTF8.GetString(typeBytes);
            Type type = Type.GetType(typeName) ?? throw new InvalidOperationException("Unable to resolve type.");
            string jsonData = Encoding.UTF8.GetString(dataBytes).TrimEnd('\0');

            return JsonSerializer.Deserialize(jsonData, type) ?? throw new InvalidOperationException("Failed to deserialize object.");
        }
    }
}
