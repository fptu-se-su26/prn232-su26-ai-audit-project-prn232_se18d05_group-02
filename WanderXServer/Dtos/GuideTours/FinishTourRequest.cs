using System.ComponentModel.DataAnnotations;

namespace WanderXServer.Dtos.GuideTours;

public class FinishTourRequest
{
    [Required(ErrorMessage = "Evidence image is required.")]
    public string EvidenceImage { get; set; } = string.Empty;
}
