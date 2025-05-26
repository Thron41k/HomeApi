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
    /// Обновление существующей комнаты (полная перезапись свойств)
    /// </summary>
    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateRoomRequest request)
    {
        var existingRoom = await repository.GetRoomById(id);
        if (existingRoom == null)
            return NotFound($"Ошибка: Комната {id} не найдена.");

        // Проверяем, не пытаемся ли переименовать в уже существующую комнату
        if (existingRoom.Name != request.Name)
        {
            var roomWithNewName = await repository.GetRoomByName(request.Name);
            if (roomWithNewName != null)
                return Conflict($"Ошибка: Комната с именем {request.Name} уже существует.");
        }

        mapper.Map(request, existingRoom);
        await repository.UpdateRoom(existingRoom);

        return Ok($"Комната {id} успешно обновлена. Новое имя: {existingRoom.Name}");
    }

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