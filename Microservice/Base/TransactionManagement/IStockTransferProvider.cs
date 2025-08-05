using Core.BaseClass;
using Core.TransactionManagement;

namespace Base.TransactionManagement
{
    public interface IStockTransferProvider
    {
        Task<ResultService<StockTransfer>> CreateByDapper(StockTransfer entity);
        Task<ResultService<string>> DeleteByDapper(string stCode);
        Task<ResultService<StockTransfer_Param>> CreateHeaderAndDetail(StockTransfer_Param entity);
        Task<ResultService<string>> DeleteDetail(List<StockTransferDetail> entity);
        Task<ResultService<IEnumerable<StockTransferDetail>>> GetDetailByStockTransferID(string stCode);
        Task<ResultService<string>> Delete_HeaderAndDetail(int stID);
        Task<ResultService<StockTransfer_Param>> Save(StockTransfer_Param entity);
    }
}
