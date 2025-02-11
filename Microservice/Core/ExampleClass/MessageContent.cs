using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ExampleClass
{
    [Table("MessageContents")]
    public class MessageContent: BaseClass.BaseClass
    {
        public string? DocType { get; set; } = string.Empty;
        public string? Title { get; set; } = string.Empty;
        public string? Content { get; set; } = string.Empty;
        public int? EventHandleID { get; set; }
        public int? MessageTemplateID { get; set; }
        public bool? IsRead { get; set; }
        public int? Priority { get; set; }
    }
}
