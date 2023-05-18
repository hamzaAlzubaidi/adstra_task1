using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using adstra_task.Data;
using adstra_task.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace adstra_task_.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IDataHelper<Authors> dataHelperForAuthor;
        private readonly IDataHelper<Category> dataHelperForCategory;
        private readonly IAuthorizationService authorizationService;
        private readonly IWebHostEnvironment webHost;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IDataHelper<AuthorPost> dataHelper;
        private readonly Code.FilesHelper filesHelper;

        private int pageItem;
        private Task<AuthorizationResult> result;
        private string UserId;

        public PostController(
            IDataHelper<AuthorPost> dataHelper,
             IDataHelper<Authors> dataHelperForAuthor,
            IDataHelper<Category> dataHelperForCategory,
            IAuthorizationService authorizationService
            , IWebHostEnvironment webHost,
             UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager
            )
        {
            this.dataHelper = (IDataHelper<AuthorPost>)dataHelper;
            this.dataHelperForAuthor = dataHelperForAuthor;
            this.dataHelperForCategory = dataHelperForCategory;
            this.authorizationService = authorizationService;
            this.webHost = webHost;
            this.userManager = userManager;
            this.signInManager = signInManager;
            filesHelper = new Code.FilesHelper(this.webHost);
            pageItem = 10;
             result = authorizationService.AuthorizeAsync(User, "Admin");
            UserId = User.FindFirst(ClaimTypes.NameIdentifier).ToString();
        }
        // GET: PostController
        public ActionResult Index(int? id )
        {
            if(result.Result.Succeeded)

            {
                if (id == 0 || id == null)
                {
                    return View(dataHelper.GetAllData().Take(pageItem));
                }
                else
                {
                    var data = dataHelper.GetAllData().Where(x => x.Id > id).Take(pageItem);
                    return View(data);
                }
            }
            else
            {
                if (id == 0 || id == null)
                {
                    return View(dataHelper.GetDataByUser(UserId).Take(pageItem));
                }
                else
                {
                    var data = dataHelper.GetDataByUser(UserId).Where(x => x.Id > id).Take(pageItem);
                    return View(data);
                }

            }

           
        }

        // GET: PostController/Details/5
        public ActionResult Details(int id)
        {
            return View(dataHelper.Find(id));
        }

        // GET: PostController/Create
        public ActionResult Create()
        {
             
            return View();
        }

        // POST: PostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CoreView.AuthorPostView collection)
        {
            try
            {
                SetUser();
                var post = new AuthorPost
                {
                    AddedDate = collection.AddedDate,
                    Author=collection.Authors,
                    AuthorId =dataHelperForAuthor.GetAllData().Where(x=>x.UserId==UserId).Select(x=>x.Id).First() ,
                    category = collection.category,
                    CategoryId = dataHelperForCategory.GetAllData().Where(x => x.Name == collection.postCategory).Select(x => x.Id).First(),
                    FullName = collection.FullName,
                    postCategory = collection.postCategory,
                    PostDescraption = collection.PostDescraption,
                    PostTitle = collection.PostTitle,
                    UserId = collection.UserId,
                    UserName = collection.UserName,
                    PostImageUrl = filesHelper.UploadFile(collection.PostImageUrl,"Image")

                };
                dataHelper.Add(post);

              
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PostController/Edit/5
        public ActionResult Edit(int id)
        {
            var authorpost = dataHelper.Find(id);
            CoreView.AuthorPostView authorPostView = new CoreView.AuthorPostView
            {
                AddedDate = authorpost.AddedDate,
                Authors = authorpost.Authors,
                AuthorId = authorpost.AuthorId,
                category = authorpost.category,
                CategoryId = authorpost.CategoryId,
                FullName = authorpost.FullName,
                postCategory = authorpost.postCategory,
                PostDescraption = authorpost.PostDescraption,
                PostTitle = authorpost.PostTitle,
                UserId = authorpost.UserId,
                UserName = authorpost.UserName,
                Id = authorpost.Id
            };
            return View(authorPostView);
        }

        // POST: PostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CoreView.AuthorPostView collection)
        {
            try
            {
                SetUser();
                var Post = new AuthorPost
                {
                    AddedDate = DateTime.Now,
                    Authors = collection.Authors,
                    AuthorId = dataHelperForAuthor.GetAllData().Where(x => x.UserId == UserId).Select(x => x.Id).First(),
                    category = collection.category,
                    CategoryId = dataHelperForCategory.GetAllData().Where(x => x.Name == collection.postCategory).Select(x => x.Id).First(),
                    FullName = dataHelperForAuthor.GetAllData().Where(x => x.UserId == UserId).Select(x => x.FullName).First(),
                    postCategory = collection.postCategory,
                    PostDescraption = collection.PostDescraption,
                    PostTitle = collection.PostTitle,
                    UserId = UserId,
                    UserName = dataHelperForAuthor.GetAllData().Where(x => x.UserId == UserId).Select(x => x.UserName).First(),
                    PostImageUrl = filesHelper.UploadFile(collection.PostImageUrl, "Images"),
                    Id = collection.Id
                };
                dataHelper.Edit(id, Post);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PostController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(dataHelper.Find(id));
        }

        // POST: PostController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, AuthorPost collection)
        {
            try
            {
                dataHelper.Delete(id);
                string filePath = "~/Images/" + collection.PostImageUrl;
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Search(string SearchItem)
        {
            SetUser();
            if (result.Result.Succeeded)
            {
                if (SearchItem == null)
                {
                    return View("Index", dataHelper.GetAllData());
                }
                else
                {
                    return View("Index", dataHelper.Search(SearchItem));
                }
            }
            else
            {
                if (SearchItem == null)
                {
                    return View("Index", dataHelper.GetDataByUser(UserId));
                }
                else
                {
                    return View("Index", dataHelper.Search(SearchItem).Where(x => x.UserId == UserId).ToList());
                }
            }

        }
        private void SetUser()
        {
            result = authorizationService.AuthorizeAsync(User, "Admin");
            UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
