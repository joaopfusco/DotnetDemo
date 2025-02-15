using DotnetDemo.Domain.Models;
using DotnetDemo.Repository.Data;
using DotnetDemo.Service.Interfaces;
using System.Linq.Expressions;

namespace DotnetDemo.Service.Services
{
    public class BaseService<TModel> : IBaseService<TModel> where TModel : BaseModel
    {
        public AppDbContext _db;

        public BaseService(AppDbContext db)
        {
            _db = db;
        }

        public IQueryable<TModel> Get(Expression<Func<TModel, bool>>? predicate = null)
        {
            var query = _db.Set<TModel>().AsQueryable();
            if (predicate != null) query = query.Where(predicate);
            return query;
        }

        public IQueryable<TModel> Get(int id)
        {
            return Get(p => p.Id.Equals(id))
                .AsQueryable();
        }

        public virtual int Insert(TModel model)
        {
            try
            {
                _db.Add(model);
                return _db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual int Update(TModel model)
        {
            try
            {
                _db.Update(model);
                return _db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual int Delete(int id)
        {
            try
            {
                TModel model = Get(id).FirstOrDefault() ?? throw new Exception("Entity not found!");
                _db.Remove(model);
                return _db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
