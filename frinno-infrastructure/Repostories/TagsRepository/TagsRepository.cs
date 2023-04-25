using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Tags;
using frinno_core.Entities.Tags;
using frinno_infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace frinno_infrastructure.Repostories.TagsRepository
{
    public class TagsRepository : ITagsService<Tag>
    {
        private readonly DataContext dataContext;
        public TagsRepository(DataContext DB)
        {
            dataContext = DB;
        }
        public async Task<Tag> AddNew(Tag newData)
        {
            var data = await dataContext.Tags.AddAsync(newData);
            await dataContext.SaveChangesAsync();
            return data.Entity;
        }

        public async Task<IEnumerable<Tag>> FetchAll()
        {
            return await dataContext.Tags
            .Include(p=>p.Profile)
            .Include(a=>a.ArticleTags)
            .ThenInclude(at=>at.Article)
            .ToListAsync();
        }

        public Tag FetchSingle(Tag data)
        {
            return dataContext.Tags
            .Include(p=>p.ArticleTags)
            .ThenInclude(p=>p.Article)
            .Include(t=>t.Profile)
            .Single((a)=>a==data);
        }

        public Tag FetchSingleById(int dataId)
        {
            return dataContext.Tags
            .Include(p=>p.ArticleTags)
            .ThenInclude(p=>p.Article)
            .Include(t=>t.Profile)
            .Single(a=>a.Id == dataId);
        }

        public void Remove(Tag data)
        {
            dataContext.Tags.Remove(data);
            SaveContextChanges();
        }

        public void SaveContextChanges()
        {
            dataContext.SaveChanges();
        }

        public async Task<Tag> Update(Tag updateData)
        {
            var data = dataContext.Tags.Update(updateData);
            await dataContext.SaveChangesAsync();
            return data.Entity;
        }
    }
}