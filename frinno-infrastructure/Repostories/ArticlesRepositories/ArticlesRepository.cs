using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Articles;
using frinno_core.Entities.Article.Aggregates;
using frinno_core.Entities.Articles;
using frinno_core.Entities.Tags;
using frinno_infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace frinno_infrastructure.Repostories.ArticlesRepositories
{
    public class ArticlesRepository : IArticlesService<Article>
    {
        private readonly DataContext DB;
        public ArticlesRepository(DataContext data)
        {
            DB = data;
        }
        public async Task<Article> AddNew(Article newData)
        {
            var data = await DB.Articles.AddAsync(newData);
            await DB.SaveChangesAsync();
            return data.Entity;
        }

        public bool Exists(Article data)
        {
            return DB.Articles.Any((a)=>a==data);
        }

        public async Task<IEnumerable<Article>> FetchAll()
        {
            return await DB.Articles
            .Include(t=>t.ArticleTags)
            .Include(p=>p.Author)
            .ThenInclude(a=>a.Address)
            .ToListAsync();
        }

        public Article FetchSingle(Article data)
        {
            return DB.Articles
            .Include(p=>p.Author)
            .ThenInclude(a=>a.Address)
            .Include(p=>p.Author)
            .Include(t=>t.ArticleTags)
            .Single((a)=>a==data);
        }

        public Article FetchSingleById(int dataId)
        {
            return DB.Articles
            .Include(p=>p.Author)
            .ThenInclude(a=>a.Address)
            .Include(p=>p.Author)
            .Include(t=>t.ArticleTags)
            .Single(a=>a.Id == dataId);
        }

        public void Remove(Article data)
        {
            DB.Articles.Remove(data);
            SaveContextChanges();
        }

        public void SaveContextChanges()
        {
            DB.SaveChanges();
        }

        public async Task<Article> Update(Article updateData)
        {
            var data = DB.Articles.Update(updateData);
            await DB.SaveChangesAsync();
            return data.Entity;
        }

        public ArticleLike AddLikes(ArticleLike data)
        {
            var dataSave = DB.Add(data);
            DB.SaveChanges();

            return dataSave.Entity;
        }
    }
}