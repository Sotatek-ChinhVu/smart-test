using System.Text;

namespace KarteReport.Utility
{
    public static class TemplateGenerator
    {
        public static string GetHTMLString()
        {
            var employees = DataStorage.GetAllEmployees();
            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>診 療 録</h1></div>
                                    <p><span dir=""ltr"" role=""presentation"">患 者 番 号 883</span></p>
                                   <table style=""border-collapse: collapse; width: 49.2899%; height: 26px;"" border=""1"">
                                    <tbody>
                                    <tr style=""height: 26px;"">
                                    <td style=""width: 37.1287%; height: 26px;""><span dir=""ltr"" role=""presentation"">公費負担者番号</span></td>
                                    <td style=""width: 2.88184%; height: 26px;"">2</td>
                                    <td style=""width: 11.2106%; height: 26px;"">8</td>
                                    <td style=""width: 10.0214%; height: 26px;"">2</td>
                                    <td style=""width: 8.9466%; height: 26px;"">7</td>
                                    <td style=""width: 11.1111%; height: 26px;"">1</td>
                                    <td style=""width: 11.1111%; height: 26px;"">5</td>
                                    <td style=""width: 11.1111%; height: 26px;"">0</td>
                                    <td style=""width: 33.0823%; height: 26px;"">0</td>
                                    </tr>
                                    </tbody>
                                    </table>
                                    <table style=""border-collapse: collapse; width: 49.858%; height: 43px;"" border=""1"">
                                    <tbody>
                                    <tr style=""height: 43px;"">
                                    <td style=""width: 34.1658%; height: 43px;""><span dir=""ltr"" role=""presentation"">公 費 負 担 医 療</span><br role=""presentation"" /><span dir=""ltr"" role=""presentation"">の 受 給 者 番 号</span></td>
                                    <td style=""width: 5.4755%; height: 43px;"">9</td>
                                    <td style=""width: 7.3647%; height: 43px;"">9</td>
                                    <td style=""width: 11.1111%; height: 43px;"">9</td>
                                    <td style=""width: 11.1111%; height: 43px;"">9</td>
                                    <td style=""width: 11.1111%; height: 43px;"">9</td>
                                    <td style=""width: 11.1111%; height: 43px;"">9</td>
                                    <td style=""width: 11.1111%; height: 43px;"">9</td>
                                    <td style=""width: 12.2638%; height: 43px;"">6</td>
                                    </tr>
                                    </tbody>
                                 </table>
                            </body>
                        </html>");
            return sb.ToString();
        }
    }
}
