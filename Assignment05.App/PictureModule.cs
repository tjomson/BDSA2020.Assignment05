using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System.Drawing;

namespace Assignment05
{
    public class PictureModule : IPictureModule
    {
        public void Resize(string inputFile, string outputFile, System.Drawing.Size size)
        {
            using (var image = Image.Load(inputFile))
            {
                var options = new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new SixLabors.ImageSharp.Size(size.Width, size.Height)
                };

                image.Mutate(x => x.Resize(options));
                image.Save(outputFile);
            }
        }
    }
}
