using AWSSDK.Common;
using AWSSDK.Constants;
using Domain.SuperAdminModels.Tenant;
using Interactor.Realtime;
using Microsoft.Extensions.Configuration;
using SharpCompress.Archives;
using SharpCompress.Archives.SevenZip;
using System.Runtime.InteropServices;
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
                string pathFolderScript = $"{pathFileExtract7z}\\{UpdateConst.UPD_FILE_FOLDER}\\{UpdateConst.UPDATE_SQL}";
                string pathFolderMaster = $"{pathFileExtract7z}\\{UpdateConst.UPD_FILE_FOLDER}\\{UpdateConst.UPDATE_MASTER}";

                // Replace path file linux
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {

                    pathFile7z = pathFile7z.Replace("\\", "/");
                    pathFolderScript = pathFolderScript.Replace("\\", "/");
                    pathFolderMaster = pathFolderMaster.Replace("\\", "/");
                }

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
                // Execute file script in folder 03_master
                string[] subFoldersMasters = Directory.GetDirectories(pathFolderMaster);
                UpdateDataTenant.ExcuteUpdateDataTenant(extractedFiles, tenant.EndPointDb, ConfigConstant.PgPostDefault, tenant.Db, tenant.UserConnect, tenant.PasswordConnect, subFoldersMasters);
                return new UpdateDataTenantOutputData(true, UpdateDataTenantStatus.Successed);
            }
            finally
            {
                _tenantRepository.ReleaseResource();
            }
        }
    }
}
