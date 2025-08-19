using Core.BaseClass;
using Core.WarehouseManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.WarehouseManagement;
public interface ICurrentStockProvider
{
    Task<ResultService<IEnumerable<CurrentStockParam>>> GetAllCurrentStockByWarehouse(string warehouseCode);
}
