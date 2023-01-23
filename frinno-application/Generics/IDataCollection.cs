using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.Entities;

namespace frinno_application.Generics
{
    public interface IDataCollection <T>
    {
        T GetSingle(T data);
        T GetSingleById(int dataId);
        IEnumerable<T> GetAll();
    }
}