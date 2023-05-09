using Helper.Extension;
using System.Globalization;

namespace Reporting.PatientManagement.Models;

public class MoneyConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value.AsInteger() == 0)
        {
            return string.Empty;
        }
        return String.Format("{0:n0}", value.AsInteger());

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string convertBackValue = value.AsString();
        if (convertBackValue.Contains(","))
        {
            convertBackValue = convertBackValue.Replace(",", "");
        }
        return convertBackValue.AsInteger();
    }
}
