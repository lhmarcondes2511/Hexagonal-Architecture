using Domain.Guest.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Domain.Guest.Entities;

namespace Data.Guests
{
    public class GuestConfiguration : IEntityTypeConfiguration<Guest>
    {
        public void Configure(EntityTypeBuilder<Guest> builder)
        {
            builder.HasKey(x => x.Id);
            builder.OwnsOne(X => X.DocumentId)
                .Property(x => x.IdNumber);

            builder.OwnsOne(x => x.DocumentId)
                .Property(x => x.DocumentType);
        }
    }
}
