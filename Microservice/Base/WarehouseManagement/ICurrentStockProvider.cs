using Core.BaseClass;
using Core.ProductClassification;
using Core.WarehouseManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.WarehouseManagement
{
    public interface ICurrentStockProvider
    {
        Task<ResultService<CurrentStock>> Save(UDTT_CurrentStock entity);
    }
}
