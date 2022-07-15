using Domain.CommonObject;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.Diseases.GetDiseaseList
{
    public class GetDiseaseListInputData : IInputData<GetDiseaseListOutputData>
    {
        public HpId HpId { get; private set; }
        public PtId PtId { get; private set; }
        public SinDate SinDate { get; private set; }
        public DiseaseViewType RequestFrom { get; private set; } = DiseaseViewType.FromMedicalExamination;

        public GetDiseaseListInputData(int hpId, long ptId, int sinDate)
        {
            HpId = HpId.From(hpId);
            PtId = PtId.From(ptId);
            SinDate = SinDate.From(sinDate);
        }
    }
}
