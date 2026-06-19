using AutoMapper;
using HealthTracker.Domain.Entities;
using HealthTracker.Application.Features.SymptomLogs.DTOs;
using HealthTracker.Application.Features.MoodLogs.DTOs;
using HealthTracker.Application.Features.SleepLogs.DTOs;
using HealthTracker.Application.Features.MealLogs.DTOs;
using HealthTracker.Application.Features.WeeklyReports.DTOs;

namespace HealthTracker.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<SymptomLog, SymptomLogDto>();
        CreateMap<MoodLog, MoodLogDto>();
        CreateMap<SleepLog, SleepLogDto>();
        CreateMap<MealLog, MealLogDto>();
        CreateMap<WeeklyReport, WeeklyReportDto>()
            .ForMember(dest => dest.GeneratedAt, opt => opt.MapFrom(src => src.CreatedAt));
    }
}