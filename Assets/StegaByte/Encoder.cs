using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
namespace StegaByte
{
    public class Encoder
    {
        public static void Encode(object obj, string filePath)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj), "Object cannot be null.");

            string jsonData = JsonSerializer.Serialize(obj);
            byte[] dataBytes = Encoding.UTF8.GetBytes(jsonData);
            string typeName = obj.GetType().AssemblyQualifiedName;
            byte[] typeBytes = Encoding.UTF8.GetBytes(typeName);
            int typeLength = typeBytes.Length;

            byte[] combinedBytes = new byte[4 + typeLength + dataBytes.Length];
            BitConverter.GetBytes(typeLength).CopyTo(combinedBytes, 0);
            typeBytes.CopyTo(combinedBytes, 4);
            dataBytes.CopyTo(combinedBytes, 4 + typeLength);

            int width = (int)Math.Ceiling(Math.Sqrt(combinedBytes.Length / 3.0));
            int height = width;
            int maxThreads = Math.Max(1, Math.Min(Environment.ProcessorCount / 2, Environment.ProcessorCount));

            using (Image<Rgba32> image = new Image<Rgba32>(width, height))
            {
                Parallel.For(0, height, new ParallelOptions { MaxDegreeOfParallelism = maxThreads }, y =>
                {
                    for (int x = 0; x < width; x++)
                    {
                        int index = (y * width + x) * 3;
                        byte r = index < combinedBytes.Length ? combinedBytes[index] : (byte)0;
                        byte g = index + 1 < combinedBytes.Length ? combinedBytes[index + 1] : (byte)0;
                        byte b = index + 2 < combinedBytes.Length ? combinedBytes[index + 2] : (byte)0;
                        image[x, y] = new Rgba32(r, g, b);
                    }
                });

                image.SaveAsPng(filePath);
            }
        }
    }
}