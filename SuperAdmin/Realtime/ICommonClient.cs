﻿namespace SuperAdmin.Realtime;

public interface ICommonClient
{
    Task ReceiveMessage(string functionCode, object message);
}
