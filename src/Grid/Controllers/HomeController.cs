using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using Grid.Features.Common;
using Grid.Features.Settings.DAL.Interfaces;
using Grid.Features.Social.DAL.Interfaces;
using Grid.Features.Social.Entities;
using Grid.Features.Social.ViewModels;
using Grid.Infrastructure;

namespace Grid.Controllers
{
    [Authorize]
    public class HomeController : GridBaseController
    {
        private readonly IGridUpdateRepository _gridUpdateRepository;
        private readonly IPostRepository _postRepository;
        private readonly IPostCommentRepository _postCommentRepository;
        private readonly IPostLikeRepository _postLikeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IGridUpdateRepository gridUpdateRepository,
                              IPostRepository postRepository,
                              IPostLikeRepository postLikeRepository,
                              IPostCommentRepository postCommentRepository,
                              IUnitOfWork unitOfWork)
        {
            _gridUpdateRepository = gridUpdateRepository;
            _postRepository = postRepository;
            _postLikeRepository = postLikeRepository;
            _postCommentRepository = postCommentRepository;
            _unitOfWork = unitOfWork;
        }

        #region Ajax Calls
        [HttpGet]
        public JsonResult GetAllPosts()
        {
            var posts = _postRepository.GetAll(o => o.OrderByDescending(c => c.CreatedOn), "CreatedByUser.Person").ToList();
            var postsFormatted = posts.Select(p => new PostViewModel(p)).ToList();
            return Json(postsFormatted, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddPost(NewPostViewModel vm)
        {
            // Add it as an Activity
            var newActivity = new Post
            {
                Title = vm.Title,
                Content = vm.Content,
                CreatedByUserId = WebUser.Id
            };

            _postRepository.Create(newActivity);
            _unitOfWork.Commit();

            return Json(true);
        }
        #endregion

        public ActionResult Index()
        {
            // Needs cleanup, we shouldn't get in if WebUser is empty.
            var permissions = new List<int>();

            if (WebUser != null)
            {
                permissions = WebUser.Permissions.ToList();
            }

            var updates = _gridUpdateRepository.GetAllBy(u => permissions.Contains(u.PermissionCode), q => q.OrderByDescending(s => s.Date)).ToList();
            return View(updates);
        }

        public ActionResult Stream()
        {
            var vm = new SocialStreamViewModel();
            return View(vm);
        }
    }
}