using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Articles;
using frinno_core.Entities.Articles;
using frinno_core.Entities.Tags;
using frinno_infrastructure.Data;

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

        // public Article CreateAticlesWithTag(int articleId, Tag tag)
        // {
        //     var article = new Article();

        //     if(articleId>0)
        //     {
        //         article = DB.Articles.Find(articleId);
        //     }
        // }

        // public Article CreateAticlesWithTags(int articleId, Tag[] tags)
        // {
        //     throw new NotImplementedException();
        // }

        public IEnumerable<Article> FetchAll()
        {
            return DB.Articles.ToList();
        }

        public Article FetchSingle(Article data)
        {
            return DB.Articles.FirstOrDefault((a)=>a==data);
        }

        public Article FetchSingleById(int dataId)
        {
            return DB.Articles.Find(dataId);
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