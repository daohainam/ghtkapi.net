using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ghtk.Repository.Abstractions.Entities;
public class Order
{
    public string Id { get; set; } = default!;
    public string TrackingId { get; set; } = default!;
    public string PartnerId { get; set; } = default!;
    public string PickName { get; set; } = default!;
    public string PickAddress { get; set; } = default!;
    public string PickProvince { get; set; } = default!;    
    public string PickDistrict { get; set; } = default!;
    public string PickWard { get; set; } = default!;
    public string PickTel { get; set; } = default!;
    public string Tel { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string Province { get; set; } = default!;
    public string District { get; set; } = default!;
    public string Ward { get; set; } = default!;
    public string Hamlet { get; set; } = default!;
    public int IsFreeship { get; set; }
    public DateTimeOffset PickDate { get; set; }
    public long PickMoney { get; set; }
    public string Note { get; set; } = default!;
    public long Value { get; set; }
    public string Transport { get; set; } = default!;
    public string PickOption { get; set; } = default!;
    public string DeliverOption { get; set; } = default!;
    public int Status { get; set; }
    public List<Product> Products { get; set; } = [];
}
