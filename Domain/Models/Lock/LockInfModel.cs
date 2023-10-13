using Helper.Common;
using Helper.Extension;
using System.Text.Json.Serialization;

namespace Domain.Models.Lock
{
    public class LockInfModel
    {
        [JsonConstructor]

        public LockInfModel(LockPtInfModel patientInfoModels)
        {
            PatientInfoModels = patientInfoModels;
        }

        public LockInfModel(LockCalcStatusModel calcStatusModels)
        {
            CalcStatusModels = calcStatusModels;
        }

        public LockInfModel(LockDocInfModel docInfModels)
        {
            DocInfModels = docInfModels;
        }

        public LockInfModel(LockPtInfModel patientInfoModels, LockCalcStatusModel calcStatusModels, LockDocInfModel docInfModels)
        {
            PatientInfoModels = patientInfoModels;
            CalcStatusModels = calcStatusModels;
            DocInfModels = docInfModels;
        }

        public int UserId
        {
            get
            {
                if (DocInfModels != null)
                {
                    return DocInfModels.LockId;
                }
                if (CalcStatusModels != null)
                {
                    return CalcStatusModels.CreateId;
                }
                if (PatientInfoModels != null)
                {
                    return PatientInfoModels.UserId;
                }
                return 0;
            }
        }

        public LockPtInfModel PatientInfoModels { get; private set; } 

        public LockCalcStatusModel CalcStatusModels { get; private set; }

        public LockDocInfModel DocInfModels { get; private set; }

        public string FunctionName
        {
            get
            {
                if (DocInfModels != null && !string.IsNullOrEmpty(DocInfModels.FunctionName))
                {
                    return DocInfModels.FunctionName;
                }
                if (CalcStatusModels != null)
                {
                    return "計算";
                }
                if (PatientInfoModels != null && !string.IsNullOrEmpty(PatientInfoModels.FunctionName))
                {
                    return PatientInfoModels.FunctionName;
                }
                return string.Empty;
            }
        }

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

        public string Machine
        {
            get
            {
                if (DocInfModels != null && !string.IsNullOrEmpty(DocInfModels.LockMachine))
                {
                    return DocInfModels.LockMachine;
                }
                if (CalcStatusModels != null && !string.IsNullOrEmpty(CalcStatusModels.CreateMachine))
                {
                    return CalcStatusModels.CreateMachine;
                }
                if (PatientInfoModels != null && !string.IsNullOrEmpty(PatientInfoModels.Machine))
                {
                    return PatientInfoModels.Machine;
                }
                return string.Empty;
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
                if (PatientInfoModels?.SinDateInt > 0)
                {
                    return PatientInfoModels.SinDateInt;
                }
                return 0;
            }
        }

        public DateTime LockDate
        {
            get
            {
                if (DocInfModels != null && (DocInfModels.LockDate != null || DocInfModels.LockDate != new DateTime()))
                {
                    return DocInfModels.LockDate;
                }
                if (CalcStatusModels != null && (CalcStatusModels.LockDate != null || CalcStatusModels.LockDate != new DateTime()))
                {
                    return CalcStatusModels.LockDate;
                }
                if (PatientInfoModels != null && (PatientInfoModels.LockDate != null || PatientInfoModels.LockDate != new DateTime()))
                {
                    return PatientInfoModels.LockDate;
                }
                return new DateTime();
            }
        }

        public bool CheckDefaultValue()
        {
            return SinDate == 0;
        }

        public string PtNumDisplay
        {
            get => PtNum > 0 ? PtNum.AsString() : string.Empty;
        }

        public string SinDateDisplay
        {
            get => SinDate > 0 ? CIUtil.SDateToShowSDate(SinDate) : string.Empty;
        }

        public string LockDateDisplay
        {
            get => LockDate != new DateTime() ? CIUtil.GetCIDateTimeStr(LockDate, true) : string.Empty;
        }
    }
}
