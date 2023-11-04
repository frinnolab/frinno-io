using frinno_core.DTOs;
using frinno_core.Entities.FileAsset;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace frinno_application.FileAssets
{
    public interface IFileAssetService
    {
        //string GetAssetPath(string path);
        Task <FileUploadReponse> SaveFileAsynyc(FileAsset file);

        Task SaveBulkFilesAsynce( List<FileAsset>  files);

        Task DeleteFileAsync(int id, string profile_id);

        Task DownloadFilesync(int id);

    }
}
