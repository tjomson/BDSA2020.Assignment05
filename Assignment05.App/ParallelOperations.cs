using System;
using System.Collections.Generic;
using System.Drawing;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Linq;

namespace Assignment05.App
{
    public class ParallelOperations
    {
        public static ICollection<long> Squares(long lowerBound, long upperBound)
        {
            var squares = new ConcurrentBag<long>();
            
            Parallel.For(lowerBound, upperBound + 1, i =>
            {
               squares.Add(i * i); 
            });

            var returnable = new List<long>(squares);

            return returnable;
        }
        
        public static IEnumerable<long> SquaresLinq(int start, int count) =>
  
        throw new NotImplementedException();

        public static void CreateThumbnails(IPictureModule resizer, IEnumerable<string> imageFiles, string outputFolder, Size size)
        {
            throw new NotImplementedException();
        }
    }
}
