﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using HomeApi.Contracts.Models.Devices;
using HomeApi.Data.Models;
using HomeApi.Data.Queries;
using HomeApi.Data.Repos;
using Microsoft.AspNetCore.Mvc;

namespace HomeApi.Controllers;

/// <summary>
/// Контроллер устройсив
/// </summary>
[ApiController]
[Route("[controller]")]
public class DevicesController(IDeviceRepository devices, IRoomRepository rooms, IMapper mapper)
    : ControllerBase
{
    /// <summary>
    /// Просмотр списка подключенных устройств
    /// </summary>
    [HttpGet] 
    [Route("")] 
    public async Task<IActionResult> GetDevices()
    {
        var devices1 = await devices.GetDevices();

        var resp = new GetDevicesResponse
        {
            DeviceAmount = devices1.Length,
            Devices = mapper.Map<Device[], DeviceView[]>(devices1)
        };
            
        return StatusCode(200, resp);
    }
        
    /// <summary>
    /// Добавление нового устройства
    /// </summary>
    [HttpPost] 
    [Route("")] 
    public async Task<IActionResult> Add( AddDeviceRequest request )
    {
        var room = await rooms.GetRoomByName(request.RoomLocation);
        if(room == null)
            return StatusCode(400, $"Ошибка: Комната {request.RoomLocation} не подключена. Сначала подключите комнату!");
            
        var device = await devices.GetDeviceByName(request.Name);
        if(device != null)
            return StatusCode(400, $"Ошибка: Устройство {request.Name} уже существует.");
            
        var newDevice = mapper.Map<AddDeviceRequest, Device>(request);
        await devices.SaveDevice(newDevice, room);
            
        return StatusCode(201, $"Устройство {request.Name} добавлено. Идентификатор: {newDevice.Id}");
    }
        
    /// <summary>
    /// Обновление существующего устройства
    /// </summary>
    [HttpPatch] 
    [Route("{id:guid}")] 
    public async Task<IActionResult> Edit(
        [FromRoute] Guid id,
        [FromBody]  EditDeviceRequest request)
    {
        var room = await rooms.GetRoomByName(request.NewRoom);
        if(room == null)
            return StatusCode(400, $"Ошибка: Комната {request.NewRoom} не подключена. Сначала подключите комнату!");
            
        var device = await devices.GetDeviceById(id);
        if(device == null)
            return StatusCode(400, $"Ошибка: Устройство с идентификатором {id} не существует.");
            
        var withSameName = await devices.GetDeviceByName(request.NewName);
        if(withSameName != null)
            return StatusCode(400, $"Ошибка: Устройство с именем {request.NewName} уже подключено. Выберите другое имя!");

        await devices.UpdateDevice(
            device,
            room,
            new UpdateDeviceQuery(request.NewName, request.NewSerial)
        );

        return StatusCode(200, $"Устройство обновлено! Имя - {device.Name}, Серийный номер - {device.SerialNumber},  Комната подключения - {device.Room.Name}");
    }


    // TODO: Задание: напишите запрос на удаление устройства

    /// <summary>
    /// Удаление существующего устройства
    /// </summary>
    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var device = await devices.GetDeviceById(id);
        if (device == null)
            return StatusCode(400, $"Ошибка: Устройство с идентификатором {id} не существует.");

        await devices.DeleteDevice(device);

        return StatusCode(200, $"Устройство '{device.Name}' с идентификатором {id} успешно удалено.");
    }
}