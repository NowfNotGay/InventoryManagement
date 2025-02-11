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
}
