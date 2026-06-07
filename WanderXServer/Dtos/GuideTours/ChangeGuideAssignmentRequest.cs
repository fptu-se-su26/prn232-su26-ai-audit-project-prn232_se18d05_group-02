using System.ComponentModel.DataAnnotations;

namespace WanderXServer.Dtos.GuideTours;

public class ChangeGuideAssignmentRequest
{
    [Required(ErrorMessage = "Guide profile is required.")]
    public Guid GuideProfileId { get; set; }
}
