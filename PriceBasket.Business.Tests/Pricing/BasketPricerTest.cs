using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Moq;
using Newtonsoft.Json;
using PriceBasket.Business.Economics;
using PriceBasket.Business.Models;
using PriceBasket.Business.Pricing;
using Xunit;

namespace PriceBasket.Business.Tests.Pricing
{
    public class BasketPricerTest
    {
        private Mock<ILog> mockLogger;
        public BasketPricerTest()
        {
            mockLogger = new Mock<ILog>();
        }

        [Fact]
        public async Task ShouldComputeSubTotalAsSumTotalValueWithNoDiscount()
        {
            const string requestdataStream = "[{'Name':'Apple', 'Unit':1},{'Name':'Milk', 'Unit':1},{'Name':'Bread', 'Unit':0}]";
            var mockrequestItems = JsonConvert.DeserializeObject<List<BasketRequestItem>>(requestdataStream);
            var mockBasketEconomicsManager = new BasketEconomicsManager(mockLogger.Object);
            await mockBasketEconomicsManager.ResetItemEconomicsAsync(new List<BasketItemEconomics>
            {
                new BasketItemEconomics {Name = "Apple", Discount = 0.1M, Price = 1.00M},
                new BasketItemEconomics {Name = "Bread", Discount = null, Price = 0.80M},
                new BasketItemEconomics {Name = "Milk", Discount = null, Price = 1.30M},
                new BasketItemEconomics {Name = "Soup", Discount = null, Price = 0.65M}
            });
            var basketPricer = new BasketPricer(mockBasketEconomicsManager, mockLogger.Object);
            var basket = await basketPricer.PriceAsync(mockrequestItems);
            Assert.Equal(2.30M, basket.SubTotal);
        }

        [Fact]
        public async Task ShouldComputeTotalAsSumofSubtotalMinusDiscounts()
        {
            const string requestdataStream = "[{'Name':'Apple', 'Unit':1},{'Name':'Milk', 'Unit':1},{'Name':'Bread', 'Unit':0}]";
            var mockrequestItems = JsonConvert.DeserializeObject<List<BasketRequestItem>>(requestdataStream);
            var mockBasketEconomicsManager = new BasketEconomicsManager(mockLogger.Object);
            await mockBasketEconomicsManager.ResetItemEconomicsAsync(new List<BasketItemEconomics>
            {
                new BasketItemEconomics {Name = "Apple", Discount = 0.1M, Price = 1.00M},
                new BasketItemEconomics {Name = "Bread", Discount = null, Price = 0.80M},
                new BasketItemEconomics {Name = "Milk", Discount = null, Price = 1.30M},
                new BasketItemEconomics {Name = "Soup", Discount = null, Price = 0.65M}
            });
            var basketPricer = new BasketPricer(mockBasketEconomicsManager, mockLogger.Object);
            var basket = await basketPricer.PriceAsync(mockrequestItems);
            Assert.Equal(2.20M,basket.Total);
        }

        [Theory]
        [InlineData("[{'Name':'Apple', 'Unit':1},{'Name':'Milk', 'Unit':1},{'Name':'Bread', 'Unit':0}]", 2.20)]
        [InlineData("[{'Name':'Apple', 'Unit':1},{'Name':'Milk', 'Unit':1},{'Name':'Soup', 'Unit':1}]", 2.85)]
        [InlineData("[{'Name':'Apple', 'Unit':1},{'Name':'Milk', 'Unit':1},{'Name':'Bread', 'Unit':1}]", 3.00)]
        public async Task ShouldComputeTotalAsSubTotalMinusDiscountsOnMultipleDataStreams(string requestdataStream,decimal expectedTotal)
        {
            var mockrequestItems = JsonConvert.DeserializeObject<List<BasketRequestItem>>(requestdataStream);
            var mockBasketEconomicsManager = new BasketEconomicsManager(mockLogger.Object);
            await mockBasketEconomicsManager.ResetItemEconomicsAsync(new List<BasketItemEconomics>
            {
                new BasketItemEconomics {Name = "Apple", Discount = 0.1M, Price = 1.00M},
                new BasketItemEconomics {Name = "Bread", Discount = null, Price = 0.80M},
                new BasketItemEconomics {Name = "Milk", Discount = null, Price = 1.30M},
                new BasketItemEconomics {Name = "Soup", Discount = null, Price = 0.65M}
            });
            var basketPricer = new BasketPricer(mockBasketEconomicsManager, mockLogger.Object);
            var basket = await basketPricer.PriceAsync(mockrequestItems);
            Assert.Equal(expectedTotal, basket.Total);
        }

        [Theory]
        [InlineData(1.00,0.80,1.30,0.65,2.20)]
        [InlineData(2.00, 4.80, 1.30, 7.65, 3.10)]
        [InlineData(3.00, 2.80, 0.30, 1.95, 3.00)]
        public async Task ShouldComputeSubTotalCorrectlyUnderDifferentPrices(decimal priceApple,decimal priceBread,
            decimal priceMilk, decimal priceSoup, decimal expectedTotal)
        {
            const string requestdataStream = "[{'Name':'Apple', 'Unit':1},{'Name':'Milk', 'Unit':1},{'Name':'Bread', 'Unit':0}]";
            var mockrequestItems = JsonConvert.DeserializeObject<List<BasketRequestItem>>(requestdataStream);
            var mockBasketEconomicsManager = new BasketEconomicsManager(mockLogger.Object);
            await mockBasketEconomicsManager.ResetItemEconomicsAsync(new List<BasketItemEconomics>
            {
                new BasketItemEconomics {Name = "Apple", Discount = 0.1M, Price = priceApple},
                new BasketItemEconomics {Name = "Bread", Discount = null, Price = priceBread},
                new BasketItemEconomics {Name = "Milk", Discount = null, Price = priceMilk},
                new BasketItemEconomics {Name = "Soup", Discount = null, Price = priceSoup}
            });
            var basketPricer = new BasketPricer(mockBasketEconomicsManager, mockLogger.Object);
            var basket = await basketPricer.PriceAsync(mockrequestItems);
            Assert.Equal(expectedTotal, basket.Total);
        }

        [Theory]
        [InlineData(0.10, null, null, null, 2.20)]
        [InlineData(0.20, null, 0.50, 0.65, 1.450)]
        [InlineData(null, 0.80, 0.30, 0.95, 1.910)]
        public async Task ShouldComputeSubTotalCorrectlyUnderDifferentDiscounts(decimal discountApple, decimal discountBread,
            decimal discountMilk, decimal discountSoup, decimal expectedTotal)
        {
            const string requestdataStream = "[{'Name':'Apple', 'Unit':1},{'Name':'Milk', 'Unit':1},{'Name':'Bread', 'Unit':0}]";
            var mockrequestItems = JsonConvert.DeserializeObject<List<BasketRequestItem>>(requestdataStream);
            var mockBasketEconomicsManager = new BasketEconomicsManager(mockLogger.Object);
            await mockBasketEconomicsManager.ResetItemEconomicsAsync(new List<BasketItemEconomics>
            {
                new BasketItemEconomics {Name = "Apple", Discount = discountApple, Price = 1.00M},
                new BasketItemEconomics {Name = "Bread", Discount = discountBread, Price = 0.80M},
                new BasketItemEconomics {Name = "Milk", Discount = discountMilk, Price = 1.30M},
                new BasketItemEconomics {Name = "Soup", Discount = discountSoup, Price = 0.65M}
            });
            var basketPricer = new BasketPricer(mockBasketEconomicsManager, mockLogger.Object);
            var basket = await basketPricer.PriceAsync(mockrequestItems);
            Assert.Equal(expectedTotal, basket.Total);
        }

        [Fact]
        public async Task ShouldCorrectlyComputeTotalWhenBasketContainsMultipleItemDiscounts()
        {
            const string requestdataStream = "[{'Name':'Apple', 'Unit':1},{'Name':'Milk', 'Unit':1},{'Name':'Bread', 'Unit':0}]";
            var mockrequestItems = JsonConvert.DeserializeObject<List<BasketRequestItem>>(requestdataStream);
            var mockBasketEconomicsManager = new BasketEconomicsManager(mockLogger.Object);
            await mockBasketEconomicsManager.ResetItemEconomicsAsync(new List<BasketItemEconomics>
            {
                new BasketItemEconomics {Name = "Apple", Discount = 0.1M, Price = 1.00M},
                new BasketItemEconomics {Name = "Bread", Discount = null, Price = 0.80M},
                new BasketItemEconomics {Name = "Milk", Discount = 0.2M, Price = 1.30M},
                new BasketItemEconomics {Name = "Soup", Discount = null, Price = 0.65M}
            });
            var basketPricer = new BasketPricer(mockBasketEconomicsManager, mockLogger.Object);
            var basket = await basketPricer.PriceAsync(mockrequestItems);
            Assert.Equal(1.94M,basket.Total);
        }

        [Fact]
        public async Task ShouldThrowExceptionWhenMissingEconomicData()
        {
            const string requestdataStream = "[{'Name':'Apple', 'Unit':1},{'Name':'Milk', 'Unit':1},{'Name':'Bread', 'Unit':0},{'Name':'Oil', 'Unit':0}]";
            var mockrequestItems = JsonConvert.DeserializeObject<List<BasketRequestItem>>(requestdataStream);
            var mockBasketEconomicsManager = new BasketEconomicsManager(mockLogger.Object);
            await mockBasketEconomicsManager.ResetItemEconomicsAsync(new List<BasketItemEconomics>
            {
                new BasketItemEconomics {Name = "Apple", Discount = 0.1M, Price = 1.00M},
                new BasketItemEconomics {Name = "Bread", Discount = null, Price = 0.80M},
                new BasketItemEconomics {Name = "Milk", Discount = 0.2M, Price = 1.30M},
                new BasketItemEconomics {Name = "Soup", Discount = null, Price = 0.65M}
            });
            var basketPricer = new BasketPricer(mockBasketEconomicsManager, mockLogger.Object);
            var exception = await Assert.ThrowsAsync<Exception>(async() =>await basketPricer.PriceAsync(mockrequestItems));
            Assert.True(exception.Message.ToString().Contains("No Economics setup for Item Oil. Please configure economics"));
        }

        [Fact]
        public async Task MissingEconomicDataExceptionShouldNotImpactExistingItemsCorrectPricing()
        {
            string requestdataStream = "[{'Name':'Apple', 'Unit':1},{'Name':'Milk', 'Unit':1},{'Name':'Bread', 'Unit':0},{'Name':'Oil', 'Unit':0}]";
            var mockrequestItems = JsonConvert.DeserializeObject<List<BasketRequestItem>>(requestdataStream);
            var mockBasketEconomicsManager = new BasketEconomicsManager(mockLogger.Object);
            await mockBasketEconomicsManager.ResetItemEconomicsAsync(new List<BasketItemEconomics>
            {
                new BasketItemEconomics {Name = "Apple", Discount = 0.1M, Price = 1.00M},
                new BasketItemEconomics {Name = "Bread", Discount = null, Price = 0.80M},
                new BasketItemEconomics {Name = "Milk", Discount = 0.2M, Price = 1.30M},
                new BasketItemEconomics {Name = "Soup", Discount = null, Price = 0.65M}
            });
            var basketPricer = new BasketPricer(mockBasketEconomicsManager, mockLogger.Object);
            var exception = await Assert.ThrowsAsync<Exception>(async () => await basketPricer.PriceAsync(mockrequestItems));
            Assert.True(exception.Message.ToString().Contains("No Economics setup for Item Oil. Please configure economics"));
            requestdataStream = "[{'Name':'Apple', 'Unit':1},{'Name':'Milk', 'Unit':1},{'Name':'Bread', 'Unit':0}]";
            mockrequestItems = JsonConvert.DeserializeObject<List<BasketRequestItem>>(requestdataStream);
            var basket = await basketPricer.PriceAsync(mockrequestItems);
            Assert.Equal(1.94M,basket.Total);
        }

        [Fact]
        public async Task AddingMisingEconomicDataToEconomicsManagerWillAllowPricingNewItems()
        {
            string requestdataStream = "[{'Name':'Apple', 'Unit':1},{'Name':'Milk', 'Unit':1},{'Name':'Bread', 'Unit':0},{'Name':'Oil', 'Unit':1}]";
            var mockrequestItems = JsonConvert.DeserializeObject<List<BasketRequestItem>>(requestdataStream);
            var mockBasketEconomicsManager = new BasketEconomicsManager(mockLogger.Object);
            await mockBasketEconomicsManager.ResetItemEconomicsAsync(new List<BasketItemEconomics>
            {
                new BasketItemEconomics {Name = "Apple", Discount = 0.1M, Price = 1.00M},
                new BasketItemEconomics {Name = "Bread", Discount = null, Price = 0.80M},
                new BasketItemEconomics {Name = "Milk", Discount = 0.2M, Price = 1.30M},
                new BasketItemEconomics {Name = "Soup", Discount = null, Price = 0.65M}
            });
            var basketPricer = new BasketPricer(mockBasketEconomicsManager, mockLogger.Object);
            var exception = await Assert.ThrowsAsync<Exception>(async () => await basketPricer.PriceAsync(mockrequestItems));
            Assert.True(exception.Message.ToString().Contains("No Economics setup for Item Oil. Please configure economics"));
            await mockBasketEconomicsManager.ResetItemEconomicsAsync(new List<BasketItemEconomics>
            {
                new BasketItemEconomics {Name = "Oil", Discount = null, Price = 0.65M}
            });
            mockrequestItems = JsonConvert.DeserializeObject<List<BasketRequestItem>>(requestdataStream);
            var basket = await basketPricer.PriceAsync(mockrequestItems);
            Assert.Equal(2.590M, basket.Total);
        }
    }
}
