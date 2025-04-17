using Core.BaseClass;
using Core.ProductProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.ProductProperties;
public interface IUnitOfMeasureProvider 
{
    Task<ResultService<UnitOfMeasure>> SaveByDapper(UnitOfMeasure entity);
    Task<ResultService<UnitOfMeasure>> GetByCode(string uomCode);
    Task<ResultService<string>> DeleteByDapper(string uomCode);
}
