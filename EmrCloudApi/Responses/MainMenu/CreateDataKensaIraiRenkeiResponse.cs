﻿namespace EmrCloudApi.Responses.MainMenu;

public class CreateDataKensaIraiRenkeiResponse
{
    public CreateDataKensaIraiRenkeiResponse(bool successed)
    {
        Successed = successed;
    }

    public bool Successed { get; private set; }
}