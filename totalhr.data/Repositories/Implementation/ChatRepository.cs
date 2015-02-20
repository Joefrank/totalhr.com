using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.Shared;
using totalhr.data.EF;
using totalhr.data.Repositories.Infrastructure;

namespace totalhr.data.Repositories.Implementation
{
    public class ChatRepository : GenericRepository<TotalHREntities, ChatRoom>, IChatRepository
    {
        public int CreateRoom(Shared.Models.ChatRoomInfo room)
        {
            var chatroom = new ChatRoom
                {
                    Name = room.RoomName,
                    Description = room.Description,
                    Private = room.Target == (int)Variables.ChatRoomTarget.Private,
                    Created = DateTime.Now,
                    CreatedBy = room.UserId,
                    InvitedUsers = room.InvitedUserIds
                };
            Context.ChatRooms.Add(chatroom);
            return Context.SaveChanges();
        }
    }
}
