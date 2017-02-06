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
        public async Task ShouldComputeSubTotalByAggregatingConstitueentBasketItems()
        {
            const string requestdataStream = "[{'Name':'Apple', 'Unit':1},{'Name':'Milk', 'Unit':1},{'Name':'Bread', 'Unit':0}]";
            var requestItems = JsonConvert.DeserializeObject<List<BasketRequestItem>>(requestdataStream);
            var mockBasketEconomicsManager = new BasketEconomicsManager(mockLogger.Object);
            await mockBasketEconomicsManager.ResetItemEconomicsAsync(new List<BasketItemEconomics>
            {
                new BasketItemEconomics {Name = "Apple", Discount = 0.1M, Price = 1.00M},
                new BasketItemEconomics {Name = "Bread", Discount = null, Price = 0.80M},
                new BasketItemEconomics {Name = "Milk", Discount = null, Price = 1.30M},
                new BasketItemEconomics {Name = "Soup", Discount = null, Price = 0.65M}
            });
            var basketPricer = new BasketPricer(mockBasketEconomicsManager, mockLogger.Object);
            var basket = await basketPricer.PriceAsync(requestItems);
            Assert.Equal(basket.SubTotal, 3.10M);
        }
    }
}
