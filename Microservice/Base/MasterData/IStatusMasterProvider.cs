using Core.BaseClass;
using Core.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.MasterData;
public interface IStatusMasterProvider
{
    //Task<ResultService<StatusMaster>> SaveByDapper(StatusMaster entity);
    Task<ResultService<StatusMaster>> GetByCode(string statusCode);
    Task<ResultService<string>> DeleteByDapper(string statusCode);
}
