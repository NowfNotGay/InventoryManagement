using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.BaseClass;
using Core.MasterData;
using Core.MasterData.ProductClassification;

namespace Base.MasterData.ProductClassification;
public interface IProductCategoryProvider
{
    public Task<ResultService<ProductCategory>> SaveByDapper(ProductCategory entity);

    public Task<ResultService<string>> DeleteByDapper(string code);

    public Task<ResultService<ProductCategory>> GetByCode(string code);
}
