﻿namespace EmrCloudApi.Configs.Options;

public class JwtOptions
{
    public const string Position = "Jwt";

    public string Secret { get; set; } = string.Empty;

    public int TokenExpires { get; set; } //hours

    public int RefreshTokenExpires { get; set; } //hours.
}
