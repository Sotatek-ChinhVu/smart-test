﻿namespace EmrCloudApi.Common;

public static class ObjectExtentions
{
    public static async Task<MemoryStream> ToMemoryStreamAsync(this Stream input)
    {
        byte[] buffer = new byte[16 * 1024];

        int read;
        MemoryStream ms = new MemoryStream();
        while ((read = (await input.ReadAsync(buffer, 0, buffer.Length))) > 0)
        {
            ms.Write(buffer, 0, read);
        }
        return ms;
    }
}
