namespace TrinitySharp.SQL.Repositories
{
    public interface IRepository<T>
    {
        public IQueryable<T> GetAll();
        public T GetByID(int ID);
        public bool Save(T ObjIn);
        public bool Delete(int ID);
        public bool Update(T ObjIn);
    }
}
