using frinno_core.Entities.FileAsset;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace frinno_core.DTOs
{
    public class FileUploadRequest
    {
        [Required]
        public string FileName { get; set; }
        public string? FileDescription{ get; set; } = string.Empty;
        public string ProfileId { get; set; }
        public FileType FileType { get; set; }
    }

    public class FileUploadReponse : FileUploadRequest
    {
        public int FileId { get; set; }
    }
   }
