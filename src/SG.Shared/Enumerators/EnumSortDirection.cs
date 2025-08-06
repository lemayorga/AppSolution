using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SG.Shared.Enumerators;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EnumSortDirection
{
  ASC,
  DESC
}