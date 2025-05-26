using System;
using System.Linq;
using System.Threading.Tasks;
using HomeApi.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeApi.Data.Repos;

/// <summary>
/// Репозиторий для операций с объектами типа "Room" в базе
/// </summary>
public class RoomRepository(HomeApiContext context) : IRoomRepository
{
    /// <summary>
    ///  Найти комнату по имени
    /// </summary>
    public async Task<Room> GetRoomByName(string name)
    {
        return await context.Rooms.Where(r => r.Name == name).FirstOrDefaultAsync();
    }
    /// <summary>
    ///  Добавить новую комнату
    /// </summary>
    public async Task AddRoom(Room room)
    {
        var entry = context.Entry(room);
        if (entry.State == EntityState.Detached)
            await context.Rooms.AddAsync(room);
            
        await context.SaveChangesAsync();
    }
}