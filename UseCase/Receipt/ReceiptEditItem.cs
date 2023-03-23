﻿using Domain.Models.Receipt;
using Helper.Common;

namespace UseCase.Receipt;

public class ReceiptEditItem
{
    public ReceiptEditItem()
    {
        Tokki1 = string.Empty;
        Tokki2 = string.Empty;
        Tokki3 = string.Empty;
        Tokki4 = string.Empty;
        Tokki5 = string.Empty;
        HokenNissu = -1;
        Kohi1Nissu = -1;
        Kohi2Nissu = -1;
        Kohi3Nissu = -1;
        Kohi4Nissu = -1;
        Kohi1ReceKyufu = -1;
        Kohi2ReceKyufu = -1;
        Kohi3ReceKyufu = -1;
        Kohi4ReceKyufu = -1;
        HokenReceTensu = -1;
        HokenReceFutan = -1;
        Kohi1ReceTensu = -1;
        Kohi1ReceFutan = -1;
        Kohi2ReceTensu = -1;
        Kohi2ReceFutan = -1;
        Kohi3ReceTensu = -1;
        Kohi3ReceFutan = -1;
        Kohi4ReceTensu = -1;
        Kohi4ReceFutan = -1;
        Tokki1Id = CIUtil.Copy(Tokki1, 1, 2);
        Tokki2Id = CIUtil.Copy(Tokki2, 1, 2);
        Tokki3Id = CIUtil.Copy(Tokki3, 1, 2);
        Tokki4Id = CIUtil.Copy(Tokki4, 1, 2);
        Tokki5Id = CIUtil.Copy(Tokki5, 1, 2);
    }

    public ReceiptEditItem(ReceiptEditModel model)
    {
        SeqNo = model.SeqNo;
        Tokki1 = model.Tokki1;
        Tokki2 = model.Tokki2;
        Tokki3 = model.Tokki3;
        Tokki4 = model.Tokki4;
        Tokki5 = model.Tokki5;
        HokenNissu = model.HokenNissu;
        Kohi1Nissu = model.Kohi1Nissu;
        Kohi2Nissu = model.Kohi2Nissu;
        Kohi3Nissu = model.Kohi3Nissu;
        Kohi4Nissu = model.Kohi4Nissu;
        Kohi1ReceKyufu = model.Kohi1ReceKyufu;
        Kohi2ReceKyufu = model.Kohi2ReceKyufu;
        Kohi3ReceKyufu = model.Kohi3ReceKyufu;
        Kohi4ReceKyufu = model.Kohi4ReceKyufu;
        HokenReceTensu = model.HokenReceTensu;
        HokenReceFutan = model.HokenReceFutan;
        Kohi1ReceTensu = model.Kohi1ReceTensu;
        Kohi1ReceFutan = model.Kohi1ReceFutan;
        Kohi2ReceTensu = model.Kohi2ReceTensu;
        Kohi2ReceFutan = model.Kohi2ReceFutan;
        Kohi3ReceTensu = model.Kohi3ReceTensu;
        Kohi3ReceFutan = model.Kohi3ReceFutan;
        Kohi4ReceTensu = model.Kohi4ReceTensu;
        Kohi4ReceFutan = model.Kohi4ReceFutan;
        Tokki1Id = CIUtil.Copy(Tokki1, 1, 2);
        Tokki2Id = CIUtil.Copy(Tokki2, 1, 2);
        Tokki3Id = CIUtil.Copy(Tokki3, 1, 2);
        Tokki4Id = CIUtil.Copy(Tokki4, 1, 2);
        Tokki5Id = CIUtil.Copy(Tokki5, 1, 2);
    }

    public ReceiptEditItem(ReceInfModel receInf)
    {
        SeqNo = int.MaxValue;
        Tokki1 = receInf.Tokki1;
        Tokki2 = receInf.Tokki2;
        Tokki3 = receInf.Tokki3;
        Tokki4 = receInf.Tokki4;
        Tokki5 = receInf.Tokki5;
        HokenNissu = receInf.HokenNissu;
        Kohi1Nissu = receInf.Kohi1Nissu;
        Kohi2Nissu = receInf.Kohi2Nissu;
        Kohi3Nissu = receInf.Kohi3Nissu;
        Kohi4Nissu = receInf.Kohi4Nissu;
        Kohi1ReceKyufu = receInf.Kohi1ReceKyufu;
        Kohi2ReceKyufu = receInf.Kohi2ReceKyufu;
        Kohi3ReceKyufu = receInf.Kohi3ReceKyufu;
        Kohi4ReceKyufu = receInf.Kohi4ReceKyufu;
        HokenReceTensu = receInf.HokenReceTensu;
        HokenReceFutan = receInf.HokenReceFutan;
        Kohi1ReceTensu = receInf.Kohi1ReceTensu;
        Kohi1ReceFutan = receInf.Kohi1ReceFutan;
        Kohi2ReceTensu = receInf.Kohi2ReceTensu;
        Kohi2ReceFutan = receInf.Kohi2ReceFutan;
        Kohi3ReceTensu = receInf.Kohi3ReceTensu;
        Kohi3ReceFutan = receInf.Kohi3ReceFutan;
        Kohi4ReceTensu = receInf.Kohi4ReceTensu;
        Kohi4ReceFutan = receInf.Kohi4ReceFutan;
        Tokki1Id = CIUtil.Copy(Tokki1, 1, 2);
        Tokki2Id = CIUtil.Copy(Tokki2, 1, 2);
        Tokki3Id = CIUtil.Copy(Tokki3, 1, 2);
        Tokki4Id = CIUtil.Copy(Tokki4, 1, 2);
        Tokki5Id = CIUtil.Copy(Tokki5, 1, 2);
    }

    // contructor for save
    public ReceiptEditItem(int seqNo, int hokenNissu, int kohi1Nissu, int kohi2Nissu, int kohi3Nissu, int kohi4Nissu, int kohi1ReceKyufu, int kohi2ReceKyufu, int kohi3ReceKyufu, int kohi4ReceKyufu, int hokenReceTensu, int hokenReceFutan, int kohi1ReceTensu, int kohi1ReceFutan, int kohi2ReceTensu, int kohi2ReceFutan, int kohi3ReceTensu, int kohi3ReceFutan, int kohi4ReceTensu, int kohi4ReceFutan, bool isDeleted, string tokki1Id, string tokki2Id, string tokki3Id, string tokki4Id, string tokki5Id)
    {
        SeqNo = seqNo;
        HokenNissu = hokenNissu;
        Kohi1Nissu = kohi1Nissu;
        Kohi2Nissu = kohi2Nissu;
        Kohi3Nissu = kohi3Nissu;
        Kohi4Nissu = kohi4Nissu;
        Kohi1ReceKyufu = kohi1ReceKyufu;
        Kohi2ReceKyufu = kohi2ReceKyufu;
        Kohi3ReceKyufu = kohi3ReceKyufu;
        Kohi4ReceKyufu = kohi4ReceKyufu;
        HokenReceTensu = hokenReceTensu;
        HokenReceFutan = hokenReceFutan;
        Kohi1ReceTensu = kohi1ReceTensu;
        Kohi1ReceFutan = kohi1ReceFutan;
        Kohi2ReceTensu = kohi2ReceTensu;
        Kohi2ReceFutan = kohi2ReceFutan;
        Kohi3ReceTensu = kohi3ReceTensu;
        Kohi3ReceFutan = kohi3ReceFutan;
        Kohi4ReceTensu = kohi4ReceTensu;
        Kohi4ReceFutan = kohi4ReceFutan;
        IsDeleted = isDeleted;
        Tokki1Id = tokki1Id;
        Tokki2Id = tokki2Id;
        Tokki3Id = tokki3Id;
        Tokki4Id = tokki4Id;
        Tokki5Id = tokki5Id;
        Tokki1 = string.Empty;
        Tokki2 = string.Empty;
        Tokki3 = string.Empty;
        Tokki4 = string.Empty;
        Tokki5 = string.Empty;
    }

    public int SeqNo { get; private set; }

    public string Tokki1 { get; private set; }

    public string Tokki2 { get; private set; }

    public string Tokki3 { get; private set; }

    public string Tokki4 { get; private set; }

    public string Tokki5 { get; private set; }

    public int HokenNissu { get; private set; }

    public int Kohi1Nissu { get; private set; }

    public int Kohi2Nissu { get; private set; }

    public int Kohi3Nissu { get; private set; }

    public int Kohi4Nissu { get; private set; }

    public int Kohi1ReceKyufu { get; private set; }

    public int Kohi2ReceKyufu { get; private set; }

    public int Kohi3ReceKyufu { get; private set; }

    public int Kohi4ReceKyufu { get; private set; }

    public int HokenReceTensu { get; private set; }

    public int HokenReceFutan { get; private set; }

    public int Kohi1ReceTensu { get; private set; }

    public int Kohi1ReceFutan { get; private set; }

    public int Kohi2ReceTensu { get; private set; }

    public int Kohi2ReceFutan { get; private set; }

    public int Kohi3ReceTensu { get; private set; }

    public int Kohi3ReceFutan { get; private set; }

    public int Kohi4ReceTensu { get; private set; }

    public int Kohi4ReceFutan { get; private set; }

    public bool IsDeleted { get; private set; }

    public string Tokki1Id { get; private set; }

    public string Tokki2Id { get; private set; }

    public string Tokki3Id { get; private set; }

    public string Tokki4Id { get; private set; }

    public string Tokki5Id { get; private set; }
}
