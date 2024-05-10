using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Interfaces
{
    public interface IGenericRepo<T> where T : class
    {
        Task<T?> GetById(int id);
        Task<IEnumerable<T>> GetAllThatMatchesACriteria(Expression<Func<T, bool>> criteria);
        Task<IEnumerable<T>> GetAll();
    }

}
