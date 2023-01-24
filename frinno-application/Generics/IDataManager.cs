using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.Entities;

namespace frinno_application.Generics
{
    public interface IDataManager<T>
    {
        void AddNew(T newData);
        void Update(T updateData);
        void Remove(int dataId);
        void SaveContextChanges();
    }
}