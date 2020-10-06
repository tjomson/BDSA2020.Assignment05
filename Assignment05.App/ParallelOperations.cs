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
            
            return squares.OrderBy(i => i).ToArray();

        }

        public static IEnumerable<long> SquaresLinq(int start, int count) =>
            (from item in Enumerable.Range(start, start + count - 1).AsParallel()
              select (long) item * item).ToArray().OrderBy(i => i);

            //As one-liner
            //Enumerable.Range(start, start + count).AsParallel().Select(l => (long) l * l).ToArray().OrderBy(i => i);

        public static void CreateThumbnails(IPictureModule resizer, IEnumerable<string> imageFiles, string outputFolder, Size size)
        {
            throw new NotImplementedException();
        }
    }
}
