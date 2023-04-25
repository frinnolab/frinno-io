using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.Entities;

namespace frinno_application.Generics
{
    public interface IDataManager<T>
    {
        Task<T> AddNew(T newData);
         Task<T> Update(T updateData);
        void Remove(T data);
        void SaveContextChanges();
    }
}