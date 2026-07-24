# TourSchedule Code First Migration Guide

Run these commands from the `WanderXServer` folder after reviewing the generated migration:

```powershell
dotnet ef migrations add AddTourSchedules
dotnet ef database update
```

This feature adds:

- `Tour.ScheduleTourId` as an integer alternate key used by MVC schedule management.
- `TourSchedules` table with `TourId`, `DayNumber`, `ScheduleDate`, `Title`, `Description`, `Location`, `StartTime`, `EndTime`, and `SortOrder`.
- A foreign key from `TourSchedules.TourId` to `Tours.ScheduleTourId`.

The development startup path also includes SQL guards in `WanderXDbContext.SeedDevelopmentData()` so an existing local dev database can create the missing columns/tables when the app starts.
