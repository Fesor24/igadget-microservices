using System.Runtime.Serialization;

namespace OrderService.Enums;

public enum PaymentStatus
{
    [EnumMember(Value = "Pending")]
    Pending,
    [EnumMember(Value = "Successful")]
    Successful,
    [EnumMember(Value = "Failed")]
    Failed
}
