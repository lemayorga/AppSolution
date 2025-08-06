using System.Text.Json.Serialization;

namespace SG.Shared.Enumerators;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EnumOperatorFilter
{
    Eq,
    Neq,
    Contains,
    Startswith,
    Endswith,
    Gt,
    Gte,
    Lt,
    Lte
}
