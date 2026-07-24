using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WanderXServer.BusinessObject;
using WanderXServer.DataAccessLayer;
using WanderXServer.ViewModels.TourSchedules;

namespace WanderXServer.Services;

public class TourScheduleService : ITourScheduleService
{
    private readonly WanderXDbContext _dbContext;

    public TourScheduleService(WanderXDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TourScheduleIndexViewModel> GetIndexAsync(TourScheduleFilterViewModel filter)
    {
        var schedules = _dbContext.TourSchedules
            .Include(schedule => schedule.Tour)
            .AsNoTracking()
            .AsQueryable();

        if (filter.TourId is int tourId)
        {
            schedules = schedules.Where(schedule => schedule.TourId == tourId);
        }

        if (filter.FilterDate is DateTime date)
        {
            schedules = schedules.Where(schedule => schedule.ScheduleDate.Date == date.Date);
        }

        if (filter.Month is int month and >= 1 and <= 12)
        {
            schedules = schedules.Where(schedule => schedule.ScheduleDate.Month == month);
        }

        if (filter.Year is int year and > 0)
        {
            schedules = schedules.Where(schedule => schedule.ScheduleDate.Year == year);
        }

        var rows = await schedules
            .OrderBy(schedule => schedule.ScheduleDate)
            .ThenBy(schedule => schedule.DayNumber)
            .ThenBy(schedule => schedule.SortOrder)
            .Select(schedule => new TourScheduleRowViewModel
            {
                Id = schedule.Id,
                TourId = schedule.TourId,
                TourName = schedule.Tour.Name,
                DayNumber = schedule.DayNumber,
                ScheduleDate = schedule.ScheduleDate,
                Title = schedule.Title,
                Location = schedule.Location,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                SortOrder = schedule.SortOrder
            })
            .ToListAsync();

        filter.Tours = await GetTourOptionsAsync(filter.TourId);

        return new TourScheduleIndexViewModel
        {
            Filter = filter,
            Schedules = rows
        };
    }

    public async Task<TourScheduleDetailsViewModel> GetDetailsAsync(int id)
    {
        var schedule = await FindScheduleAsync(id);
        return ToDetails(schedule);
    }

    public async Task<TourScheduleFormViewModel> CreateFormAsync(int? tourId = null)
    {
        var sortOrder = 1;

        if (tourId is int selectedTourId)
        {
            sortOrder = await GetNextSortOrderAsync(selectedTourId);
        }

        return new TourScheduleFormViewModel
        {
            TourId = tourId ?? 0,
            SortOrder = sortOrder,
            Tours = await GetTourOptionsAsync(tourId)
        };
    }

    public async Task<int> CreateAsync(TourScheduleFormViewModel model)
    {
        await EnsureTourExistsAsync(model.TourId);

        var schedule = new TourSchedule
        {
            TourId = model.TourId,
            DayNumber = model.DayNumber,
            ScheduleDate = model.ScheduleDate.Date,
            Title = model.Title.Trim(),
            Description = model.Description.Trim(),
            Location = model.Location.Trim(),
            StartTime = model.StartTime,
            EndTime = model.EndTime,
            SortOrder = model.SortOrder
        };

        _dbContext.TourSchedules.Add(schedule);
        await _dbContext.SaveChangesAsync();
        await NormalizeSortOrderAsync(model.TourId);

        return schedule.Id;
    }

    public async Task<TourScheduleFormViewModel> EditFormAsync(int id)
    {
        var schedule = await FindScheduleAsync(id);

        return new TourScheduleFormViewModel
        {
            Id = schedule.Id,
            TourId = schedule.TourId,
            DayNumber = schedule.DayNumber,
            ScheduleDate = schedule.ScheduleDate,
            Title = schedule.Title,
            Description = schedule.Description,
            Location = schedule.Location,
            StartTime = schedule.StartTime,
            EndTime = schedule.EndTime,
            SortOrder = schedule.SortOrder,
            Tours = await GetTourOptionsAsync(schedule.TourId)
        };
    }

    public async Task UpdateAsync(TourScheduleFormViewModel model)
    {
        await EnsureTourExistsAsync(model.TourId);

        var schedule = await FindScheduleAsync(model.Id);
        var previousTourId = schedule.TourId;

        schedule.TourId = model.TourId;
        schedule.DayNumber = model.DayNumber;
        schedule.ScheduleDate = model.ScheduleDate.Date;
        schedule.Title = model.Title.Trim();
        schedule.Description = model.Description.Trim();
        schedule.Location = model.Location.Trim();
        schedule.StartTime = model.StartTime;
        schedule.EndTime = model.EndTime;
        schedule.SortOrder = model.SortOrder;

        await _dbContext.SaveChangesAsync();
        await NormalizeSortOrderAsync(previousTourId);

        if (previousTourId != model.TourId)
        {
            await NormalizeSortOrderAsync(model.TourId);
        }
    }

    public async Task DeleteAsync(int id)
    {
        var schedule = await FindScheduleAsync(id);
        var tourId = schedule.TourId;

        _dbContext.TourSchedules.Remove(schedule);
        await _dbContext.SaveChangesAsync();
        await NormalizeSortOrderAsync(tourId);
    }

    public async Task MoveUpAsync(int id)
    {
        var schedule = await FindScheduleAsync(id);
        var previous = await GetOrderedSchedules(schedule.TourId)
            .Where(item =>
                item.ScheduleDate < schedule.ScheduleDate ||
                item.ScheduleDate == schedule.ScheduleDate && item.DayNumber < schedule.DayNumber ||
                item.ScheduleDate == schedule.ScheduleDate && item.DayNumber == schedule.DayNumber && item.SortOrder < schedule.SortOrder)
            .OrderByDescending(item => item.ScheduleDate)
            .ThenByDescending(item => item.DayNumber)
            .ThenByDescending(item => item.SortOrder)
            .FirstOrDefaultAsync();

        if (previous is null)
        {
            return;
        }

        (schedule.SortOrder, previous.SortOrder) = (previous.SortOrder, schedule.SortOrder);
        await _dbContext.SaveChangesAsync();
        await NormalizeSortOrderAsync(schedule.TourId);
    }

    public async Task MoveDownAsync(int id)
    {
        var schedule = await FindScheduleAsync(id);
        var next = await GetOrderedSchedules(schedule.TourId)
            .Where(item =>
                item.ScheduleDate > schedule.ScheduleDate ||
                item.ScheduleDate == schedule.ScheduleDate && item.DayNumber > schedule.DayNumber ||
                item.ScheduleDate == schedule.ScheduleDate && item.DayNumber == schedule.DayNumber && item.SortOrder > schedule.SortOrder)
            .FirstOrDefaultAsync();

        if (next is null)
        {
            return;
        }

        (schedule.SortOrder, next.SortOrder) = (next.SortOrder, schedule.SortOrder);
        await _dbContext.SaveChangesAsync();
        await NormalizeSortOrderAsync(schedule.TourId);
    }

    public async Task<IReadOnlyList<SelectListItem>> GetTourOptionsAsync(int? selectedTourId = null)
    {
        return await _dbContext.Tours
            .AsNoTracking()
            .OrderBy(tour => tour.Name)
            .Select(tour => new SelectListItem
            {
                Value = tour.ScheduleTourId.ToString(),
                Text = $"{tour.Name} ({tour.Code})",
                Selected = selectedTourId.HasValue && tour.ScheduleTourId == selectedTourId.Value
            })
            .ToListAsync();
    }

    private async Task<TourSchedule> FindScheduleAsync(int id)
    {
        var schedule = await _dbContext.TourSchedules
            .Include(item => item.Tour)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (schedule is null)
        {
            throw new KeyNotFoundException("Tour schedule was not found.");
        }

        return schedule;
    }

    private async Task EnsureTourExistsAsync(int tourId)
    {
        if (!await _dbContext.Tours.AnyAsync(tour => tour.ScheduleTourId == tourId))
        {
            throw new KeyNotFoundException("Tour was not found.");
        }
    }

    private async Task<int> GetNextSortOrderAsync(int tourId)
    {
        var maxOrder = await _dbContext.TourSchedules
            .Where(schedule => schedule.TourId == tourId)
            .MaxAsync(schedule => (int?)schedule.SortOrder);

        return (maxOrder ?? 0) + 1;
    }

    private IQueryable<TourSchedule> GetOrderedSchedules(int tourId)
    {
        return _dbContext.TourSchedules
            .Where(schedule => schedule.TourId == tourId)
            .OrderBy(schedule => schedule.ScheduleDate)
            .ThenBy(schedule => schedule.DayNumber)
            .ThenBy(schedule => schedule.SortOrder);
    }

    private async Task NormalizeSortOrderAsync(int tourId)
    {
        var schedules = await GetOrderedSchedules(tourId).ToListAsync();

        for (var index = 0; index < schedules.Count; index++)
        {
            schedules[index].SortOrder = index + 1;
        }

        await _dbContext.SaveChangesAsync();
    }

    private static TourScheduleDetailsViewModel ToDetails(TourSchedule schedule)
    {
        return new TourScheduleDetailsViewModel
        {
            Id = schedule.Id,
            TourId = schedule.TourId,
            TourName = schedule.Tour.Name,
            DayNumber = schedule.DayNumber,
            ScheduleDate = schedule.ScheduleDate,
            Title = schedule.Title,
            Description = schedule.Description,
            Location = schedule.Location,
            StartTime = schedule.StartTime,
            EndTime = schedule.EndTime,
            SortOrder = schedule.SortOrder
        };
    }
}
