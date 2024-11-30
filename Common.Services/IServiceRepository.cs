using Foodie.Models;
using Microsoft.EntityFrameworkCore;

namespace Foodie.Common.Services
{
    public interface IServiceRepository<t>
    {
        public IQueryable<t> List();
        public t Add(t model);
        public t Update(t model);
        public int Remove(t model);
        public int Delete(int id);
        public t Find(int id);
        public int AddRange(List<t> models);
        public int RemoveRange(List<t> models);
    }
    public class ServiceRepository<t> : IBaseService, IServiceRepository<t>, IDisposable where t : class
    {
        protected FoodieDbContext db;
        DbSet<t> entity;
        public ServiceRepository()
        {
            db = new FoodieDbContext();
            entity = db.Set<t>();
        }
        public ServiceRepository(FoodieDbContext db)
        {
            this.db = db;
            entity = db.Set<t>();
        }
        public ServiceRepository(FoodieDbContext db, DbSet<t> entity) : this(db)
        {
            this.entity = entity;
        }

        public IQueryable<t> List()
        {
            try
            {
                return entity;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual t Add(t model)
        {
            try
            {
                entity.Add(model);
                db.SaveChanges();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual t Update(t model)
        {
            try
            {
                db.ChangeTracker.Clear();
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Remove(t model)
        {
            try
            {
                entity.Remove(model);
                var result = db.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public t Find(int id)
        {
            try
            {
                return entity.Find(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddRange(List<t> models)
        {
            try
            {
                entity.AddRange(models);
                var result = db.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int RemoveRange(List<t> models)
        {
            try
            {
                entity.RemoveRange(models);
                var result = db.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public void Dispose()
        {
            //your memory
            //your connection
            //place to clean
        }

        public virtual int Delete(int id)
        {
            try
            {
                entity.Remove(entity.Find(id));

                var result = db.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
    public interface IServiceFactory
    {
        IServiceRepository<t> GetInstance<t>() where t : class;
        void BeginTransaction();
        void RollBack();
        void CommitTransaction();
        void WriteLog(string message, object exception, string v);
    }
    public class ServiceFactory : IServiceFactory, IDisposable
    {
        public FoodieDbContext db;


        public ServiceFactory(FoodieDbContext db)
        {
            this.db = db;
        }
        public void Dispose()
        {
            // throw new NotImplementedException();
            //db.Dispose();
        }
        public IServiceRepository<t> GetInstance<t>() where t : class
        {
            return new ServiceRepository<t>(db);
        }

        public void BeginTransaction()
        {
            db.Database.BeginTransaction();
        }
        public void RollBack()
        {
            db.Database.RollbackTransaction();
        }

        public void CommitTransaction()
        {
            db.Database.CommitTransaction();

        }

        public void WriteLog(string message, object exception, string v)
        {
            throw new NotImplementedException();
        }
    }
}
