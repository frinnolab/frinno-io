using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.Entities;

namespace frinno_application.Generics
{
    public interface IDataCollection <T>
    {
        T FetchSingle(T data);
        T FetchSingleById(int dataId);
        IEnumerable<T> FetchAll();
    }
}