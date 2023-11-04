using frinno_application.FileAssets;
using frinno_core.DTOs;
using frinno_core.Entities.FileAsset;
using frinno_infrastructure.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace frinno_infrastructure.Repostories
{
    public class FileAssetsRepository : IFileAssetService
    {
        private DataContext data;

        public FileAssetsRepository(DataContext data) {  this.data = data; }

        public Task DeleteFileAsync(int id, string profile_id)
        {
            throw new NotImplementedException();
        }

        public Task DownloadFilesync(int id)
        {
            throw new NotImplementedException();
        }

        public Task SaveBulkFilesAsynce(List<FileAsset> files)
        {
            throw new NotImplementedException();
        }

        public async Task <FileUploadReponse> SaveFileAsynyc(FileAsset file)
        {
            try
            {
                var obj = await data.FileAssets.AddAsync(file);

                await data.SaveChangesAsync();

                var response = new FileUploadReponse()
                {
                    FileId = obj.Entity.Id,
                    FileDescription = obj.Entity.Description,
                    FileName = obj.Entity.Name,
                    FileType = obj.Entity.FileType,
                    ProfileId = obj.Entity.UploadProfile.Id,
                };

                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
