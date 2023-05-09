using Reporting.CommonMasters.Enums;

namespace Reporting.CommonMasters.Services;

public class DbConnectionSetting
{
    public static string RenkeiConnectionString = EmrDbType.MsSql.ToDescription();
}
