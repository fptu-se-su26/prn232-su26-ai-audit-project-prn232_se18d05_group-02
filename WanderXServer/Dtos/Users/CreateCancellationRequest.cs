using System.ComponentModel.DataAnnotations;

namespace WanderXServer.Dtos.Users;

public class CreateCancellationRequest
{
    [StringLength(1000)]
    public string? Reason { get; set; }
}
