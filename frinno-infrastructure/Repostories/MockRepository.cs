using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Generics;
using frinno_core.Entities.MockModels;

namespace frinno_infrastructure.Repostories
{
    public class MockRepository : IMockingDataService
    {
        private readonly MockDataContext mockData;

        public MockRepository(MockDataContext context)
        {
            mockData = context;
        }
        public void AddBulkMockData(List<object> newMockDatas)
        {
            newMockDatas.ForEach(d=>mockData.MockArticles.Add(d as MockArticle));
            mockData.SaveChanges();
        }

        public void AddMockData(object newMockData)
        {
            var article = newMockData as MockArticle;
            mockData.MockArticles.Add(article);
        }

        public IEnumerable<object> FindMockAllDatas()
        {
            throw new NotImplementedException();
        }

        public void FindMockDataById(int mockDataId)
        {
            //..var data = mockData.Find(mockDataId);
        }

        public void RemoveMockData(int mockDataId)
        {
            throw new NotImplementedException();
        }
    }
}