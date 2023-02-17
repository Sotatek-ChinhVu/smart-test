﻿using System.Xml.Linq;
using static Helper.Constants.ApprovalInfConstant;

namespace Domain.Models.ApprovalInfo
{
    public class ApprovalInfModel
    {
        public ApprovalInfModel(int hpId, int id, long raiinNo, int seqNo, long ptId, int sinDate, int isDeleted, long ptNum, string kanaName, string name, int kaId, int uketukeNo)
        {
            HpId = hpId;
            Id = id;
            RaiinNo = raiinNo;
            SeqNo = seqNo;
            PtId = ptId;
            SinDate = sinDate;
            IsDeleted = isDeleted;
            PtNum = ptNum;
            KanaName = kanaName;
            Name = name;
            UketokeNo = uketukeNo;
            KaId = kaId;
        }
        public ApprovalInfModel(int id, int hpId, long ptId, int sinDate, long raiinNo, int isDeleted)
        {
            Id = id;
            HpId = hpId;
            IsDeleted = isDeleted;
            RaiinNo = raiinNo;
            PtId = ptId;
            SinDate = sinDate;
            KanaName = string.Empty;
            Name = string.Empty;
        }
        public int HpId { get; private set; }

        public int Id { get; private set; }

        public long RaiinNo { get; private set; }

        public int SeqNo { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public int IsDeleted { get; private set; }

        public long PtNum { get; private set; }

        public string KanaName { get; private set; }

        public string Name { get; private set; }

        public int UketokeNo { get; private set; }

        public int KaId { get; private set; }

        public ValidationStatus Validation()
        {
            if(HpId < 0)
            {
                return ValidationStatus.InvalidHpId;
            }
            if(Id < 0)
            {
                return ValidationStatus.InvalidId;
            }
            if(RaiinNo < 0)
            {
                return ValidationStatus.InvalidRaiinNo;
            }
            if(PtId < 0)
            {
                return ValidationStatus.InvalidPtId;
            } 
            if(SinDate < 0)
            {
                return ValidationStatus.InvalidSinDate;
            }   
            if(IsDeleted != 0 || IsDeleted != 1)
            {
                return ValidationStatus.InvalidIsDeleted;
            }  
            
            return ValidationStatus.Valid;
        }
    }
}
