using frinno_core.Entities.FileAsset;
using frinno_core.Entities.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace frinno_infrastructure.Mappings
{
    public class FileAssetsMappings : IEntityTypeConfiguration<FileAsset>
    {
        public void Configure(EntityTypeBuilder<FileAsset> builder)
        {
            builder.Property<string>(p => p.UploadProfile.Id)
                .HasColumnName("profile_id");

            builder.HasOne(f => f.UploadProfile)
                .WithMany()
                .HasForeignKey("profile_id");
        }
    }
}
