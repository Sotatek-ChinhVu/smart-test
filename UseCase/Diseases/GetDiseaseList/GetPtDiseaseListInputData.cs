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
    public class GetPtDiseaseListInputData : IInputData<GetPtDiseaseListOutputData>
    {
        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public int HokenId { get; private set; }
        public DiseaseViewType RequestFrom { get; private set; }

        public GetPtDiseaseListInputData(int hpId, long ptId, int sinDate, int hokenId, int diseaseViewModel)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            HokenId = hokenId;
            RequestFrom = (DiseaseViewType) Enum.Parse(typeof(DiseaseViewType), diseaseViewModel.ToString());
        }
    }
}
