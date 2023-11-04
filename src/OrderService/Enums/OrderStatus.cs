using System.Runtime.Serialization;

namespace OrderService.Enums;

public enum OrderStatus
{
    [EnumMember(Value = "Pending")]
    Pending,
    [EnumMember(Value = "Shipped")]
    Shipped,
    [EnumMember(Value = "Cancelled")]
    Cancelled,
    [EnumMember(Value = "Delivered")]
    Delivered
}
