using Core.BaseClass;
using Core.TransactionManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.TransactionManagement;
public interface IInventoryTransactionProvider
{
    Task<ResultService<InventoryTransaction>> GetByCode(string inventoryTransactionCode);
    Task<ResultService<string>> DeleteByDapper(string inventoryTransactionCode);
}
