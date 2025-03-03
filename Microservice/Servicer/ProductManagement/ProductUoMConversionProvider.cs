using Base.BaseService;
using Base.ProductManagement;
using Context.ProductProperties;
using Core.BaseClass;
using Core.ProductManagement;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicer.ProductManagement;
public class ProductUoMConversionProvider : ICRUD_Service<ProductUoMConversion, int>, IProductUoMConversionProvider
{
    //private readonly DB_ProductManagement_Context _dB;
    private readonly IConfiguration _configuration;
    public async Task<ResultService<ProductUoMConversion>> Create(ProductUoMConversion entity)
    {
        throw new NotImplementedException();
    }
    public async Task<ResultService<string>> Delete(int id)
    {
        throw new NotImplementedException();
    }
    public async Task<ResultService<ProductUoMConversion>> Get(int id)
    {
        throw new NotImplementedException();
    }
    public async Task<ResultService<IEnumerable<ProductUoMConversion>>> GetAll()
    {
        throw new NotImplementedException();
    }
    public async Task<ResultService<ProductUoMConversion>> Update(ProductUoMConversion entity)
    {
        throw new NotImplementedException();
    }

    
}

