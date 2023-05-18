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

namespace adstra_task_.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IAuthorizationService authorizationService;
        private readonly IWebHostEnvironment webHost;
        private readonly IDataHelper<Authors> dataHelper;
        private readonly Code.FilesHelper filesHelper;
        private int pageItem;
        public AuthorController(
            IDataHelper<Authors> dataHelper,
            IAuthorizationService authorizationService
            , IWebHostEnvironment webHost)
        {
            this.dataHelper = dataHelper;
            this.authorizationService = authorizationService;
            this.webHost = webHost;
            filesHelper = new Code.FilesHelper(this.webHost);
            pageItem = 10;
        }
        // GET: AuthorController
        [Authorize("Admin")]
        public ActionResult Index(int? id)
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
        [Authorize("Admin")]

        public ActionResult Search(string SearchItem)
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



        // GET: AuthorController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var author = dataHelper.Find(id);
            CoreView.AuthorView authorView = new CoreView.AuthorView
            {
                Id = author.Id,
                FullName= author.FullName,
                Bio = author.Bio,
                Instgram = author.Instgram,
                Twitter = author.Twitter,
                UserId = author.UserId,
                UserName = author.UserName,

            };
            return View(authorView);
        }

        // POST: AuthorController/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CoreView.AuthorView collection)
        {
            try
            {
                var author = new Authors
                {
                    Id = collection.Id,
                    Bio = collection.Bio,
                    FullName= collection.FullName,
                    Instgram=collection.Instgram,
                    Twitter=collection.Twitter,
                    UserId=collection.UserId,
                    ProfileImageUrl =filesHelper.UploadFile(collection.ProfileImageUrl,"Image")

                };
                dataHelper.Edit(id,author);
                var result = authorizationService.AuthorizeAsync(User, "Admin");
                    if(result.Result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                    else
                {
                    return Redirect("/AdminIndex");
                }
               
            }
            catch
            {
                return View();
            }
        }

        // GET: AuthorController/Delete/5
        [Authorize("Admin")]
        public ActionResult Delete(int id)
        {
            var author = dataHelper.Find(id);
            CoreView.AuthorView authorView = new CoreView.AuthorView
            {
                Id = author.Id,
                FullName = author.FullName,
                Bio = author.Bio,
                Instgram = author.Instgram,
                Twitter = author.Twitter,
                UserId = author.UserId,
                UserName = author.UserName,

            };
            return View(authorView);
        }

        // POST: AuthorController/Delete/5
        [Authorize("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Authors collection)
        {
            try
            {
                dataHelper.Delete(id);
                string fileBath = "~/Image/" + collection.ProfileImageUrl;
                    if(System.IO.File.Exists(fileBath))
                {
                    System.IO.File.Delete(fileBath);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
