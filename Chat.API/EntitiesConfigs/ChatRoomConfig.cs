using Chat.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.API.EntitiesConfigs
{
    public class ChatRoomConfig : IEntityTypeConfiguration<ChatRoom>
    {
        public void Configure(EntityTypeBuilder<ChatRoom> builder)
        {
            builder.Property(cr => cr.Name)
              .HasMaxLength(200);

            builder.HasIndex(cr => cr.Name)
                .IsUnique();

            builder.Property(cr => cr.Description)
                .HasMaxLength(500);
        }
    }
}
