using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using frinno_core.Entities;

namespace frinno_application.Generics
{
    public interface IMasterService <T> : IDataManager<T>, IDataCollection<T>  where T : BaseEntity 
    {
        IEnumerable<T> GetAllBy(Expression<Func<T, bool>> predicate);
    }
}