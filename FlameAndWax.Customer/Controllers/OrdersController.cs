﻿using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Customer.Models;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FlameAndWax.Customer.Controllers
{
    [Authorize(Roles = nameof(Constants.Roles.Customer))]
    public class OrdersController : Controller
    {
        private readonly IOrdersService _ordersService;
        private readonly IConfiguration _configuration;
        private string ConnectionString { get; }

        public OrdersController(IOrdersService ordersService, IConfiguration configuration)
        {
            _ordersService = ordersService;
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("FlameAndWaxDBConnection");
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 3)
        {
            var customerIdLoggedIn = User.Claims.FirstOrDefault(userId => userId.Type == ClaimTypes.NameIdentifier).Value;
            var ordersServiceResult = await _ordersService.FetchOrdersByStatusAsync(pageNumber, pageSize, int.Parse(customerIdLoggedIn), Constants.OrderStatus.Pending, ConnectionString);
            if (ordersServiceResult.HasError) return BadRequest(new { errorContent = ordersServiceResult.ErrorContent });

            var totalNumberOfOrdersServiceResult = await _ordersService.FetchTotalNumberOfOrdersByOrderStatusAsync(Constants.OrderStatus.Pending, ConnectionString, int.Parse(customerIdLoggedIn));

            var totalNumberOfPages = Math.Ceiling((decimal)totalNumberOfOrdersServiceResult.Result / pageSize);
            ViewData["OrderCount"] = (int)totalNumberOfPages;
            ViewData["OrderStatus"] = nameof(Constants.OrderStatus.Pending);

            var orderViewModels = new List<OrderViewModel>();
            foreach (var order in ordersServiceResult.Result) BuildOrderViewModels(order.OrderDetails, orderViewModels, order);

            return View(orderViewModels);
        }

        public async Task<IActionResult> PageOrders(string orderStatus = nameof(Constants.OrderStatus.Pending), int pageNumber = 1, int pageSize = 3)
        {
            var customerIdLoggedIn = User.Claims.FirstOrDefault(userId => userId.Type == ClaimTypes.NameIdentifier).Value;
            var categorizedOrdersServiceResult = await _ordersService.FetchOrdersByStatusAsync(pageNumber, pageSize, int.Parse(customerIdLoggedIn), ServiceHelper.ConvertStringtoOrderStatus(orderStatus), ConnectionString);
            if (categorizedOrdersServiceResult.HasError) return BadRequest(new { errorContent = categorizedOrdersServiceResult.ErrorContent });

            var totalNumberOfOrdersServiceResult = await _ordersService.FetchTotalNumberOfOrdersByOrderStatusAsync(ServiceHelper.ConvertStringtoOrderStatus(orderStatus), ConnectionString, int.Parse(customerIdLoggedIn));

            var totalNumberOfPages = Math.Ceiling((decimal)totalNumberOfOrdersServiceResult.Result / pageSize);
            ViewData["OrderCount"] = (int)totalNumberOfPages;
            ViewData["OrderStatus"] = orderStatus;

            var orderViewModels = new List<OrderViewModel>();
            foreach (var order in categorizedOrdersServiceResult.Result) BuildOrderViewModels(order.OrderDetails, orderViewModels, order);

            return PartialView("OrdersPartialView", orderViewModels);
        }

        public IActionResult Sort(string status)
        {
            return RedirectToAction(nameof(PageOrders), new { orderStatus = status });
        }

        private void BuildOrderViewModels(
            IEnumerable<OrderDetailModel> orderDetailServiceResult,
            List<OrderViewModel> orderViewModels,
            OrderModel order)
        {            
            var orderDetails = orderDetailServiceResult.Select(orderDetail => new OrderDetailViewModel(orderDetail)).ToList();
            orderViewModels.Add(new OrderViewModel(order, orderDetails));          
        }
    }
}
