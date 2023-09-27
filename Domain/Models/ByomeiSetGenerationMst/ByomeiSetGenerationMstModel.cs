using Helper.Common;

namespace Domain.Models.ByomeiSetGenerationMst
{
    public class ByomeiSetGenerationMstModel
    {
        public ByomeiSetGenerationMstModel(int hpId, int generationId, int startDate, int isDeleted)
        {
            HpId = hpId;
            GenerationId = generationId;
            StartDate = startDate;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }
        public int GenerationId { get; private set; }
        public int StartDate { get; private set; }
        public int IsDeleted { get; private set; }

        public int SinDate { get { return StartDate; } }
        public string Text
        {
            get
            {
                if (StartDate > 0)
                {
                    return CIUtil.IntToDate(StartDate).ToString("yyyy/MM") + "～";
                }
                else
                {
                    return "0000/00～";
                }
            }
        }
    }
}
