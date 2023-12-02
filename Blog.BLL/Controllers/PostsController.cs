using Blog.BLL.Models;
using Blog.BLL.Repositories;
using Blog.BLL.ViewModels;
using Blog_withPostgresql.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace Blog.BLL.Controllers
{
    public class PostsController : Controller
    {        

        private IWebHostEnvironment _env;
        private IMyLogger _logger;
        private IPostRepo _postRepo;
        IUserRepo _userRepo;
        private readonly ITegRepo _tegRepo;
        IPostsTegsRepo _postsTegsRepo;

        public PostsController(IWebHostEnvironment environment, IMyLogger logger, IPostRepo postRepo, IUserRepo userRepo, ITegRepo tegRepo, IPostsTegsRepo postsTegsRepo)
        {
            _env = environment;
            _logger = logger;
            _postRepo = postRepo;
            _userRepo = userRepo;
            _tegRepo = tegRepo;
            _postsTegsRepo = postsTegsRepo;
        }
        
        //[Authorize]
        [HttpGet("UserBlog")]
        public async Task<IActionResult> UserBlog()
        {
            var userEmail = HttpContext.User.Identity.Name;
            User userByEmail = _userRepo.GetUserByEmail(userEmail);
            List<Post> currentPosts = (await _postRepo.GetAllPosts()).Where(p => p.UserId == userByEmail.Id).ToList();
            UserBlogViewModel userBlogViewModel = new UserBlogViewModel()
            {
                UserPosts = currentPosts 
            };
            return View(userBlogViewModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> UserBlog(UserBlogViewModel viewModel)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = _userRepo.GetUserByEmail(currentUser.Identity.Name);

            try
            {

                if (ModelState.IsValid)
                {
                    // Проверка существующего поста
                    //var existingPost = await _context.Posts
                    //.Include(p => p.Tegs)
                    //.Include(p => p.User)
                    //.FirstOrDefaultAsync(p => p.Id == viewModel.PostId);

                    if (/*existingPost == null*/true)
                    {
                        //создание нового поста
                        var newPost = new Post
                        {
                            postTitle = viewModel.Title,
                            PublicationDate = DateTime.UtcNow,
                            postText = viewModel.Text,
                            UserId = user.Id
                        };

                        //запись тегов и выдёргивание Id тегов
                        List<int> tegIds = new List<int>();
                        foreach (var tegVM in viewModel.HasWritingTags())
                        {
                            if (_tegRepo.FindTegByTitle(tegVM.tegTitle).tegTitle == null)
                            {
                                int idTeg = await _tegRepo.AddTeg(tegVM);
                                tegIds.Add(idTeg);
                            }
                            else
                            {
                                tegIds.Add(_tegRepo.FindTegByTitle(tegVM.tegTitle).Id);
                            }
                        }

                        //запись нового поста
                        int postId = await _postRepo.AddPost(newPost);

                        //создание экземпляра PostsTegs
                        foreach (var item in tegIds)
                        {
                            PostsTegs postsTegs = new PostsTegs()
                            {
                                PostId = postId,
                                TegId = item
                            };
                            
                            await _postsTegsRepo.InsertIntoPostTegs(postsTegs);
                        }
                    }

                    return RedirectToAction("UserBlog", "Posts");
                }
                else
                {
                    //var addPosstsForView = await _context.Posts
                    //    .Where(u => u.UserId == userId).ToListAsync();
                    //viewModel.UserPosts = addPosstsForView;
                    return View();
                }
            }
            catch (Exception ex)
            {
               await _logger.WriteError($"{ex.Message}");
            }
            return View(viewModel);
        }
        /*
        [HttpGet]
        public async Task<IActionResult> PostDiscussion(int id)
        {
            List<Comment> comments = await _context.Comments
                .Include(u => u.User)
                .ToListAsync();
            List<Post> posts = await _context.Posts.Include(u => u.User).ToListAsync();
            var post = await _context.Posts.Include(t => t.Tegs).FirstOrDefaultAsync(i => i.Id == id);
            PostViewModel pVM = new PostViewModel()
            {
                Id = id,
                CommentsOfPost = post.Comments,
                Title = post.Title,
                AuthorOfPost = post.User.UserName,
                Text = post.Text,
                Tegs = post.Tegs
            };
            CommentViewModel cVM = new CommentViewModel();
            DiscussionPostViewModel dpVM = new DiscussionPostViewModel { PostVM = pVM, CommentVM = comments };
            if (dpVM == null)
            {
                return NotFound();
            }
            return View("PostDiscussion", dpVM);
        }

        [HttpPost]
        public async Task<IActionResult> SetComment(DiscussionPostViewModel cVM)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier); //представляет идентификатор пользователя.

            //var comment = _context.Comments.Where(d => d.PostId == discussionPVM.Id).Select(d=>d).FirstOrDefaultAsync();
            Comment comment = new Comment()
            {
                UserId = userId!,
                CommentText = cVM.CommentText,
                PostId = cVM.PostVM.Id,
                CommentPublicationTime = DateTime.UtcNow
            };

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();


            return RedirectToAction("AllPostsPage", "Blog");
        }

        [HttpGet]
        public async Task<IActionResult> EditPost(int postId)
        {
            var post = await _context.Posts.Include(p => p.Tegs).FirstOrDefaultAsync(p => p.Id == postId);
            if (post == null)
            {
                return BadRequest();
            }

            var ubVM = new UserBlogViewModel
            {
                PostId = postId,
                Title = post.Title,
                Text = post.Text,
                PublicationDate = post.PublicationDate,
                tegsList = post.Tegs,
                tegs = string.Join(", ", post.Tegs.Select(t => t.TegTitle))
            };
            return View("EditPost", ubVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(UserBlogViewModel viewModel)
        {
            var post = await _context.Posts.Include(p => p.Tegs).FirstOrDefaultAsync(p => p.Id == viewModel.PostId);
            if (post == null)
            {
                return BadRequest();
            }
            // Очистить существующие теги
            post.Tegs.Clear();
            // Обновить свойства объекта Post
            post.Title = viewModel.Title;
            post.Text = viewModel.Text;

            // Добавить или обновить выбранные теги
            var tags = viewModel.HasWritingTags(); // Предположим, что HasWritingTags возвращает список объектов Teg
            foreach (var tag in tags)
            {
                var existingTag = await _context.Tegs.FirstOrDefaultAsync(t => t.Id == tag.Id);
                if (existingTag == null)
                {
                    post.Tegs.Add(tag); // Добавить новый тег
                }
                else
                {
                    post.Tegs.Add(existingTag); // Использовать существующий тег
                }
            }

            _context.Posts.Update(post); // Обновить пост в контексте

            await _context.SaveChangesAsync(); // Сохранить изменения
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditPostByAdminModer(UserBlogViewModel viewModel)
        {
            var post = await _context.Posts.Include(p => p.Tegs).FirstOrDefaultAsync(p => p.Id == viewModel.PostId);
            if (post == null)
            {
                return BadRequest();
            }
            // Очистить существующие теги
            post.Tegs.Clear();
            // Обновить свойства объекта Post
            post.Title = viewModel.Title;
            post.Text = viewModel.Text;

            // Добавить или обновить выбранные теги
            var tags = viewModel.HasWritingTags(); // Предположим, что HasWritingTags возвращает список объектов Teg
            foreach (var tag in tags)
            {
                var existingTag = await _context.Tegs.FirstOrDefaultAsync(t => t.Id == tag.Id);
                if (existingTag == null)
                {
                    post.Tegs.Add(tag); // Добавить новый тег
                }
                else
                {
                    post.Tegs.Add(existingTag); // Использовать существующий тег
                }
            }
            _context.Posts.Update(post); // Обновить пост в контексте

            await _context.SaveChangesAsync(); // Сохранить изменения
            return View("EditPost", viewModel);
        }


        #region Удаление статьи администратором
        [HttpPost]
        public async Task<IActionResult> DeletePostByAdmin(int postId)
        {
            Post postfodDelete = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (postfodDelete == null) { return BadRequest(); }
            else
            {
                _context.Posts.Remove(postfodDelete);
                _context.SaveChanges();
            }

            return RedirectToAction("AllPostsPage", "Blog");
        }
        #endregion
        */

    }
}
