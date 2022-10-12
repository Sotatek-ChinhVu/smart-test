namespace Domain.Models.PatientInfor
{
    public class PtKyuseiInfModel
    {
        public PtKyuseiInfModel(int hpId, long ptId, string kanaName, string name, int endDate, int isDeleted)
        {
            HpId = hpId;
            PtId = ptId;
            KanaName = kanaName;
            Name = name;
            EndDate = endDate;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public string KanaName { get; private set; }
        public string Name { get; private set; }
        public int EndDate { get; private set; }
        public int IsDeleted { get; private set; }

        public string FirstName
        {
            get => SetFirstName();
        }

        public string LastName
        {
            get => SetLastName();
        }

        public string FirstKanaName
        {
            get => SetFirstKanaName();
        }

        public string LastKanaName
        {
            get => SetLastKanaName();
        }

        #region validation Name
        private string SetFirstName()
        {
            if (!string.IsNullOrWhiteSpace(Name))
            {
                try
                {
                    return Name.Substring(0, Name.IndexOf("　"));
                }
                catch (Exception)
                {
                    return Name;
                }
            }

            return string.Empty;
        }

        private string SetLastName()
        {
            if (Name == FirstName)
                return string.Empty;

            return Name.Substring(Name.IndexOf("　") + 1);
        }

        private string SetFirstKanaName()
        {
            if (!string.IsNullOrWhiteSpace(KanaName))
            {
                try
                {
                    return KanaName.Substring(0, KanaName.IndexOf(" "));
                }
                catch (Exception)
                {
                    return KanaName;
                }
            }

            return string.Empty;
        }

        private string SetLastKanaName()
        {
            if (KanaName == FirstKanaName)
                return string.Empty;

            return KanaName.Substring(KanaName.IndexOf(" ") + 1);
        }
        #endregion

    }
}
