using Core.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.BaseService
{
    public interface ICRUD_Service<T,U>
    {
        Task<T> Create(T entity);
        Task<T> Update(T entity);
        Task<string> Delete(U id);
        Task<T> Get(U id);
        Task<IEnumerable<T>> GetAll();
    }

    public interface ICRUD_Service_V2<T, U>
    {
        Task<ResultService<T>> Create(T entity);
        Task<ResultService<T>> Update(T entity);
        Task<ResultService<string>> Delete(U id);
        Task<ResultService<T>> Get(U id);
        Task<ResultService<IEnumerable<T>>> GetAll();
    }
}
