using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.PatientComment
{
    public class PatientCommentModel
    {
        public PatientCommentModel(int hpId, long ptId, string? text)
        {
            HpId = hpId;
            PtId = ptId;
            Text = text;
        }

        public int HpId { get; set; }
        public long PtId { get; set; }
        public string? Text { get; set; } = string.Empty;
    }
}
