using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ROP.UnitTest
{
    public class TestFallback
    {

        [Fact]
        public void TestFallbackFAllsInsideFallback()
        {
            var result =
                MetodoOriginal(1)
                .Bind(x => 
                    MetodoQueFalla(x)
                    .Fallback(_=>MetodoQueDevuelveNumeroDeMeses(x))
                    );

            Assert.True(result.Success);
            Assert.Equal(12, result.Value);

        }


        [Fact]
        public void TestFallbackWhenMethodDoesntFail()
        {
            var result =
                MetodoOriginal(2)
                .Bind(x =>
                    MatodoQueMultiplica(x)
                    .Fallback(_ => MetodoQueDevuelveNumeroDeMeses(x))
                    );

            Assert.True(result.Success);
            Assert.Equal(4, result.Value);
        }

        [Fact]
        public async Task TestFallbackWithNonAsyncMethodInTheMiddle()
        {
            int originalValue = 1;

            Result<int> result = await MetodoQueFalla(originalValue).Async()    // <- async value
                .Fallback(x => MetodoOriginal(1))                               // <- Sincronous method
                .Bind(MatodoQueMultiplica);                                     // <- async metohd

            Assert.True(result.Success);
            Assert.Equal(originalValue, result.Value);
        }
                
        [Fact]
        public void TestFallbackWithConditionAndSyncMethods()
        {
            int originalValue = 999;
            int notOriginalValue = 1;
            string errorMessage = "aOriginalMessage";

            Result<int> result = MetodoQueFallaConErrorMessage(originalValue, errorMessage)                     // <- Sincronous value
                .Fallback(x => x.Errors.First().Message == errorMessage, y => MetodoOriginal(notOriginalValue)) // <- Sincronous value
                .Bind(MatodoQueMultiplica);

            Assert.True(result.Success);
            Assert.Equal(notOriginalValue, result.Value);
        }

        [Fact]
        public void TestFallbackWithNotMetConditionAndSyncMethods()
        {
            int originalValue = 999;
            int notOriginalValue = 1;
            string errorMessage = "aOriginalMessage";

            Result<int> result = MetodoQueFallaConErrorMessage(originalValue, errorMessage)
                .Fallback(_ => false, y => MetodoOriginal(notOriginalValue))
                .Bind(MatodoQueMultiplica);

            Assert.False(result.Success);
            Assert.Equal(errorMessage, result.Errors.First().Message);
        }

        [Fact]
        public void TestFallbackWhenMethodDoesNotFailWithSyncMethods()
        {
            int originalValue = 999;
            int notOriginalValue = 1;

            Result<int> result = MetodoOriginal(notOriginalValue)
                .Fallback(_ => true, y => MetodoOriginal(originalValue))
                .Bind(MatodoQueMultiplica);

            Assert.True(result.Success);
            Assert.Equal(notOriginalValue, result.Value);
        }
        
        [Fact]
        public async Task TestFallbackWithConditionAndNonAsyncMethodInTheMiddle()
        {
            int originalValue = 999;
            int notOriginalValue = 1;
            string errorMessage = "aOriginalMessage";

            Result<int> result = await MetodoQueFallaConErrorMessage(originalValue, errorMessage).Async()   // <- async value
                .Fallback(_ => true, y => MetodoOriginal(notOriginalValue))                                 // <- Sincronous value
                .Bind(MatodoQueMultiplica);                                                                 

            Assert.True(result.Success);
            Assert.Equal(notOriginalValue, result.Value);
        }

        [Fact]
        public async Task TestFallbackWithNotMetConditionAndNonAsyncMethodInTheMiddle()
        {
            int originalValue = 999;
            int notOriginalValue = 1;
            string errorMessage = "aOriginalMessage";

            Result<int> result = await MetodoQueFallaConErrorMessage(originalValue, errorMessage).Async()
                .Fallback(_ => false, y => MetodoOriginal(notOriginalValue))
                .Bind(MatodoQueMultiplica);

            Assert.False(result.Success);
            Assert.Equal(errorMessage, result.Errors.First().Message);
        }

        [Fact]
        public async Task TestFallbackWhenMethodDoesNotFailWithAsyncMethodInTheMiddle()
        {
            int originalValue = 999;
            int notOriginalValue = 1;

            Result<int> result = await MetodoOriginal(notOriginalValue).Async()
                .Fallback(_ => true, y => MetodoOriginal(originalValue))
                .Bind(MatodoQueMultiplica);

            Assert.True(result.Success);
            Assert.Equal(notOriginalValue, result.Value);
        }
        
        [Fact]
        public async Task TestFallbackWithConditionAndAsyncMethods()
        {
            int originalValue = 999;
            int notOriginalValue = 1;
            string errorMessage = "aOriginalMessage";

            Result<int> result = await MetodoQueFallaConErrorMessage(originalValue, errorMessage).Async()   // <- async value
                .Fallback(_ => true, y => MetodoOriginal(notOriginalValue).Async())                         // <- async value
                .Bind(MatodoQueMultiplica); 

            Assert.True(result.Success);
            Assert.Equal(notOriginalValue, result.Value);
        }

        [Fact]
        public async Task TestFallbackWithNotMetConditionAndAsyncMethods()
        {
            int originalValue = 999;
            int notOriginalValue = 1;
            string errorMessage = "aOriginalMessage";

            Result<int> result = await MetodoQueFallaConErrorMessage(originalValue, errorMessage).Async()
                .Fallback(_ => false, y => MetodoOriginal(notOriginalValue).Async())
                .Bind(MatodoQueMultiplica);

            Assert.False(result.Success);
            Assert.Equal(errorMessage, result.Errors.First().Message);
        }

        [Fact]
        public async Task TestFallbackWhenMethodDoesNotFailWithAsyncMethods()
        {
            int originalValue = 999;
            int notOriginalValue = 1;

            Result<int> result = await MetodoOriginal(notOriginalValue).Async()
                .Fallback(_ => true, y => MetodoOriginal(originalValue).Async())
                .Bind(MatodoQueMultiplica);

            Assert.True(result.Success);
            Assert.Equal(notOriginalValue, result.Value);
        }

        private Result<int> MetodoOriginal(int i) => i;
        
        private Result<int> MetodoQueFalla(int i) => Result.Failure<int>("error");

        private Result<int> MatodoQueMultiplica(int i) => i * i;

        private Result<int> MetodoQueDevuelveNumeroDeMeses(int i) => 12;
        
        private Result<int> MetodoQueFallaConErrorMessage(int originalValue, string errorMessage) 
            => Result.Failure<int>(errorMessage);
    }
}
