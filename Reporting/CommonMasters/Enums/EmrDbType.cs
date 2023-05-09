using System.ComponentModel;

namespace Reporting.CommonMasters.Enums;

public enum EmrDbType
{
    [Description("Data Source=DESKTOP-9F1PTDM:1521/aidb;User Id=AISYS;Password=ai-Nx10nrl;")]
    Oracle = 1,
    [Description("host=localhost;port=5432;database=Emr;user id=postgres;password=Emr!23")]
    PostgreSql = 2,
    [Description("host=localhost;port=5432;database=EmrTest;user id=postgres;password=Emr!23")]
    PostgreSqlTest = 3,
    [Description("Data Source=localhost;Initial Catalog=EmrTest;Integrated Security=False;UID=sa;pwd=admin;MultipleActiveResultSets=True;")]
    MsSql = 4,
}

public static class DbTypeExtensions
{
    public static string ToDescription(this EmrDbType val)
    {
        DescriptionAttribute[] attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes[0].Description;
    }
}
