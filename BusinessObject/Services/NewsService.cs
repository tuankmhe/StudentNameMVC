using BusinessObject.Repository;
using DataAccessLayer.Models;

namespace BusinessObject.Services
{
    public class NewsService : INewsService
    {
        private readonly IRepository<NewsArticle> _newsRepository;
        private readonly IRepository<Tag> _tagRepository;

        public NewsService(IRepository<NewsArticle> newsRepository, IRepository<Tag> tagRepository)
        {
            _newsRepository = newsRepository;
            _tagRepository = tagRepository;
        }

        // 1. Tạo bài viết kèm danh sách Tag
        public void CreateNews(NewsArticle news, List<string> tagNames)
        {
            // Nếu chưa có ID, tạo mới bằng GUID
            if (string.IsNullOrEmpty(news.NewsArticleId))
            {
                news.NewsArticleId = Guid.NewGuid().ToString();
            }

            // Thêm bài viết
            _newsRepository.Insert(news);
            _newsRepository.Save(); // Sau khi lưu, news.NewsArticleId đã có giá trị (nếu dùng Identity hoặc tự gán)

            // Thêm các Tag liên quan nếu có
            //if (tagNames != null && tagNames.Any())
            //{
            //    foreach (var tagName in tagNames)
            //    {
            //        if (!string.IsNullOrWhiteSpace(tagName))
            //        {
            //            var tag = new Tag
            //            {
            //                TagName = tagName.Trim(),
            //                Note = string.Empty
            //            };
            //            // Giả sử mỗi tag chỉ được gán cho 1 bài viết
            //            tag.NewsArticles.Add(news);
            //            news.Tags.Add(tag);

            //            _tagRepository.Insert(tag);
            //        }
            //    }
            //    _tagRepository.Save();
            //}
        }

        // 2. Cập nhật bài viết và danh sách Tag
        public void UpdateNews(NewsArticle news, List<string> tagNames)
        {
            // Cập nhật bài viết
            _newsRepository.Update(news);
            _newsRepository.Save();

            // Xóa các Tag cũ liên quan đến bài viết này
            var existingTags = _tagRepository.GetAll()
                .Where(t => t.NewsArticles.Any(n => n.NewsArticleId == news.NewsArticleId))
                .ToList();
            //foreach (var tag in existingTags)
            //{
            //    // Loại bỏ liên kết giữa tag và bài viết
            //    tag.NewsArticles.Remove(news);
            //    // Nếu tag không còn liên kết nào, xóa hoàn toàn
            //    if (!tag.NewsArticles.Any())
            //    {
            //        _tagRepository.Delete(tag);
            //    }
            //}
            //_tagRepository.Save();

            //// Thêm các Tag mới
            //if (tagNames != null && tagNames.Any())
            //{
            //    foreach (var tagName in tagNames)
            //    {
            //        if (!string.IsNullOrWhiteSpace(tagName))
            //        {
            //            var newTag = new Tag
            //            {
            //                TagName = tagName.Trim(),
            //                Note = string.Empty
            //            };
            //            newTag.NewsArticles.Add(news);
            //            news.Tags.Add(newTag);

            //            _tagRepository.Insert(newTag);
            //        }
            //    }
            //    _tagRepository.Save();
            //}
        }

        // 3. Lấy toàn bộ bài viết
        public IEnumerable<NewsArticle> GetAllNews()
        {
            return _newsRepository.GetAll();
        }

        // 4. Lấy bài viết theo staff (CreatedById)
        public IEnumerable<NewsArticle> GetNewsByStaff(short staffId)
        {
            return _newsRepository.GetAll()
                .Where(n => n.CreatedById.HasValue && n.CreatedById.Value == staffId);
        }

        // 5. Tìm kiếm bài viết của staff theo keyword (trong tiêu đề hoặc nội dung)
        public IEnumerable<NewsArticle> SearchNewsByStaff(short staffId, string keyword)
        {
            return _newsRepository.GetAll()
                .Where(n => n.CreatedById.HasValue && n.CreatedById.Value == staffId &&
                            ((n.NewsTitle != null && n.NewsTitle.Contains(keyword)) ||
                             (n.NewsContent != null && n.NewsContent.Contains(keyword))));
        }

        // 6. Xóa bài viết theo NewsArticleId (kiểu string)
        public void DeleteNews(string newsArticleId)
        {
            var news = _newsRepository.GetAll().FirstOrDefault(n => n.NewsArticleId == newsArticleId);
            if (news != null)
            {
                _newsRepository.Delete(news);
                _newsRepository.Save();
            }
        }
    }

}
