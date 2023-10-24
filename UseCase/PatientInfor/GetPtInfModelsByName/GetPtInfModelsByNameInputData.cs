using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.GetPtInfModelsByName;

public class GetPtInfModelsByNameInputData : IInputData<GetPtInfModelsByNameOutputData>
{
    public GetPtInfModelsByNameInputData(int hpId, string kanaName, string name, int birthDate, int sex1, int sex2)
    {
        HpId = hpId;
        KanaName = kanaName;
        Name = name;
        BirthDate = birthDate;
        Sex1 = sex1;
        Sex2 = sex2;
    }

    public int HpId { get; private set; }

    public string KanaName { get; private set; }

    public string Name { get; private set; }

    public int BirthDate { get; private set; }

    public int Sex1 { get; private set; }

    public int Sex2 { get; private set; }
}
