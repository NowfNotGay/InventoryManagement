﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.BaseClass;
using Core.WarehouseManagement;

namespace Base.WarehouseManagement
{
    public interface IGoodsReceiptNoteProvider
    {
        Task<ResultService<GoodsReceiptNote>> CreateByDapper(GoodsReceiptNote entity);
        Task<ResultService<string>> DeleteByDapper(string grnCode);
        Task<ResultService<GoodsReceiptNote_Param>> CreateHeaderAndLine(GoodsReceiptNote_Param entity);
        Task<ResultService<string>> DeleteLine(List<GoodsReceiptNoteLine> entity);
        Task<ResultService<IEnumerable<GoodsReceiptNoteLine>>> GetLineByRefCode(string GRNcode);

        Task<ResultService<string>> Delete_HeaderAndDetail(int grnID);
    }
}
