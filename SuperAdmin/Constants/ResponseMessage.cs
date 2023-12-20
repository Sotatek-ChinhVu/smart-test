namespace SuperAdmin.Constants
{
    public static class ResponseMessage
    {
        public static readonly string InvalidRequest = "リクエストは無効です。";
        public static readonly string Success = "新しい医療機関が作成されました。";
        public static readonly string Failed = "新しい医療機関の作成に失敗しました。";
        public static readonly string InvalidLoginId = "ログインIDが無効です。";
        public static readonly string InvalidAdminId = "管理者IDが無効です。";
        public static readonly string InvalidPassword = "パスワードが無効です。";
        public static readonly string InvalidTenantId = "医療機関が無効です。";
        public static readonly string InvalidClusterMode = "クラスターモードが無効です。";
        public static readonly string InvalidSizeType = "サイズタイプが無効です。";
        public static readonly string InvalidSize = "サイズが無効です。";
        public static readonly string TenantDoesNotExist = "医療機関が存在しません。";
        public static readonly string TenantDbDoesNotExistInRDS = " RDSに医療機関のデータベースが存在しません。";
        public static readonly string SubDomainExists = "サブドメインが登録されています。";
        public static readonly string InvalidSubDomain = "サブドメインが無効です。";
        public static readonly string HopitalExists = "病院が登録されています。";
        public static readonly string InvalidIdNotification = "通知IDが存在しません。";
        public static readonly string NewDomainAleadyExist = "新しいドメインが登録されています。";
        public static readonly string NotAllowUpdateTenantDedicateToSharing = "共有タイプに専用の医療機関の更新を許可しません。";
        public static readonly string TenantIsTerminating = "医療機関が終了中です。";
        public static readonly string TenantIsNotAvailableToSortTerminate = "医療機関は論理的終了の準備ができません。";
    }

}
