using Core.BaseClass;

namespace Base.BaseService
{
    public interface ICRUD_Service<T, U>
    {
        Task<ResultService<T>> Create(T entity);
        Task<ResultService<T>> Update(T entity);
        Task<ResultService<string>> Delete(U id);
        Task<ResultService<T>> Get(U id);
        Task<ResultService<IEnumerable<T>>> GetAll();
        Task<ResultService<string>> Save(T entity);

    }

}
