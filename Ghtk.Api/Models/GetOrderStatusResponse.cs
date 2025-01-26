namespace Ghtk.Api.Models;

using System;
using System.Text.Json.Serialization;

public class GetOrderStatusResponse: ApiResult
{
    [JsonPropertyName("order")]
    public StatusOrder Order { get; set; } = default!;
}

public class StatusOrder
{
    [JsonPropertyName("label_id")]
    public string LabelId { get; set; } = default!;

    [JsonPropertyName("partner_id")]
    public string PartnerId { get; set; } = default!;

    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("status_text")]
    public string StatusText { get; set; } = default!;

    [JsonPropertyName("created")]
    public DateTimeOffset Created { get; set; }

    [JsonPropertyName("modified")]
    public DateTimeOffset Modified { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = default!;

    [JsonPropertyName("pick_date")]
    public DateTimeOffset PickDate { get; set; }

    [JsonPropertyName("deliver_date")]
    public DateTimeOffset DeliverDate { get; set; }

    [JsonPropertyName("customer_fullname")]
    public string CustomerFullname { get; set; } = default!;

    [JsonPropertyName("customer_tel")]
    public string CustomerTel { get; set; } = default!;

    [JsonPropertyName("address")]
    public string Address { get; set; } = default!;

    [JsonPropertyName("storage_day")]
    public int StorageDay { get; set; }

    [JsonPropertyName("ship_money")]
    public int ShipMoney { get; set; }

    [JsonPropertyName("insurance")]
    public int Insurance { get; set; }

    [JsonPropertyName("value")]
    public long Value { get; set; }

    [JsonPropertyName("weight")]
    public int Weight { get; set; }

    [JsonPropertyName("pick_money")]
    public int PickMoney { get; set; }

    [JsonPropertyName("is_freeship")]
    public int IsFreeship { get; set; }
}
