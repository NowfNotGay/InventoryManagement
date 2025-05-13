using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.BaseClass;
using Core.MasterData.ProductProperties;

namespace Base.MasterData.ProductProperties;
public interface IMaterialProvider
{
    Task<ResultService<Material>> SaveByDapper(Material entity);
    Task<ResultService<string>> DeleteByDapper(string code);
}
