using Core.BaseClass;
using Core.WarehouseManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.WarehouseManagement;
public interface IInventoryTransactionProvider
{
    Task<ResultService<InventoryTransaction>> SaveByDapper(InventoryTransaction entity);
    Task<ResultService<InventoryTransaction>> GetByCode(string inventoryTransactionCode);
    Task<ResultService<string>> DeleteByDapper(string inventoryTransactionCode);
}
