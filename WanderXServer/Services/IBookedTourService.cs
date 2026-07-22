using WanderXServer.Dtos.BookedTours;

namespace WanderXServer.Services;

public interface IBookedTourService
{
    Task<IReadOnlyList<BookedTourResponse>> GetBookedToursAsync(BookedTourFilterRequest filter);
}
