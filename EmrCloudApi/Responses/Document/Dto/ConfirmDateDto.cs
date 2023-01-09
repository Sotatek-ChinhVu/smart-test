<<<<<<<< HEAD:EmrCloudApi/Tenant/Requests/ConfirmDate/ConfirmDateDto.cs
﻿namespace EmrCloudApi.Tenant.Requests.ConfirmDate
========
﻿namespace EmrCloudApi.Responses.Document.Dto
>>>>>>>> develop:EmrCloudApi/Responses/Document/Dto/ConfirmDateDto.cs
{
    public class ConfirmDateDto
    {
        public ConfirmDateDto(int hokenGrp, int hokenId, long seqNo, int checkId, string checkName, string checkComment, int confirmDate)
        {
            HokenGrp = hokenGrp;
            HokenId = hokenId;
            SeqNo = seqNo;
            CheckId = checkId;
            CheckName = checkName;
            CheckComment = checkComment;
            ConfirmDate = confirmDate;
        }

        public int HokenGrp { get; private set; }

        public int HokenId { get; private set; }

        public long SeqNo { get; private set; }

        public int CheckId { get; private set; }

        public string CheckName { get; private set; }

        public string CheckComment { get; private set; }

        public int ConfirmDate { get; private set; }
    }
}