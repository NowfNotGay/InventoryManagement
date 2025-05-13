using Core.BaseClass;
using Core.MasterData.ProductClassification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.MasterData.ProductClassification;
public interface IVehicleModelProvider
{
    Task<ResultService<VehicleModel>> SaveByDapper(VehicleModel entity);
    Task<ResultService<VehicleModel>> GetByCode(string modelCode);
    Task<ResultService<string>> DeleteByDapper(string modelCode);
}
