namespace UseCase.SuperAdmin.RevokeInsertPermission
{
    public class DbInfoItem
    {
        public DbInfoItem(string dbName, string endPointDb)
        {
            DbName = dbName;
            EndPointDb = endPointDb;
        }
        public string DbName { private set; get; }
        public string EndPointDb { private set; get; }
    }
}
