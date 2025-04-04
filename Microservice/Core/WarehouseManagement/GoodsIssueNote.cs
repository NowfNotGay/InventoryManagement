﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.WarehouseManagement;
public class GoodsIssueNote : BaseClass.BaseClass
{
    public string GINCode { get; set; }
    public int WarehouseID { get; set; }
    public int CustomerID { get; set; }
    public int TransactionTypeID { get; set; }
    public DateTime IssueDate { get; set; } = DateTime.Now;
    public string Notes { get; set; }
}

public class GoodsIssueNoteLine : BaseClass.BaseClass
{
    public string RefGINCode { get; set; }
    public int ProductID { get; set; }
    public int ProductVariantID { get; set; }
    public int UoMID { get; set; }
    public decimal Quantity { get; set; }
    public int UoMConversionID { get; set; }
    public decimal ConvertedQuantity { get; set; }
    public int StorageBinID { get; set; }
}
public class GoodsIssueNote_Param
{
    public string CreatedBy { get; set; }
    public List<GoodsIssueNote> GINs { get; set; }
    public List<GoodsIssueNoteLine> GINLines { get; set; }
}
