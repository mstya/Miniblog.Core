using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Miniblog.Core.Entities;

namespace Miniblog.Core.Services
{
    //public class MssqlBlogService : IBlogService
    //{
    //    private BlogContext db;
    //    private readonly IHttpContextAccessor contextAccessor;

    //    public MssqlBlogService(BlogContext db, IHttpContextAccessor contextAccessor)
    //    {
    //        this.db = db;
    //        this.contextAccessor = contextAccessor;
    //    }

    //    public Task DeletePostAsync(Post post, CancellationToken token)
    //    {
    //        this.db.Posts.Remove(post);
    //        return this.db.SaveChangesAsync(token);
    //    }

    //    public Task<List<Category>> GetCategoriesAsync()
    //    {
    //        bool isAdmin = IsAdmin(); 

    //        return this.db.Posts.Where(p => p.IsPublished || isAdmin)
    //            .SelectMany(post => post.Categories)
    //            .Select(cat => cat)
    //            .Distinct()
    //                   .ToListAsync();
    //    }

    //    public Task<Post> GetPostByIdAsync(string id)
    //    {
    //        return this.db.Posts.Include(c => c.Categories).Include(x => x.Comments).FirstOrDefaultAsync(x => x.Id == id);
    //    }

    //    public Task<Post> GetPostBySlugAsync(string slug)
    //    {
    //        return this.db.Posts.Include(c => c.Categories).Include(x => x.Comments).FirstOrDefaultAsync(x => x.Slug == slug);
    //    }

    //    public Task<List<Post>> GetPostsAsync(int count, int skip = 0)
    //    {
    //        return this.db.Posts.Skip(skip).Take(count).Include(x => x.Comments).ToListAsync();
    //    }

    //    public Task<List<Post>> GetPostsByCategoryAsync(string category)
    //    {
    //        return this.db.Posts.Where(x => x.Categories.Any(c => c.Name == category)).ToListAsync();
    //    }

    //    public Task<string> SaveFileAsync(byte[] bytes, string fileName, string suffix = null)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public async Task SavePostAsync(Post post)
    //    {
    //        if(string.IsNullOrEmpty(post.Id))
    //        {
    //            post.Id = Guid.NewGuid().ToString();
    //            await this.db.Posts.AddAsync(post);
    //            await this.db.SaveChangesAsync();
    //            return;
    //        }

    //        this.db.Posts.Update(post);
    //        await this.db.SaveChangesAsync();
    //    }

    //    protected bool IsAdmin()
    //    {
    //        return this.contextAccessor.HttpContext?.User?.Identity.IsAuthenticated == true;
    //    }
    //}

    //public class FileBlogService : IBlogService
    //{
    //    private readonly List<PostViewModel> _cache = new List<PostViewModel>();
    //    private readonly IHttpContextAccessor _contextAccessor;
    //    private readonly string _folder;

    //    public FileBlogService(IHostingEnvironment env, IHttpContextAccessor contextAccessor)
    //    {
    //        _folder = Path.Combine(env.WebRootPath, "Posts");
    //        _contextAccessor = contextAccessor;

    //        Initialize();
    //    }

    //    public virtual Task<IEnumerable<PostViewModel>> GetPosts(int count, int skip = 0)
    //    {
    //        bool isAdmin = IsAdmin();

    //        var posts = _cache
    //            .Where(p => p.PubDate <= DateTime.UtcNow && (p.IsPublished || isAdmin))
    //            .Skip(skip)
    //            .Take(count);

    //        return Task.FromResult(posts);
    //    }

    //    public virtual Task<IEnumerable<PostViewModel>> GetPostsByCategory(string category)
    //    {
    //        bool isAdmin = IsAdmin();

    //        var posts = from p in _cache
    //                    where p.PubDate <= DateTime.UtcNow && (p.IsPublished || isAdmin)
    //                    where p.Categories.Contains(category, StringComparer.OrdinalIgnoreCase)
    //                    select p;

    //        return Task.FromResult(posts);

    //    }

    //    public virtual Task<PostViewModel> GetPostBySlug(string slug)
    //    {
    //        var post = _cache.FirstOrDefault(p => p.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));
    //        bool isAdmin = IsAdmin();

    //        if (post != null && post.PubDate <= DateTime.UtcNow && (post.IsPublished || isAdmin))
    //        {
    //            return Task.FromResult(post);
    //        }

    //        return Task.FromResult<PostViewModel>(null);
    //    }

    //    public virtual Task<PostViewModel> GetPostById(string id)
    //    {
    //        var post = _cache.FirstOrDefault(p => p.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
    //        bool isAdmin = IsAdmin();

    //        if (post != null && post.PubDate <= DateTime.UtcNow && (post.IsPublished || isAdmin))
    //        {
    //            return Task.FromResult(post);
    //        }

    //        return Task.FromResult<PostViewModel>(null);
    //    }

    //    public virtual Task<IEnumerable<string>> GetCategories()
    //    {
    //        bool isAdmin = IsAdmin();

    //        var categories = _cache
    //            .Where(p => p.IsPublished || isAdmin)
    //            .SelectMany(post => post.Categories)
    //            .Select(cat => cat.ToLowerInvariant())
    //            .Distinct();

    //        return Task.FromResult(categories);
    //    }

    //    public async Task SavePost(PostViewModel post)
    //    {
    //        string filePath = GetFilePath(post);
    //        post.LastModified = DateTime.UtcNow;

    //        XDocument doc = new XDocument(
    //                        new XElement("post",
    //                            new XElement("title", post.Title),
    //                            new XElement("slug", post.Slug),
    //                            new XElement("pubDate", post.PubDate.ToString("yyyy-MM-dd HH:mm:ss")),
    //                            new XElement("lastModified", post.LastModified.ToString("yyyy-MM-dd HH:mm:ss")),
    //                            new XElement("excerpt", post.Excerpt),
    //                            new XElement("content", post.Content),
    //                            new XElement("ispublished", post.IsPublished),
    //                            new XElement("categories", string.Empty),
    //                            new XElement("comments", string.Empty)
    //                        ));

    //        XElement categories = doc.XPathSelectElement("post/categories");
    //        foreach (string category in post.Categories)
    //        {
    //            categories.Add(new XElement("category", category));
    //        }

    //        XElement comments = doc.XPathSelectElement("post/comments");
    //        foreach (CommentViewModel comment in post.Comments)
    //        {
    //            comments.Add(
    //                new XElement("comment",
    //                    new XElement("author", comment.Author),
    //                    new XElement("email", comment.Email),
    //                    new XElement("date", comment.PubDate.ToString("yyyy-MM-dd HH:m:ss")),
    //                    new XElement("content", comment.Content),
    //                    new XAttribute("isAdmin", comment.IsAdmin),
    //                    new XAttribute("id", comment.Id)
    //                ));
    //        }

    //        using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
    //        {
    //            await doc.SaveAsync(fs, SaveOptions.None, CancellationToken.None).ConfigureAwait(false);
    //        }

    //        if (!_cache.Contains(post))
    //        {
    //            _cache.Add(post);
    //            SortCache();
    //        }
    //    }

    //    public Task DeletePost(PostViewModel post)
    //    {
    //        string filePath = GetFilePath(post);

    //        if (File.Exists(filePath))
    //        {
    //            File.Delete(filePath);
    //        }

    //        if (_cache.Contains(post))
    //        {
    //            _cache.Remove(post);
    //        }

    //        return Task.CompletedTask;
    //    }

    //    public async Task<string> SaveFileAsync(byte[] bytes, string fileName, string suffix = null)
    //    {
    //        suffix = suffix ?? DateTime.UtcNow.Ticks.ToString();

    //        string ext = Path.GetExtension(fileName);
    //        string name = Path.GetFileNameWithoutExtension(fileName);

    //        string relative = $"files/{name}_{suffix}{ext}";
    //        string absolute = Path.Combine(_folder, relative);
    //        string dir = Path.GetDirectoryName(absolute);

    //        Directory.CreateDirectory(dir);
    //        using (var writer = new FileStream(absolute, FileMode.CreateNew))
    //        {
    //            await writer.WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(false);
    //        }

    //        return "/Posts/" + relative;
    //    }

    //    private string GetFilePath(PostViewModel post)
    //    {
    //        return Path.Combine(_folder, post.Id + ".xml");
    //    }

    //    private void Initialize()
    //    {
    //        LoadPosts();
    //        SortCache();
    //    }

    //    private void LoadPosts()
    //    {
    //        if (!Directory.Exists(_folder))
    //            Directory.CreateDirectory(_folder);

    //        // Can this be done in parallel to speed it up?
    //        foreach (string file in Directory.EnumerateFiles(_folder, "*.xml", SearchOption.TopDirectoryOnly))
    //        {
    //            XElement doc = XElement.Load(file);

    //            PostViewModel post = new PostViewModel
    //            {
    //                Id = Path.GetFileNameWithoutExtension(file),
    //                Title = ReadValue(doc, "title"),
    //                Excerpt = ReadValue(doc, "excerpt"),
    //                Content = ReadValue(doc, "content"),
    //                Slug = ReadValue(doc, "slug").ToLowerInvariant(),
    //                PubDate = DateTime.Parse(ReadValue(doc, "pubDate")),
    //                LastModified = DateTime.Parse(ReadValue(doc, "lastModified", DateTime.Now.ToString(CultureInfo.InvariantCulture))),
    //                IsPublished = bool.Parse(ReadValue(doc, "ispublished", "true")),
    //            };

    //            LoadCategories(post, doc);
    //            LoadComments(post, doc);
    //            _cache.Add(post);
    //        }
    //    }

    //    private static void LoadCategories(PostViewModel post, XElement doc)
    //    {
    //        XElement categories = doc.Element("categories");
    //        if (categories == null)
    //            return;

    //        List<string> list = new List<string>();

    //        foreach (var node in categories.Elements("category"))
    //        {
    //            list.Add(node.Value);
    //        }

    //        post.Categories = list.ToList();
    //    }

    //    private static void LoadComments(PostViewModel post, XElement doc)
    //    {
    //        var comments = doc.Element("comments");

    //        if (comments == null)
    //            return;

    //        foreach (var node in comments.Elements("comment"))
    //        {
    //            CommentViewModel comment = new CommentViewModel()
    //            {
    //                Id = ReadAttribute(node, "id"),
    //                Author = ReadValue(node, "author"),
    //                Email = ReadValue(node, "email"),
    //                IsAdmin = bool.Parse(ReadAttribute(node, "isAdmin", "false")),
    //                Content = ReadValue(node, "content"),
    //                PubDate = DateTime.Parse(ReadValue(node, "date", "2000-01-01")),
    //            };

    //            post.Comments.Add(comment);
    //        }
    //    }

    //    private static string ReadValue(XElement doc, XName name, string defaultValue = "")
    //    {
    //        if (doc.Element(name) != null)
    //            return doc.Element(name)?.Value;

    //        return defaultValue;
    //    }

    //    private static string ReadAttribute(XElement element, XName name, string defaultValue = "")
    //    {
    //        if (element.Attribute(name) != null)
    //            return element.Attribute(name)?.Value;

    //        return defaultValue;
    //    }
    //    protected void SortCache()
    //    {
    //        _cache.Sort((p1, p2) => p2.PubDate.CompareTo(p1.PubDate));
    //    }

    //    protected bool IsAdmin()
    //    {
    //        return _contextAccessor.HttpContext?.User?.Identity.IsAuthenticated == true;
    //    }

    //}
}
