namespace Domain.Models.Yousiki
{
    public class CategoryModel
    {
        public CategoryModel(int dataType, int isDeleted, int status) 
        {
            DataType = dataType;
            IsDeleted = isDeleted;
            Status = status;
        }

        public int DataType {  get; private set; }

        public int IsDeleted {  get; private set; }

        public int Status { get; private set; }
    }
}
