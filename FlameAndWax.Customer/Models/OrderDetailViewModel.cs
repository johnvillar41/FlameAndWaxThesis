﻿using FlameAndWax.Data.Models;
using System.Linq;
using static FlameAndWax.Data.Constants.Constants;

namespace FlameAndWax.Customer.Models
{
    public class OrderDetailViewModel
    {
        public int ProductId { get; set; }
        public string ProductPictureLink { get; set; }
        public int ProductQuantityOrdered { get; set; }
        public double SubTotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public OrderDetailViewModel(OrderDetailModel orderDetailModel)
        {
            ProductId = orderDetailModel.Product.ProductId;
            ProductPictureLink = orderDetailModel.Product.ProductGallery.FirstOrDefault().ProductPhotoLink;
            SubTotalPrice = orderDetailModel.TotalPrice;
            Status = orderDetailModel.Status;
            ProductQuantityOrdered = orderDetailModel.Quantity;
        }
        public OrderDetailViewModel()
        {

        }
    }
}