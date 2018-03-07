using System.Linq;
using System.Web.Mvc;
using Grid.DAL.Interfaces;
using Grid.DAL.Settings.Interfaces;
using Grid.DAL.Social.Interfaces;
using Grid.Entities.Social;
using Grid.Infrastructure;
using Grid.ViewModels.Social;

namespace Grid.UI.WebApp.Controllers
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
            var userPermissions = WebUser.Permissions;
            var updates = _gridUpdateRepository.GetAllBy(u => userPermissions.Contains(u.PermissionCode), q => q.OrderByDescending(s => s.Date)).ToList();
            return View(updates);
        }

        public ActionResult Stream()
        {
            var vm = new SocialStreamViewModel();
            return View(vm);
        }
    }
}