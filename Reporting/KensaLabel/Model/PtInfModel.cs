using Helper.Common;

namespace Reporting.KensaLabel.Model
{
    public class PtInfModel
    {
        public long PtNum { get; set; }

        public string ?KanaName { get; set; } 

        public string ?Name { get; set; }

        public int PrintDate { get; set; }

        public int Sex { get; set; }

        public int BirthDay { get; set; }

        public string Age
        {
            get
            {
                int age = -1;
                if (BirthDay > 0)
                {
                    age = CIUtil.SDateToAge(BirthDay, PrintDate);
                }
                return GetAgeString(age);
            }
        }

        private string GetAgeString(int age)
        {
            if (age < 0)
            {
                return string.Empty;
            }
            return string.Format("{0}", age);
        }
    }
}
