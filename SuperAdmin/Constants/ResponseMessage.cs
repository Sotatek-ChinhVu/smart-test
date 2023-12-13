namespace SuperAdmin.Constants
{
    public static class ResponseMessage
    {
        public static readonly string Success = "Success";
        public static readonly string Fail = "Fail";
        public static readonly string InvalidLoginId = "Invalid Login Id";
        public static readonly string InvalidPassword = "Invalid Password";
        public static readonly string InvalidTenantId = "Invalid Tenant Id";
        public static readonly string InvalidClusterMode = "Invalid Cluster Mode";
        public static readonly string InvalidSizeType = "Invalid Size Type";
        public static readonly string InvalidSize = "Invalid Size";
        public static readonly string TenantDoesNotExist = "Tenant Does Not Exist";
        public static readonly string TenantDbDoesNotExistInRDS = "TenantDb Does Not Exist In RDS";
        public static readonly string SubDomainExists = "SubDomain Exists";
        public static readonly string InvalidIdNotification = "Invalid Id Notification.";
        public static readonly string InvalidDomain = "Invalid Domain";
        public static readonly string NewDomainAleadyExist = "New Domain Aleady Exist";
        public static readonly string FailedTenantIsPremium = "Failed Tenant Is Premium";
        public static readonly string TenantIsTerminating = "Tenant Is Terminating";
        public static readonly string TenantIsNotAvailableToSortTerminate = "Tenant Is Not Available To Sort Terminate";
    }
    
}
