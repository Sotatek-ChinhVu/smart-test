namespace Helper.Constants;

public class StatusTenantDisplayConst
{
    public static Dictionary<byte, byte> StatusTenantDisplayDictionnary = new()
    {
        {2, 1}, {3, 1}, {5,1}, {6,1}, {8,1}, //pending
        {7, 2}, {10, 2}, {13,2}, {16,2}, //failded
        {1, 3}, {9,3}, //running
        //stopping
        {14, 5}, //stopped
        {4, 6}, //sutting-down
        {12, 7}, //teminated
    };
}
