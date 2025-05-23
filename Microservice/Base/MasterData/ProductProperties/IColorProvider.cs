using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.BaseClass;
using Core.MasterData.ProductProperties;

namespace Base.MasterData.ProductProperties;
public interface IColorProvider
{
    Task<ResultService<Color>> SaveByDapper(Color entity);
    Task<ResultService<string>> DeleteByDapper(string code);
}
