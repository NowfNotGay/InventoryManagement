using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.TransactionManagement;
public class GoodsIssueNote : BaseClass.BaseClass
{
    public string GINCode { get; set; }
    public string WarehouseCode { get; set; }
    public string CustomerCode { get; set; }
    public string TransactionTypeCode { get; set; }
    public DateTime IssueDate { get; set; } = DateTime.Now;
    public string Notes { get; set; }
}

public class GoodsIssueNoteLine : BaseClass.BaseClass
{
    public string RefGINCode { get; set; }
    public string ProductCode { get; set; }
    public string ProductVariantCode { get; set; }
    public string UoMCode { get; set; }
    public decimal Quantity { get; set; }
    public string UoMConversionCode { get; set; }
    public decimal ConvertedQuantity { get; set; }
    public string StorageBinCode { get; set; }
}
public class GoodsIssueNote_Param
{
    public string CreatedBy { get; set; }
    public List<GoodsIssueNote> GINs { get; set; }
    public List<GoodsIssueNoteLine> GINLines { get; set; }
}
