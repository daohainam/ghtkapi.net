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
        }
    }
}