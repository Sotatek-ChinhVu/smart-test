using Helper.Constants;

namespace Helper.Util
{
    public static class PtManagementUtil
    {
        public static List<ConfigObject> GetStaCsvTemplate(int dataSbt)
        {
            List<ConfigObject> result = new();
            if (dataSbt == 1)
            {
                result = StaCsvConfigTemplate.PtInfConfig.ToList();
            }
            else if (dataSbt == 2)
            {
                result = StaCsvConfigTemplate.HokenInfConfig.ToList();
            }
            else if (dataSbt == 3)
            {
                result = StaCsvConfigTemplate.ByomeiInfConfig.ToList();
            }
            else if (dataSbt == 4)
            {
                result = StaCsvConfigTemplate.RaiinInfConfig.ToList();
            }
            else if (dataSbt == 5)
            {
                result = StaCsvConfigTemplate.MedicalInfOrderConfig.ToList();
            }
            else if (dataSbt == 6)
            {
                result = StaCsvConfigTemplate.MedicalInfCalConfig.ToList();
            }
            else if (dataSbt == 7)
            {
                result = StaCsvConfigTemplate.MedicalRecordInfConfig.ToList();
            }
            else if (dataSbt == 8)
            {
                result = StaCsvConfigTemplate.KensaInfConfig.ToList();
            }
            return result;
        }

        public static string GetConfName(int dataSbt)
        {
            string result = string.Empty;
            switch (dataSbt)
            {
                case 1:
                    result = "患者情報";
                    break;
                case 2:
                    result = "保険情報";
                    break;
                case 3:
                    result = "病名情報";
                    break;
                case 4:
                    result = "来院情報";
                    break;
                case 5:
                    result = "診療情報（オーダー）";
                    break;
                case 6:
                    result = "診療情報（算定）";
                    break;
                case 7:
                    result = "カルテ情報";
                    break;
                case 8:
                    result = "検査情報";
                    break;
            }
            return result;
        }
    }
}
