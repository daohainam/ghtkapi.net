using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ghtk.Repository.Abstractions.Entities;
public class Product
{
    public string Name { get; set; } = default!;
    public double Weight { get; set; }
    public long Quantity { get; set; }
    public long ProductCode { get; set; }
}