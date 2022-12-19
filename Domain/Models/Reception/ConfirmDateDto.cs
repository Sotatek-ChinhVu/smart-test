namespace Domain.Models.Reception;

public class ConfirmDateDto
{
    public int SinDate { get; set; }

    public long SeqNo { get; set; }

    public string Comment { get; set; } = string.Empty;

    public bool IsDelete { get; set; }
}
