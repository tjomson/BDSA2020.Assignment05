using System;
using System.Collections.Generic;
using System.Linq;
using Assignment05.App;
using Xunit;

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
    }
}
