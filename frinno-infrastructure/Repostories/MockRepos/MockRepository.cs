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
            mockData.SaveChanges();
        }

        public IEnumerable<object> FindMockAllDatas()
        {
            var items = mockData.MockArticles.ToList();

            return items;
        }

        public object FindMockDataById(int mockDataId)
        {
            var data = mockData.MockArticles.Find(mockDataId);

            return data;
        }

        public void RemoveMockData(int mockDataId)
        {
            var data = mockData.MockArticles.Find(mockData);
            mockData.Remove(data);
            mockData.SaveChanges();

        }
    }
}