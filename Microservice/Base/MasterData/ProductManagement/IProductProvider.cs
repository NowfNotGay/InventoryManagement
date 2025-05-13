using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.BaseClass;
using Core.ProductManagement;

namespace Base.ProductManagement;
public interface IProductProvider
{
    Task<ResultService<IEnumerable<ProductParam>>> GetAllProductParam();
    Task<ResultService<ProductParam>> GetByIDParam(int id);
    Task<ResultService<Product>> SaveByDapper(Product entity);
    Task<ResultService<string>> DeleteByDapper(string code);
}
