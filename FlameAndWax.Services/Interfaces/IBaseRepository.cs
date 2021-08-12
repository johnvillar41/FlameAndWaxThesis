﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlameAndWax.Data.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task Add(T Data);
        Task Delete(int id);
        Task Update(T data, int id);
        Task<T> Fetch(int id);
        Task<IEnumerable<T>> FetchAll();
    }
}
