using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.CompilerServices.RuntimeHelpers;
namespace Core.WarehouseManagement
{
    public class CurrentStock : BaseClass.BaseClass
    {
        public int ProductID { get; set; } = 0;
        public int? ProductVariantID { get; set; }

        public int UoMID { get; set; } = 0;

        public decimal Quantity { get; set; } = 0;

        public int WarehouseID { get; set; } = 0;

        public int? StorageBinID { get; set; }

    }
}
