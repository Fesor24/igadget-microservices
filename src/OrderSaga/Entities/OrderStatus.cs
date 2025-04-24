using System.Runtime.Serialization;

namespace OrderSaga.Entities;

public enum OrderStatus
{
    [EnumMember(Value = "Pending")]
    Pending,
    [EnumMember(Value = "Paid")]
    Paid,
    [EnumMember(Value = "Shipped")]
    Shipped,
    [EnumMember(Value = "Cancelled")]
    Cancelled,
    [EnumMember(Value = "CancellationRequested")]
    CancellationRequested,
    [EnumMember(Value = "Delivered")]
    Delivered
}