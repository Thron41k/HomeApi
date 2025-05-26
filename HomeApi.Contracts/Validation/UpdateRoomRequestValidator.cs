using FluentValidation;
using HomeApi.Contracts.Models.Rooms;

namespace HomeApi.Contracts.Validation;

/// <summary>
/// Класс-валидатор запросов на обновление комнаты
/// </summary>
public class UpdateRoomRequestValidator : AbstractValidator<UpdateRoomRequest>
{
    public UpdateRoomRequestValidator() 
    {
        RuleFor(x => x.Area).NotEmpty(); 
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Voltage).NotEmpty();
        RuleFor(x => x.GasConnected).NotEmpty();
    }
}