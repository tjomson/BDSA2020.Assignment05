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
        public void Test_Squares(long lowerBound, long upperBound, long[] expected)
        {
            var asList = expected.ToList();
            var actual = ParallelOperations.Squares(lowerBound, upperBound);

            foreach (var square in actual)
            {
                Assert.Contains(square, asList);
            }
            Assert.Equal(4, actual.Count);
            
        }
    }
}
