﻿using UseCase.Core.Sync.Core;

namespace Schema.Insurance.SaveInsuranceScan
{
    public class SaveInsuranceScanInputData : IInputData<SaveInsuranceScanOutputData>
    {
        public SaveInsuranceScanInputData(int hpId, long ptId , int hokenGrp, int hokenId, string urlOldImage, int userId, Stream streamImage)
        {
            HpId = hpId;
            PtId = ptId;
            HokenGrp = hokenGrp;
            HokenId = hokenId;
            UrlOldImage = urlOldImage;
            UserId = userId;
            StreamImage = streamImage;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int HokenGrp { get; private set; }

        public int HokenId { get; private set; }

        public string UrlOldImage { get; private set; }

        public int UserId { get; private set; }

        public Stream StreamImage { get; private set; }
    }
}
