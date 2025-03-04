using DataAccessLayer.Models;

namespace BusinessObject.Services
{
    public interface INewsService
    {
        void CreateNews(NewsArticle news, List<string> tagNames);
        void UpdateNews(NewsArticle news, List<string> tagNames);
        IEnumerable<NewsArticle> GetAllNews();
        IEnumerable<NewsArticle> GetNewsByStaff(short staffId);
        IEnumerable<NewsArticle> SearchNewsByStaff(short staffId, string keyword);
        void DeleteNews(string newsArticleId);
    }
}
