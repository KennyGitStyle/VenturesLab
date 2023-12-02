using API.DTOs;
using API.Helper;
using Domain.Entites;
using Infrastructure.Helper;
using Infrastructure.Service;
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints;

/// <summary>
/// Static class for mapping HTTP endpoints related to user tasks.
/// It contains methods for handling various CRUD operations on user tasks.
/// </summary>
public static class UserTasksEndpoints
{
    /// <summary>
    /// Configures the endpoints for UserTask operations.
    /// </summary>
    /// <param name="app">The application's endpoint route builder.</param>
    public static void MapUserTasksEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("api/usertasks_bysorting", GetSortedUserTasks)
           .WithName(nameof(GetSortedUserTasks));

        app.MapGet("api/usertasks_grouping", GetUserTasksByGroup)
           .WithName(nameof(GetUserTasksByGroup));

        app.MapGet("api/usertask/{id}", GetUserTask)
            .WithName(nameof(GetUserTask))
            .CacheOutput();

        app.MapGet("api/usertasks", GetUserTasks)
            .WithName(nameof(GetUserTasks))
            .WithMetadata(new CacheableAttribute(600));

        app.MapPost("api/usertask", AddUserTask)
            .WithName(nameof(AddUserTask));

        app.MapPut("api/usertask/{id}", UpdateUserTask)
            .WithName(nameof(UpdateUserTask));

        app.MapDelete("api/usertask/{id}", DeleteUserTask)
            .WithName(nameof(DeleteUserTask));
    }
    
    public static async Task<Results<Ok<IEnumerable<UserTaskDto>>, NotFound<string>>> GetSortedUserTasks(IUnitOfWork unitOfWork, [AsParameters]SortParams sortParams)
    {
        try
        {
            var sortedTasks = await unitOfWork.UserTask.GetSortedUserTasks(sortParams);
            var sortedTaskDtos = sortedTasks.Adapt<IEnumerable<UserTaskDto>>();
            
            return TypedResults.Ok(sortedTaskDtos);
        }
        catch (Exception ex)
        {
            return TypedResults.NotFound($"Error: {ex.Message}");
        }
    }

    public static async Task<Results<Ok<IEnumerable<GroupedUserTaskDto>>, NotFound<string>>>
    GetUserTasksByGroup(IUnitOfWork unitOfWork)
    {
        try
        {
            var tasks = await unitOfWork.UserTask.GetTodayAndUpcomingUserTasks();
            var groupedTasks = tasks.Select(group => new GroupedUserTaskDto
            {
                Date = group.Key,
                Tasks = group.Select(task => task.Adapt<UserTaskDto>())
            });

            return TypedResults.Ok(groupedTasks);
        }
        catch (Exception ex)
        {
            return TypedResults.NotFound($"Error: {ex.Message}");
        }
    }
    
    public static async Task<Results<Ok<IEnumerable<UserTaskDto>>, NotFound<string>>>
    GetUserTasks(IUnitOfWork unitOfWork)
    {
        try
        {
            var tasks = await unitOfWork.UserTask.GetUserTasks();
            var taskDtos = tasks.Adapt<IEnumerable<UserTaskDto>>();
                
            return TypedResults.Ok(taskDtos);
        }
        catch (Exception ex)
        {
            return TypedResults.NotFound($"Error: {ex.Message}");
        }
    }

    public static async Task<Results<Ok<UserTaskDto>, NotFound<string>>> 
    GetUserTask(Guid id, IUnitOfWork unitOfWork)
    {
        try
        {
            var task = await unitOfWork.UserTask.GetUserTask(id);
            var taskDto = task.Adapt<UserTaskDto>();
                
            return TypedResults.Ok(taskDto);
        }
        catch (Exception ex)
        {
            return TypedResults.NotFound($"Error: {ex.Message}");
        }
    }

    public static async Task<Results<Created, NotFound<string>>> 
    AddUserTask(IUnitOfWork unitOfWork, [FromBody] UserTaskDto userTaskCreate)
    {
        try
        {
            var userTask = userTaskCreate.Adapt<UserTask>();
            await unitOfWork.UserTask.AddUserTask(userTask);
                
            return TypedResults.Created();
        }
        catch (Exception ex)
        {
            return TypedResults.NotFound($"Error: {ex.Message}");
        }
    }

    public static async Task<Results<NoContent, NotFound<string>>> 
    UpdateUserTask(Guid id, IUnitOfWork unitOfWork, [FromBody] UserTaskDto userTaskUpdateDto)
    {
        if(id == Guid.Empty)
        {
            return TypedResults.NotFound("Error");
        }

        if(userTaskUpdateDto is null)
        {
            return TypedResults.NotFound("Error");
        }

        try
        {
            var userTask = userTaskUpdateDto.Adapt<UserTask>();
            await unitOfWork.UserTask.UpdateUserTask(id, userTask);

            return TypedResults.NoContent();
        }
        catch (Exception ex)
        {
            return TypedResults.NotFound($"Error: {ex.Message}");
        }
    }

    public static async Task<Results<NoContent, NotFound<string>>> 
    DeleteUserTask(Guid id, IUnitOfWork unitOfWork)
    {
        try
        {
            await unitOfWork.UserTask.DeleteUserTask(id);
                
            return TypedResults.NoContent();
        }
        catch (Exception ex)
        {
            return TypedResults.NotFound($"Error: {ex.Message}");
        }
    }
}
