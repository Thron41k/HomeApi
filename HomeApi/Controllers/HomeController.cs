using AutoMapper;
using HomeApi.Configuration;
using HomeApi.Contracts.Models.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HomeApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController(IOptions<HomeOptions> options, IMapper mapper) : ControllerBase
{
    // Инициализация конфигурации при вызове конструктора

    /// <summary>
    /// Метод для получения информации о доме
    /// </summary>
    [HttpGet]
    [Route("info")] 
    public IActionResult Info()
    {
        // Получим запрос, смапив конфигурацию на модель запроса
        var infoResponse = mapper.Map<HomeOptions, InfoResponse>(options.Value);
        // Вернём ответ
        return StatusCode(200, infoResponse);
    }
}