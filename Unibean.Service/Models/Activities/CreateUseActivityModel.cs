using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Type = Unibean.Repository.Entities.Type;

namespace Unibean.Service.Models.Activities;

public class CreateUseActivityModel
{
    [JsonIgnore]
    public Type Type { get; set; } = Type.Use;

    public string Description { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? State { get; set; }    
}
