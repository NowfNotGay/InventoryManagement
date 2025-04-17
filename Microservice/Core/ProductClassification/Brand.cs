﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ProductClassification;
[Table("Brand")]

public class Brand : BaseClass.BaseClass
{
    public string BrandCode { get; set; }
    public string BrandName { get; set; }
}


