using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Articles;
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
        public Article AddNew(Article newData)
        {
            var data = DB.Articles.Add(newData);
            SaveContextChanges();

            return data.Entity;
        }

        public bool Exists(Article data)
        {
            return DB.Articles.Any((a)=>a==data);
        }

        public IEnumerable<Article> FetchAll()
        {
            return DB.Articles
            .Include(p=>p.ProfileArticles)
            .ThenInclude(p=>p.Profile)
            .Include(t=>t.ArticleTags)
            .ToList();
        }

        public Article FetchSingle(Article data)
        {
            return DB.Articles
            .Include(p=>p.ProfileArticles)
            .ThenInclude(p=>p.Profile)
            .Include(t=>t.ArticleTags)
            .Single((a)=>a==data);
        }

        public Article FetchSingleById(int dataId)
        {
            return DB.Articles
            .Include(p=>p.ProfileArticles)
            .ThenInclude(p=>p.Profile)
            .Include(t=>t.ArticleTags)
            .Single(a=>a.ID == dataId);
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

        public Article Update(Article updateData)
        {
            var data = DB.Articles.Update(updateData);
            SaveContextChanges();

            return data.Entity;
        }
    }
}