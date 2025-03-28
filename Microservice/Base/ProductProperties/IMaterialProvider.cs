using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.BaseClass;
using Core.ProductProperties;

namespace Base.ProductProperties;
public interface IMaterialProvider
{
    Task<ResultService<Material>> SaveByDapper(Material entity);
    Task<ResultService<string>> DeleteByDapper(string code);
}
