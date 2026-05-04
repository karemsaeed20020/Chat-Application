

using ChatApplication.API.DTOs.Room;

namespace Chat.API.Services.RoomService
{
    public interface IRoomServicecs
    {
        Task<Result<IEnumerable<RoomResponse>>> GetAllRoomsAsync(CancellationToken cancellationToken = default);

        Task<Result<IEnumerable<RoomResponse>>> GetUserRoomsAsync(string userId, CancellationToken cancellationToken = default);

        Task<Result<RoomResponse>> GetRoomByIdAsync(int roomId, CancellationToken cancellationToken = default);

        Task<Result<RoomResponse>> CreateRoomAsync(FormRoomRequest request, CancellationToken cancellationToken = default);

        Task<Result<RoomResponse>> UpdateRoomAsync(int roomId, FormRoomRequest request, CancellationToken cancellationToken = default);

    }
}
