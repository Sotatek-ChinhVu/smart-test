namespace EmrCloudApi.Requests.Todo

{
    public class UpsertTodoGrpMstRequest
    {
        public List<UpsertTodoGrpMstListRequest> TodoGrpMstList { get; set; } = new List<UpsertTodoGrpMstListRequest>();
    }


    public class UpsertTodoGrpMstListRequest
    {
        public int TodoGrpNo { get; set; }

        public string TodoGrpName { get; set; } = string.Empty;

        public string GrpColor { get; set; } = string.Empty;

        public int SortNo { get; set; }

        public int IsDeleted { get; set; }
    }
}


