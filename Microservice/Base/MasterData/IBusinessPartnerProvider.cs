using Core.BaseClass;
using Core.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.MasterData;
public interface IBusinessPartnerProvider
{
    public Task<ResultService<BusinessPartner>> SaveByDapper(BusinessPartner entity);

    public Task<ResultService<string>> DeleteByDapper(string partnerCode);
}
