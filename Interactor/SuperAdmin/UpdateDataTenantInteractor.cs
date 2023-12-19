using AWSSDK.Common;
using Domain.SuperAdminModels.Tenant;
using Interactor.Realtime;
using Microsoft.Extensions.Configuration;
using SharpCompress.Archives;
using SharpCompress.Archives.SevenZip;
using UseCase.SuperAdmin.UpdateDataTenant;
using ReaderOptions = SharpCompress.Readers.ReaderOptions;

namespace Interactor.SuperAdmin
{
    public class UpdateDataTenantInteractor : IUpdateDataTenantInputPort
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IConfiguration _configuration;
        public UpdateDataTenantInteractor(
            ITenantRepository tenantRepository,
             IConfiguration configuration
            )
        {
            _tenantRepository = tenantRepository;
            _configuration = configuration;
        }
        public UpdateDataTenantOutputData Handle(UpdateDataTenantInputData inputData)
        {
            try
            {
                IWebSocketService _webSocketService;
                _webSocketService = (IWebSocketService)inputData.WebSocketService;
                string pathFolderUpdateDataTenant = _configuration["PathFolderUpdateDataTenant"] ?? string.Empty;
                string passwordFile7z = _configuration["PasswordFile7z"] ?? string.Empty;

                if (inputData.TenantId <= 0)
                {
                    return new UpdateDataTenantOutputData(false, UpdateDataTenantStatus.InvalidTenantId);
                }

                var tenant = _tenantRepository.Get(inputData.TenantId);

                if (tenant == null || tenant.TenantId <= 0)
                {
                    return new UpdateDataTenantOutputData(false, UpdateDataTenantStatus.TenantDoesNotExist);
                }

                if (!string.Equals(Path.GetExtension(inputData.FileUpdateData.FileName), ".7z", StringComparison.OrdinalIgnoreCase))
                {
                    return new UpdateDataTenantOutputData(false, UpdateDataTenantStatus.UploadFileIncorrectFormat7z);
                }


                string pathFile7z = $"{pathFolderUpdateDataTenant}\\{tenant.SubDomain}-{Guid.NewGuid()}.7z";
                string pathFileExtract7z = $"{pathFolderUpdateDataTenant}\\7Z-{tenant.SubDomain}-{Guid.NewGuid()}";

                string pathFolderScript = $"{pathFileExtract7z}\\updfile\\02_script";
                //string pathFolderScript = $"D:\\7Z-nghiaduong2-e2cbf31a-11d2-4418-bbdf-05f4ea5ae431\\updfile\\02_script";

                // Save file 7z
                using (var fileStream = new FileStream(pathFile7z, FileMode.Create))
                {
                    inputData.FileUpdateData.CopyTo(fileStream);
                }

                // Extract file 7z
                using (var archive = SevenZipArchive.Open(pathFile7z, new ReaderOptions() { Password = passwordFile7z }))
                {
                    archive.ExtractToDirectory(pathFileExtract7z);
                }

                // Execute file script in folder 02_script
                    
                // Create transaction executed 
                string[] extractedFiles = Directory.GetFiles(pathFolderScript);
                PostgresSqlAction.ExecuteSqlFiles(extractedFiles, "localhost", 5432,"test01",  "postgres", "1234$");
                return new UpdateDataTenantOutputData(true, UpdateDataTenantStatus.Successed);
            }
            finally
            {
                _tenantRepository.ReleaseResource();
            }
        }
    }
}
