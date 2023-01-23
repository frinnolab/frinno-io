using frinno_application.Articles;
using frinno_core.Entities.Articles;
using frinno_core.Entities.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
namespace frinno_infrastructure.Repostories
{
    public class ArticlesRepository : IArticlesService<Article>
    {
        public Article AddNew(Article newData)
        {
            throw new NotImplementedException();
        }

        public Article CreateAticlesWithTag(int articleId, Tag tag)
        {
            throw new NotImplementedException();
        }

        public Article CreateAticlesWithTags(int articleId, Tag[] tags)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Article> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Article> GetAllBy(Expression<Func<Article, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Article GetSingle(Article data)
        {
            throw new NotImplementedException();
        }

        public Article GetSingleById(int dataId)
        {
            throw new NotImplementedException();
        }

        public void Remove(int dataId)
        {
            throw new NotImplementedException();
        }

        public void SaveContextChanges()
        {
            throw new NotImplementedException();
        }

        public Article Update(Article updateData)
        {
            throw new NotImplementedException();
        }
    }
}
