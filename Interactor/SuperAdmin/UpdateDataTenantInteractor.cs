using Domain.SuperAdminModels.Notification;
using Domain.SuperAdminModels.Tenant;
using Interactor.Realtime;
using SharpCompress.Archives;
using SharpCompress.Archives.SevenZip;
using UseCase.SuperAdmin.UpdateDataTenant;
using ReaderOptions = SharpCompress.Readers.ReaderOptions;

namespace Interactor.SuperAdmin
{
    public class UpdateDataTenantInteractor : IUpdateDataTenantInputPort
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly ITenantRepository _tenantRepositoryRunTask;
        private readonly INotificationRepository _notificationRepositoryRunTask;

        public UpdateDataTenantInteractor(
            ITenantRepository tenantRepository,
            ITenantRepository tenantRepositoryRunTask,
            INotificationRepository notificationRepositoryRunTask
            )
        {
            _tenantRepository = tenantRepository;
            _tenantRepositoryRunTask = tenantRepositoryRunTask;
            _notificationRepositoryRunTask = notificationRepositoryRunTask;
        }
        public UpdateDataTenantOutputData Handle(UpdateDataTenantInputData inputData)
        {
            try
            {
                IWebSocketService _webSocketService;
                _webSocketService = (IWebSocketService)inputData.WebSocketService;

                if (inputData.TenantId <= 0)
                {
                    return new UpdateDataTenantOutputData(false, UpdateDataTenantStatus.InvalidTenantId);
                }

                var tenant = _tenantRepository.Get(inputData.TenantId);

                if (tenant == null || tenant.TenantId <= 0)
                {
                    return new UpdateDataTenantOutputData(false, UpdateDataTenantStatus.TenantDoesNotExist);
                }

                if (string.Equals(Path.GetExtension(inputData.FileUpdateData.FileName), ".7z", StringComparison.OrdinalIgnoreCase))
                {
                    return new UpdateDataTenantOutputData(false, UpdateDataTenantStatus.UploadFileIncorrectFormat7z);
                }

                // Extract file 7z
                using (var archive = SevenZipArchive.Open(@"D:\e01.200825_01.7z", new ReaderOptions() { Password = "Sotatek" }))
                {
                    archive.ExtractToDirectory(@"D:\7Z");
                }
                // Execute file script in folder 02_script

                // Create transaction executed 

                return new UpdateDataTenantOutputData(true, UpdateDataTenantStatus.Successed);
            }
            finally
            {
                _tenantRepository.ReleaseResource();
            }
        }
    }
}
