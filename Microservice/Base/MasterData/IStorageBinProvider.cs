using Core.BaseClass;
using Core.MasterData;
using Core.MasterData.ProductProperties;

namespace Base.MasterData;
public interface IStorageBinProvider
{
    Task<ResultService<StorageBin>> SaveByDapper(StorageBin entity);
    Task<ResultService<string>> DeleteByDapper(string code);

    Task<ResultService<IEnumerable<StorageBin>>> GetAllByWarehouseCode (string code);
}
