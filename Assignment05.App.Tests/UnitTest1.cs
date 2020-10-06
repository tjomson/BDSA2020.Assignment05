using System;
using System.Collections.Generic;
using System.Linq;
using Assignment05.App;
using SixLabors.ImageSharp;
using Xunit;
using Size = System.Drawing.Size;

namespace Assignment05.App.Tests
{
    public class UnitTest1
    {
        [Theory]
        [InlineData(2, 5, new long[]{4,9,16,25})]
        [InlineData(1, 5, new long[]{1,4,9,16,25})]
        public void Test_Squares(long lowerBound, long upperBound, long[] expected)
        {
            var actual = ParallelOperations.Squares(lowerBound, upperBound);

            var counter = 0;
            foreach (var square in actual)
            {
                Assert.Equal(expected[counter], square);
                counter++;
            }
            Assert.Equal(expected.Length, counter);
            
        }

        [Theory]
        [InlineData(1, 5, new long[]{1,4,9,16,25})]
        public void Test_Squares_Linq(int start, int count, long[] expected)
        {
            var actual = ParallelOperations.SquaresLinq(start, count);

            var counter = 0;
            foreach (var square in actual)
            {
                Assert.Equal(expected[counter], square);
                counter++;
            }
            Assert.Equal(expected.Length, counter);
            
        }
        
        [Fact]
        public void Test_Create_Thumbnails()
        {
            var pictureModule = new PictureModule();
            var imageFiles = new List<string>()
            {
                "file00053809264",
                "file000132701536",
                "file000267747089",
                "file000267804564",
                "file000325161223",
                "file000466623310",
                "file000477760838",
                "file000541344089",
                "file000555007525"
            };
            ParallelOperations.CreateThumbnails(pictureModule, imageFiles, "outputFolder", new Size(600, 600));
            
            foreach (var file in imageFiles)
            {
                var image = Image.Load("./../../../../Assignment05.App/outputFolder/" + file + ".jpg");
                Assert.True(image.Height == 600 || image.Width == 600);
            }
            
        }
    }
}
