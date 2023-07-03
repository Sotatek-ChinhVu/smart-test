﻿using UseCase.Core.Sync.Core;
using UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetByomeiInput;
using UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetKarteInput;
using UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetOrderInput;

namespace UseCase.SuperSetDetail.SaveSuperSetDetail;

public class SaveSuperSetDetailInputData : IInputData<SaveSuperSetDetailOutputData>
{
    public SaveSuperSetDetailInputData(long ptId, long raiinNo, int sinDate, int setCd, int userId, int hpId, List<SaveSetByomeiInputItem> setByomeiModelInputs, SaveSetKarteInputItem saveSetKarteInputItem, List<SaveSetOrderInfInputItem> saveSetOrderInputItems, FileItemInputItem fileItem)
    {
        PtId = ptId;
        RaiinNo = raiinNo;
        SinDate = sinDate;
        SetCd = setCd;
        UserId = userId;
        HpId = hpId;
        SetByomeiModelInputs = setByomeiModelInputs;
        SaveSetKarteInputItem = saveSetKarteInputItem;
        SaveSetOrderInputItems = saveSetOrderInputItems;
        FileItem = fileItem;
    }

    public long PtId { get; private set; }

    public long RaiinNo { get; private set; }

    public int SinDate { get; private set; }

    public int SetCd { get; private set; }

    public int UserId { get; private set; }

    public int HpId { get; private set; }

    public List<SaveSetByomeiInputItem> SetByomeiModelInputs { get; private set; }

    public SaveSetKarteInputItem SaveSetKarteInputItem { get; private set; }

    public List<SaveSetOrderInfInputItem> SaveSetOrderInputItems { get; private set; }

    public FileItemInputItem FileItem { get; private set; }
}
