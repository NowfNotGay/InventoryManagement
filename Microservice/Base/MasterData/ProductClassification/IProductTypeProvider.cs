﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.BaseClass;
using Core.MasterData.ProductClassification;

namespace Base.MasterData.ProductClassification;
public interface IProductTypeProvider
{
    public Task<ResultService<ProductType>> SaveByDapper(ProductType entity);

    public Task<ResultService<string>> DeleteByDapper(string code);

    public Task<ResultService<ProductType>> GetByCode(string productTypeCode);
}
