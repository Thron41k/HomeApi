﻿using FluentValidation;
using HomeApi.Contracts.Models.Rooms;

namespace HomeApi.Contracts.Validation;

/// <summary>
/// Класс-валидатор запросов на добавление новой комнаты
/// </summary>
public class AddRoomRequestValidator : AbstractValidator<AddRoomRequest>
{
    public AddRoomRequestValidator() 
    {
        RuleFor(x => x.Area).NotEmpty(); 
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Voltage).NotEmpty();
        RuleFor(x => x.GasConnected).NotEmpty();
    }
}