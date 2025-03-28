using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.BaseClass;
using Core.WarehouseManagement;


namespace Base.WarehouseManagement;
public interface IGoodsIssueNoteProvider
{
    Task<ResultService<GoodsIssueNote>> SaveByDapper(GoodsIssueNote entity);
    Task<ResultService<string>> DeleteByDapper(string ginCode);
    Task<ResultService<GoodsIssueNote_Param>> SaveHeaderAndLine(GoodsIssueNote_Param entity);
    Task<ResultService<string>> DeleteLine(List<GoodsIssueNoteLine> entity);
    Task<ResultService<IEnumerable<GoodsIssueNoteLine>>> GetLineByRefCode(string ginCode);
    Task<ResultService<string>> Delete_HeaderAndDetail(int ginID);

}
