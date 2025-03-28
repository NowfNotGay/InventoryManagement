using Core.BaseClass;
using Core.ProductClassification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.ProductClassification;
public interface IBrandProvider
{
    Task<ResultService<Brand>> SaveByDapper(Brand entity);
    Task<ResultService<string>> DeleteByDapper(string brandCode);
    


}
