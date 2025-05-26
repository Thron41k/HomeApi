using System;
using System.Threading.Tasks;
using AutoMapper;
using HomeApi.Contracts.Models.Rooms;
using HomeApi.Data.Models;
using HomeApi.Data.Repos;
using Microsoft.AspNetCore.Mvc;

namespace HomeApi.Controllers;

/// <summary>
/// Контроллер комнат
/// </summary>
[ApiController]
[Route("[controller]")]
public class RoomsController(IRoomRepository repository, IMapper mapper) : ControllerBase
{
    //TODO: Задание - добавить метод на получение всех существующих комнат


    /// <summary>
    /// Добавление комнаты
    /// </summary>
    [HttpPost] 
    [Route("")] 
    public async Task<IActionResult> Add([FromBody] AddRoomRequest request)
    {
        var existingRoom = await repository.GetRoomByName(request.Name);
        if (existingRoom != null) return StatusCode(409, $"Ошибка: Комната {request.Name} уже существует.");
        var newRoom = mapper.Map<AddRoomRequest, Room>(request);
        await repository.AddRoom(newRoom);
        return StatusCode(201, $"Комната {request.Name} добавлена!");
    }
}