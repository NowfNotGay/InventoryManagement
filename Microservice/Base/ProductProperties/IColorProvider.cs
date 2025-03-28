using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.BaseClass;
using Core.ProductClassification;
using Core.ProductProperties;

namespace Base.ProductProperties;
public interface IColorProvider
{
    Task<ResultService<Color>> SaveByDapper(Color entity);
    Task<ResultService<string>> DeleteByDapper(string code);
}
