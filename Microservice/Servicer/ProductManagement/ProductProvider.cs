using Base.BaseService;
using Base.ProductManagement;
using Core.BaseClass;
using Core.ProductManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicer.ProductManagement;
public class ProductProvider : ICRUD_Service<Product, int>, IProductProvider
{
    public Task<ResultService<Product>> Create(Product entity)
    {
        throw new NotImplementedException();
    }

    public Task<ResultService<string>> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ResultService<Product>> Get(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ResultService<IEnumerable<Product>>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<ResultService<Product>> Update(Product entity)
    {
        throw new NotImplementedException();
    }
}
