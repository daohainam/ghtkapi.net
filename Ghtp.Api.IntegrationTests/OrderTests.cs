using Ghtk.Api;
using Ghtk.Api.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace Ghtp.Api.IntegrationTests
{
    public class OrderTests : IClassFixture<SUTWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly SUTWebApplicationFactory _factory;

        public OrderTests(SUTWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();

            _client.DefaultRequestHeaders.Add("X-Client-Source", SUTWebApplicationFactory.ClientSource);
            _client.DefaultRequestHeaders.Add("Token", SUTWebApplicationFactory.Token);
        }

        [Fact]
        public async Task Submit_And_Cancel_Order_Test()
        {
            var submitOrderRequest = new SubmitOrderRequest
            {
                Order = new SubmitOrderRequestOrder
                {
                    Id = Guid.NewGuid().ToString(),
                    PickName = "PickName",
                    PickAddress = "PickAddress",
                    PickProvince = "PickProvince",
                    PickDistrict = "PickDistrict",
                    PickWard = "PickWard",
                    PickTel = "PickTel",
                    Tel = "Tel",
                    Name = "Name",
                    Address = "Address",
                    Province = "Province",
                    District = "District",
                    Ward = "Ward",
                    Hamlet = "Hamlet",
                    IsFreeship = 1,
                    PickMoney = 1,
                    Note = "note",
                    Value = 1,
                    Transport = "transport",
                    PickOption = "pick_option",
                    DeliverOption = "deliver_option",
                    PickDate = DateTimeOffset.Now,
                },
                Products = [
                    new OrderProduct
                    {
                        Name = "Product1",
                        Weight = 999991,
                        Quantity = 1,
                        ProductCode = 199999,
                    },
                    new OrderProduct
                    {
                        Name = "Product2",
                        Weight = 999992,
                        Quantity = 2,
                        ProductCode = 299999,
                    },
                ],
            };

            var response = await _client.PostAsJsonAsync("/services/shipment/order", submitOrderRequest);
            response.EnsureSuccessStatusCode();

            var submitOrderResponse = await response.Content.ReadFromJsonAsync<SubmitOrderResponse>();
            Assert.NotNull(submitOrderResponse);
            Assert.NotNull(submitOrderResponse.Order.TrackingId);
            Assert.Equal(submitOrderRequest.Products.Length, submitOrderResponse.Order.Products.Length);
            foreach (var product in submitOrderRequest.Products)
            {
                Assert.Contains(submitOrderResponse.Order.Products, p => p.Name == product.Name && p.Quantity == product.Quantity && p.Weight == product.Weight && p.ProductCode == product.ProductCode);
            }
            
            var getOrdersResponse = await _client.GetAsync($"/services/shipment/v2/{submitOrderResponse.Order.TrackingId}");
            getOrdersResponse.EnsureSuccessStatusCode();

            var getOrderStatusResponse = await getOrdersResponse.Content.ReadFromJsonAsync<GetOrderStatusResponse>();
            Assert.NotNull(getOrderStatusResponse);

            var cancelOrderResponse = await _client.PostAsync($"/services/shipment/cancel/{submitOrderResponse.Order.TrackingId}", new StringContent(string.Empty));
            cancelOrderResponse.EnsureSuccessStatusCode();

            var apiResult = await cancelOrderResponse.Content.ReadFromJsonAsync<ApiResult>();
            Assert.NotNull(apiResult);
            Assert.True(apiResult.Success);

        }
    }
}