﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.WarehouseManagement;
public class InventoryTransaction : BaseClass.BaseClass
{

        public string InventoryTransactionCode { get; set; }
        public int ProductID { get; set; }
        public int? ProductVariantID { get; set; }
        public int UoMID { get; set; }
        public decimal Quantity { get; set; }
        public int WarehouseID { get; set; }
        public int? StorageBinID { get; set; }
        public int TransactionTypeID { get; set; }
        public DateTime TransactionDate { get; set; }
        public int? ReferenceID { get; set; }
        public string? ReferenceType { get; set; }
        public string? Notes { get; set; }

}
public class InventoryTransactionParam : BaseClass.BaseClass
{
    public string InventoryTransactionCode { get; set; }


    //Product
    public string ProductCode { get; set; }
    public string ProductName { get; set; }


    //ProductVariant
    public string ProductVariantCode { get; set; }
    public string? Attributes { get; set; }


    //UoM
    public string UoMCode { get; set; } = string.Empty;
    public string UoMName { get; set; } = string.Empty;        
    

    public decimal Quantity { get; set; }


    //Warehouse
    public string WarehouseCode { get; set; }
    public string WarehouseName { get; set; }


    //StorageBin
    public string StorageBinCode { get; set; }
    public string StorageBinName { get; set; }


    //TransactionType
    public string TransactionTypeCode { get; set; }
    public string TransactionTypeName { get; set; }


    public DateTime TransactionDate { get; set; }
    public int? ReferenceID { get; set; }
    public string? ReferenceType { get; set; }
    public string? Notes { get; set; }
}