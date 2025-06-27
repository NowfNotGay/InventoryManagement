using Core.BaseClass;
using Core.WarehouseManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.WarehouseManagement;
public interface IWarehouseProvider
{
    Task<ResultService<Warehouse>> SaveByDapper(Warehouse entity);
    Task<ResultService<Warehouse>> GetByCode(string warehouseCode);
    Task<ResultService<string>> DeleteByDapper(string warehouseCode);

}
