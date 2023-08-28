using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.SavePtKyusei;

public class SavePtKyuseiOutputData : IOutputData
{
    public SavePtKyuseiOutputData(SavePtKyuseiStatus status)
    {
        Status = status;
    }

    public SavePtKyuseiStatus Status { get; private set; }
}
