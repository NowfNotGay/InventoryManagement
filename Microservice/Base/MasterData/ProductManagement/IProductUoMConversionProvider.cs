using Core.BaseClass;
using Core.ProductManagement;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.ProductManagement;
public interface IProductUoMConversionProvider
{
    Task<ResultService<ProductUoMConversionParam>> SaveByDapper(ProductUoMConversion entity);
    Task<ResultService<ProductUoMConversion>> GetByCode(string productCode);
    Task<ResultService<string>> DeleteByDapper(string productCode);

    Task<ResultService<IEnumerable<ProductUoMConversionParam>>> GetAllDapper();
}
