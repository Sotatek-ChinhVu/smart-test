namespace EmrCloudApi.Requests.Todo;

public class UpsertTodoGrpMstRequest
{
    public List<TodoGrpMstModel> UpsertTodoGrpMst { get; set; } = new List<TodoGrpMstModel>();

    public class TodoGrpMstModel
    {
        public int TodoGrpNo { get; set; }

        public string TodoGrpName { get; set; } = string.Empty;

        public string GrpColor { get; set; } = string.Empty;

        public int SortNo { get; set; }

        public int IsDeleted { get; set; }
    }
}


