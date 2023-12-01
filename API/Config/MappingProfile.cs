using API.DTOs;
using Domain.Entites;
using Mapster;

namespace API.Config;

/// <summary>
/// Configures object mappings between domain entities and Data Transfer Objects (DTOs) using Mapster.
/// This class defines how entities in the domain model are mapped to their corresponding DTOs.
/// </summary>
public static class MappingProfile 
{
    public static void Configure()
    {
        TypeAdapterConfig<User, UserDto>.NewConfig()
            .Map(dest => dest.Id, src => src.Id.ToString())
            .Map(dest => dest.Firstname, src => src.Firstname)
            .Map(dest => dest.Lastname, src => src.Lastname)
            .Map(dest => dest.DateOfBirth, src => src.DataOfBirth)
            .Map(dest => dest.Tasks, src => src.Tasks);

        TypeAdapterConfig<UserTask, UserTaskDto>.NewConfig()
            .Map(dest => dest.Id, src => src.Id.ToString())
            .Map(dest => dest.CurrentDate, src => src.CurrentDate)
            .Map(dest => dest.StartTime, src => src.StartTime)
            .Map(dest => dest.EndTime, src => src.EndTime)
            .Map(dest => dest.Subject, src => src.Subject)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.IsCurrentDate, src => src.IsCurrentDate);
    }
}
