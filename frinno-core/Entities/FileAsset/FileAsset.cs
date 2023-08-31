using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace frinno_core.Entities.FileAsset
{
    public class FileAsset : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public byte[] FileData { get; set; }
        public FileType FileType { get; set; }
        public Profiles.Profile UploadProfile { get; set; }
    }
}
