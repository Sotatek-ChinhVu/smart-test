﻿using UseCase.Family.GetListFamily;

namespace EmrCloudApi.Responses.Family;

public class FamilyDto
{
    public FamilyDto(FamilyOutputItem model)
    {
        SeqNo = model.SeqNo;
        ZokugaraCd = model.ZokugaraCd;
        FamilyPtNum = model.FamilyPtNum;
        Name = model.Name;
        Sex = model.Sex;
        Birthday = model.Birthday;
        Age = model.Age;
        IsDead = model.IsDead;
        IsSeparated = model.IsSeparated;
        Biko = model.Biko;
        SortNo = model.SortNo;
        BirthdayDisplay = model.BirthdayDisplay;
        ListPtFamilyReki = model.ListPtFamilyReki.Select(item => new PtFamilyRekiDto(item)).ToList();
    }

    public long SeqNo { get; private set; }

    public string ZokugaraCd { get; private set; }

    public long FamilyPtNum { get; private set; }

    public string Name { get; private set; }

    public int Sex { get; private set; }

    public int Birthday { get; private set; }

    public int Age { get; private set; }

    public int IsDead { get; private set; }

    public int IsSeparated { get; private set; }

    public string Biko { get; private set; }

    public int SortNo { get; private set; }

    public string BirthdayDisplay { get; private set; }

    public List<PtFamilyRekiDto> ListPtFamilyReki { get; private set; }
}
