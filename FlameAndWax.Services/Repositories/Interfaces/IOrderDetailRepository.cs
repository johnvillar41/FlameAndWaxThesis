﻿using FlameAndWax.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories.Interfaces
{
    public interface IOrderDetailRepository : IBaseRepository<OrderDetailModel>
    {
        Task<IEnumerable<OrderDetailModel>> FetchOrderDetailsAsync(int orderId, string connectionString);        
    }
}
