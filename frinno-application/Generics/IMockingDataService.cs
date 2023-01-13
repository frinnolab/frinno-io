using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.Entities.MockModels;

namespace frinno_application.Generics
{
    public interface IMockingDataService
    {
        void AddMockData(object newMockData);
        void AddBulkMockData(List<object> newMockDatas);
        IEnumerable<object> FindMockAllDatas();
        void FindMockDataById(int mockDataId);
        void RemoveMockData(int mockDataId);
    }
}