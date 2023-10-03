using Domain.Models.CalcStatus;
using Domain.Models.Document;
using Domain.Models.PatientInfor;
using Domain.Models.SpecialNote.PatientInfo;

namespace Domain.Models.Lock
{
    public class LockModel
    {
        public LockModel(int userId, string userName, DateTime lockDateTime, string functionName, string functionCode, int lockLevel, int lockRange, string tabKey)
        {
            UserId = userId;
            UserName = userName;
            LockDateTime = lockDateTime;
            FunctionName = functionName;
            FunctionCode = functionCode;
            LockLevel = lockLevel;
            LockRange = lockRange;
            TabKey = tabKey;
        }

        public LockModel()
        {
            UserName = string.Empty;
            LockDateTime = DateTime.MinValue;
            FunctionName = string.Empty;
            FunctionCode = string.Empty;
            TabKey = string.Empty;
        }

        public LockModel(string functionCode, int userId, string userName, string functionName)
        {
            FunctionCode = functionCode;
            UserName = userName;
            LockDateTime = DateTime.MinValue;
            FunctionName = functionName;
            UserId = userId;
            LockLevel = 0;
            TabKey = string.Empty;
        }

        public LockModel(PatientInfoModel patientInfoModels, CalcStatusModel calcStatusModels, DocInfModel docInfModels)
        {
            PatientInfoModels = patientInfoModels;
            CalcStatusModels = calcStatusModels;
            DocInfModels = docInfModels;
            UserName = string.Empty;
            FunctionName = string.Empty;
            FunctionCode = string.Empty;
            TabKey = string.Empty;
        }

        public PatientInfoModel PatientInfoModels {  get; private set; }

        public CalcStatusModel CalcStatusModels { get; private set; }

        public DocInfModel DocInfModels { get; private set; } 

        public int UserId { get; private set; }

        public string UserName { get; private set; }

        public DateTime LockDateTime { get; private set; }

        public string FunctionName { get; private set; }

        public string FunctionCode { get; private set; }

        public int LockLevel { get; private set; } = -1;

        public int LockRange { get; private set; } = -1;

        public string TabKey { get; private set; }

        public bool IsEmpty => LockLevel < 0 || LockRange < 0;

        public long PtNum
        {
            get
            {
                if (DocInfModels?.PtNum > 0)
                {
                    return DocInfModels.PtNum;
                }
                if (CalcStatusModels?.PtNum > 0)
                {
                    return CalcStatusModels.PtNum;
                }
                if (PatientInfoModels?.PtNum > 0)
                {
                    return PatientInfoModels.PtNum;
                }
                return 0;
            }
        }

        public int SinDate
        {
            get
            {
                if (DocInfModels?.SinDate > 0)
                {
                    return DocInfModels.SinDate;
                }
                if (CalcStatusModels?.SinDate > 0)
                {
                    return CalcStatusModels.SinDate;
                }
                if (PatientInfoModels?.SinDate > 0)
                {
                    return PatientInfoModels.SinDateInt;
                }
                return 0;
            }
        }

        public int UserIdLock
        {
            get
            {
                if (DocInfModels?.us > 0)
                {
                    return DocInfModels.SinDate;
                }
                if (CalcStatusModels?.us > 0)
                {
                    return CalcStatusModels.SinDate;
                }
                if (PatientInfoModels?.us > 0)
                {
                    return PatientInfoModels.SinDateInt;
                }
                return 0;
            }
        }

        public bool CheckDefaultValue()
        {
            return SinDate == 0;
        }
    }
}