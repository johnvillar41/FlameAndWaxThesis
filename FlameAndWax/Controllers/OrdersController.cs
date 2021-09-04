﻿using FlameAndWax.Data.Constants;
using FlameAndWax.Models;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FlameAndWax.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IConfiguration _configuration;
        private string ConnectionString { get; set; }

        public OrdersController(ICustomerService customerService, IConfiguration configuration)
        {
            _customerService = customerService;
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("FlameAndWaxDBConnection");
        }

        public async Task<IActionResult> Index()
        {
            var customerIdLoggedIn = User.Claims.FirstOrDefault(userId => userId.Type == ClaimTypes.NameIdentifier).Value;
            var ordersServiceResult = await _customerService.FetchOrders(int.Parse(customerIdLoggedIn), ConnectionString);

            if (ordersServiceResult.HasError)
                return PartialView("Error", new ErrorViewModel { ErrorContent = ordersServiceResult.ErrorContent });

            var orderViewModels = new List<OrderViewModel>();
            foreach (var order in ordersServiceResult.Result)
            {
                var orderDetailsServiceResult = await _customerService.FetchOrderDetails(order.OrderId, ConnectionString);
                if (orderDetailsServiceResult.HasError)
                    return View("Error", new ErrorViewModel { ErrorContent = orderDetailsServiceResult.ErrorContent });

                var orderDetails = new List<OrderDetailViewModel>();
                foreach (var orderDetail in orderDetailsServiceResult.Result)
                {
                    orderDetails.Add(
                            new OrderDetailViewModel
                            {
                                ProductId = orderDetail.Product.ProductId,
                                ProductPictureLink = orderDetail.Product.ProductGallery.FirstOrDefault().PhotoLink,
                                ProductQuantityOrdered = orderDetail.Quantity,
                                SubTotalPrice = orderDetail.TotalPrice,
                                Status = orderDetail.Status
                            }
                        );
                }

                orderViewModels.Add(
                        new OrderViewModel
                        {
                            OrderId = order.OrderId,
                            Date = order.DateOrdered,
                            ModeOfPayment = order.ModeOfPayment,
                            Courier = order.Courier,
                            TotalCost = order.TotalCost,
                            Status = order.Status,
                            OrderDetails = orderDetails
                        }
                    );
            }
            return View(orderViewModels);
        }

        public async Task<IActionResult> Sort(string status)
        {
            var customerIdLoggedIn = User.Claims.FirstOrDefault(userId => userId.Type == ClaimTypes.NameIdentifier).Value;
            var categorizedOrdersServiceResult = await _customerService.FetchOrdersByStatus(int.Parse(customerIdLoggedIn), ServiceHelper.ConvertStringtoOrderStatus(status), ConnectionString);

            if (categorizedOrdersServiceResult.HasError)
                return PartialView("Error", new ErrorViewModel { ErrorContent = categorizedOrdersServiceResult.ErrorContent });

            var orderViewModels = new List<OrderViewModel>();
            foreach (var order in categorizedOrdersServiceResult.Result)
            {
                var orderDetailsServiceResult = await _customerService.FetchOrderDetails(order.OrderId, ConnectionString);
                if (orderDetailsServiceResult.HasError)
                    return View("Error", new ErrorViewModel { ErrorContent = categorizedOrdersServiceResult.ErrorContent });

                var orderDetails = new List<OrderDetailViewModel>();
                foreach (var orderDetail in orderDetailsServiceResult.Result)
                {
                    orderDetails.Add(
                            new OrderDetailViewModel
                            {
                                ProductId = orderDetail.Product.ProductId,
                                ProductPictureLink = orderDetail.Product.ProductGallery.FirstOrDefault().PhotoLink,
                                ProductQuantityOrdered = orderDetail.Quantity,
                                SubTotalPrice = orderDetail.TotalPrice,
                                Status = orderDetail.Status
                            }
                        );
                }

                orderViewModels.Add(
                        new OrderViewModel
                        {
                            OrderId = order.OrderId,
                            Date = order.DateOrdered,
                            ModeOfPayment = order.ModeOfPayment,
                            Courier = order.Courier,
                            TotalCost = order.TotalCost,
                            Status = order.Status,
                            OrderDetails = orderDetails
                        }
                    );
            }
            return PartialView(orderViewModels);

            // For order details
            //var orderDetails = new List<OrderDetailViewModel>();
            //foreach (var orderDetail in categorizedOrdersServiceResult.Result)
            //{
            //    var orderDetailsServiceResult = await _customerService.FetchOrderDetails(orderDetail.OrderId, ConnectionString);
            //    if (orderDetailsServiceResult.HasError)
            //        return View("Error", new ErrorViewModel { ErrorContent = orderDetailsServiceResult.ErrorContent });

            //    orderDetails.Add(
            //            new OrderDetailViewModel
            //            {
            //                ProductId = orderDetail.,
            //                ProductPictureLink = orderDetail.Product.ProductGallery.FirstOrDefault().PhotoLink,
            //                ProductQuantityOrdered = orderDetail.Quantity,
            //                SubTotalPrice = orderDetail.TotalPrice,
            //                Status = orderDetail.Status
            //            }
            //        );
            //}

            //// For entire orders
            //var orders = new List<OrderViewModel>();
            //foreach (var order in categorizedOrdersServiceResult.Result)
            //{
            //    orders.Add(new OrderViewModel
            //    {
            //        OrderId = order.OrderId,
            //        Date = order.DateOrdered,
            //        TotalCost = order.TotalCost,
            //        ModeOfPayment = order.ModeOfPayment,
            //        Courier = order.Courier,
            //        Status = order.Status,
            //        OrderDetails = orderDetails
            //    });

            //}

            //return PartialView("ProductsPartial", orders);
        }
    }
}
