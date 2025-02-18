using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MasterData;
[Table("StatusMaster")]
public class StatusMaster:BaseClass.BaseClass
{
    public string StatusCode{ get; set; } = string.Empty;
    public string StatusName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
