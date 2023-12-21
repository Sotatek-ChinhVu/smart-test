namespace Helper.Constants;

public static class StatusTenantDisplayConst
{
    /// <summary>
    /// StatusTenantDisplayDictionnary to FE
    /// </summary>
    private static readonly Dictionary<byte, byte> statusTenantDisplayDictionnary = new()
    {
        {2, 1}, {3, 1}, {5,1}, {8,1}, //pending
        {7, 2}, {10, 2}, {13,2}, {16,2}, //failded
        {1, 3}, {9,3}, //running
        {17 ,4}, {18,4}, {6,4}, {15, 4}, //stopping
        {14, 5}, //stopped
        {4, 6}, {11, 6}, //sutting-down
        {12, 7}, //teminated
    };

    public static Dictionary<byte, byte> StatusTenantDisplayDictionnary => statusTenantDisplayDictionnary;
}
