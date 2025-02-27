﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ProductProperties;
public class Dimension : BaseClass.BaseClass
{
    public decimal? Height { get; set; }
    public decimal? Length { get; set; }
    public decimal? Width { get; set; }
    public string UoMName { get; set; } = "cm";
}
