using AutoMapper;
using SixMinApi.Data;
using SixMinApi.Dtos;
using SixMinApi.Models;

namespace SixMinApi.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication? MapCommandRoutes(this WebApplication app)
    {
        app.MapGet("api/v1/commands", async (ICommandRepository commandRepository, IMapper mapper) => {
            var commands = await commandRepository.GetAllCommands();
            var commandReadDto = mapper.Map<IEnumerable<CommandReadDto>>(commands);
            return Results.Ok(commandReadDto);
        });

        app.MapGet("api/v1/commands/{id}", async (ICommandRepository commandRepository, IMapper mapper, int id) => {
            var command = await commandRepository.GetCommandById(id);
            if (command != null)
            {
                return Results.Ok(mapper.Map<CommandReadDto>(command));
            }

            return Results.NotFound();
        });

        app.MapPost("api/v1/commands", async (ICommandRepository commandRepository, IMapper mapper, CommandCreateDto commandCreateDto) => {
            var command = mapper.Map<Command>(commandCreateDto);
            await commandRepository.CreateCommand(command);
            await commandRepository.SaveChanges();
            var commandReadDto = mapper.Map<CommandReadDto>(command); 
            return Results.Created($"api/v1/commands/{commandReadDto.Id}", commandReadDto);
        });

        app.MapPut("api/v1/commands/{id}", async (ICommandRepository commandRepository, IMapper mapper, int id, CommandUpdateDto commandUpdateDto) => {
            var command = await commandRepository.GetCommandById(id);
            if (command == null)
            {
                return Results.NotFound();
            }

            mapper.Map(commandUpdateDto, command);
            await commandRepository.SaveChanges();
            return Results.NoContent();
        });

        app.MapDelete("api/v1/commands/{id}", async (ICommandRepository commandRepository, IMapper mapper, int id) => {
            var command = await commandRepository.GetCommandById(id);
            if (command == null)
            {
                return Results.NotFound();
            }
            
            commandRepository.DeleteCommand(command);
            await commandRepository.SaveChanges();
            return Results.NoContent();
        });

        return app;
    }
}