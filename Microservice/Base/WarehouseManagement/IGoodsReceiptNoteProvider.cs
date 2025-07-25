﻿using Core.BaseClass;
using Core.WarehouseManagement;

namespace Base.WarehouseManagement
{
    public interface IGoodsReceiptNoteProvider
    {
        Task<ResultService<GoodsReceiptNote>> SaveByDapper(GoodsReceiptNote entity);
        Task<ResultService<string>> DeleteByDapper(string grnCode);
        //Task<ResultService<GoodsReceiptNote_Param>> CreateHeaderAndLine(GoodsReceiptNote_Param entity);
        Task<ResultService<GoodsReceiptNote_Param>> Save(GoodsReceiptNote_Param entity);
        Task<ResultService<string>> GoodReceiptNoteLine_Save(GoodsReceiptNoteLine entity); 
        Task<ResultService<string>> GoodsReceiptNoteLine_Delete_Multi_Line(List<GoodsReceiptNoteLine> entity);
        Task<ResultService<string>> GoodsReceiptNoteLine_Delete_SingleLine(Guid RowPointer);
        Task<ResultService<IEnumerable<GoodsReceiptNoteLine>>> GetLineByRefCode(string GRNcode);

        Task<ResultService<string>> Delete_HeaderAndDetail(string grnCode);
    }
}
