using System.ComponentModel.DataAnnotations;

namespace WanderXServer.Dtos.GuideTours;

public class DeclineTourRequest
{
    [Required]
    [StringLength(600, MinimumLength = 10)]
    public string Reason { get; set; } = string.Empty;
}
