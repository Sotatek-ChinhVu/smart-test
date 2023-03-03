using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetDefaultSelectedTime
{
    public class GetDefaultSelectedTimeInputData : IInputData<GetDefaultSelectedTimeOutputData>
    {
        public GetDefaultSelectedTimeInputData(int dayOfWeek, int uketukeTime, bool fromOutOfSystem, int hpId, int sinDate, int birthDay)
        {
            DayOfWeek = dayOfWeek;
            UketukeTime = uketukeTime;
            FromOutOfSystem = fromOutOfSystem;
            HpId = hpId;
            SinDate = sinDate;
            BirthDay = birthDay;
        }

        public int HpId { get; private set; }

        public int SinDate { get; private set; }

        public int DayOfWeek { get; private set; }

        public int UketukeTime { get; private set; }

        public int BirthDay { get; private set; }

        public bool FromOutOfSystem { get; private set; }
    }
}
