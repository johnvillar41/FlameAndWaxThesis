﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories.Interfaces
{
    public interface IAddBaseInterface<T>
    {
        Task<int> AddAsync(T Data, string connectionString);
    }
    public interface IDeleteBaseInterface
    {
        Task DeleteAsync(int id, string connectionString);
    }
    public interface IUpdateBaseInterface<T>
    {
        Task UpdateAsync(T data, int id, string connectionString);
    }
    public interface IFetchBaseInterface<T>
    {
        Task<T> FetchAsync(int id, string connectionString);
    }
    public interface IBaseRepository<T> : IAddBaseInterface<T>, IDeleteBaseInterface, IUpdateBaseInterface<T>,IFetchBaseInterface<T> where T : class
    {
        Task<IEnumerable<T>> FetchPaginatedResultAsync(int pageNumber, int pageSize, string connectionString);
    }
}
