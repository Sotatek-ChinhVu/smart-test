namespace Domain.Models.Yousiki
{
    public class CategoryModel
    {
        public CategoryModel(int dataType, int isDeleted) 
        {
            DataType = dataType;
            IsDeleted = isDeleted;
        }

        public int DataType {  get; private set; }

        public int IsDeleted {  get; private set; }
    }
}
