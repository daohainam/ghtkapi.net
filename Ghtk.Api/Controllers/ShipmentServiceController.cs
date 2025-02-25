﻿using AutoMapper;
using Ghtk.Api.Models;
using Ghtk.Repository;
using Ghtk.Repository.Abstractions.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ghtk.Api.Controllers
{
    [ApiController]
    [Route("/services/shipment")]
    public class ShipmentServiceController(IOrderRepository orderRepository, IMapper mapper, ILogger<ShipmentServiceController> logger) : ControllerBase
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<ShipmentServiceController> _logger = logger;

        [HttpPost]
        [Route("order")]
        [Authorize]
        public async Task<IActionResult> SubmitOrder([FromBody] SubmitOrderRequest order)
        {
            _logger.LogInformation("SubmitOrder called");

            var partnerId = User.FindFirst("PartnerId")?.Value;
            if (string.IsNullOrEmpty(partnerId))
            {
                return Unauthorized();
            }

            var orderEntity = _mapper.Map<Order>(order);
            orderEntity.PartnerId = partnerId;

            await _orderRepository.CreateOrderAsync(orderEntity);

            var response = new SubmitOrderResponse
            {
                Success = true,

                Order = new SubmitOrderResponseOrder
                {
                    PartnerId = partnerId,
                    Label = "label",
                    Area = 1,
                    Fee = 1.0,
                    InsuranceFee = 1.0,
                    TrackingId = orderEntity.TrackingId,
                    EstimatedPickTime = "2021-01-01T00:00:00Z",
                    EstimatedDeliverTime = "2021-01-01T00:00:00Z",
                    Products = order.Products.Select(p => new OrderProduct() {
                        Name = p.Name,
                        Quantity = p.Quantity,
                        Weight = p.Weight,
                        ProductCode = p.ProductCode
                    }).ToArray(),
                    StatusId = orderEntity.Status
                }
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("v2/{id}")]
        [Authorize]
        public async Task<IActionResult> GetOrderStatus(string id)
        {
            _logger.LogInformation("GetOrderStatus called with id {id}", id);

            var partnerId = User.FindFirst("PartnerId")?.Value;
            if (string.IsNullOrEmpty(partnerId))
            {
                return Unauthorized();
            }

            var order = await _orderRepository.FindOrderAsync(id, partnerId);
            if (order == null)
            {
                return NotFound(new ApiResult()
                {
                    Success = false,
                    Message = "Tracking Id not found"
                });
            }

            var result = new GetOrderStatusResponse()
            {
                Success = true,
                Order = new StatusOrder
                {
                    LabelId = "label_id",
                    PartnerId = partnerId,
                    Status = order.Status,
                    StatusText = "status_text",
                    Created = DateTimeOffset.Now,
                    Modified = DateTimeOffset.Now,
                    Message = "message",
                    PickDate = DateTimeOffset.Now,
                    DeliverDate = DateTimeOffset.Now,
                    CustomerFullname = "customer_fullname",
                    CustomerTel = "customer_tel",
                    Address = order.Address,
                    StorageDay = 1,
                    ShipMoney = 1,
                    Insurance = 1,
                    Value = order.Value
                }
            };

            return Ok(result);
        }

        [HttpPost]
        [Route("cancel/{id}")]
        [Authorize]
        public async Task<IActionResult> CancelOrder(string id)
        {
            _logger.LogInformation("CancelOrder called with id {id}", id);

            var partnerId = User.FindFirst("PartnerId")?.Value;
            if (string.IsNullOrEmpty(partnerId))
            {
                return Unauthorized();
            }

            var b = await _orderRepository.CancelOrderAsync(id, partnerId);

            if (!b)
            {
                return NotFound(new ApiResult()
                {
                    Success = false,
                    Message = "Tracking Id not found"
                });
            }

            var result = new ApiResult
            {
                Success = true,
                Message = "Order canceled"
            };

            return Ok(result);
        }
    }
}
