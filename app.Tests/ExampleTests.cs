using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Xunit;

namespace transport_sim_app.Tests
{
    public class ExampleTests
    {
        [Fact]
        public void PrimeNumbers_Test()
        {
            IList<int> primes = new List<int>();
            const int PRIME_NUMBER_COUNT = 15;

            for (int i = 2; i < PRIME_NUMBER_COUNT; i++)
            {
                var prime = true;
                for (int j = 2; j < i; j++)
                {
                    if (i % j == 0) prime = false;
                }
                if (prime) primes.Add(i);
            }

            var checkValues = GetPrimeNumber();
            Assert.NotEmpty(primes);
            Assert.Equal(primes, checkValues);
        }

        IList<int> GetPrimeNumber() => new List<int>{2,3,5,7,11,13};
    }
}
