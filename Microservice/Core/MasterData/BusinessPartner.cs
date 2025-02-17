using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MasterData;
[Table("BusinessPartner")]
public class BusinessPartner:BaseClass.BaseClass
{
    public string PartnerCode { get; set; } = string.Empty;
    public string PartnerName { get; set; } = string.Empty;
    public bool IsSupplier { get; set; } = false;
    public bool IsCustomer { get; set; } = false;
    public string ContactInfo { get; set; } = string.Empty;
    public int StatusID { get; set; }

}
