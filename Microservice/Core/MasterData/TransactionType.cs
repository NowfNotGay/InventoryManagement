using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MasterData
{
    public class TransactionType : BaseClass.BaseClass
    {
        public string TransactionTypeCode { get; set; }
        public string TransactionTypeName { get; set; }
        public int DocumentTypeID { get; set; }
        public string? Description { get; set; }
    }
}
