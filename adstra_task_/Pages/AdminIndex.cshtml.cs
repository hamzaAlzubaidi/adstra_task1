using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using adstra_task.Data;
using adstra_task.Core;

namespace adstra_task_.Pages
{
    [Authorize]
    public class AdminIndexModel : PageModel
    {
        private readonly IDataHelper<AuthorPost> dataHelper;

        public AdminIndexModel(IDataHelper<AuthorPost> dataHelper)
        {
            this.dataHelper = dataHelper;
        }

        public int AllPost { get; set; }
        public int PostLastMouth { get; set; }
        public int PostThisYear { get; set; }
        public void OnGet()
        {
            var datem = DateTime.Now.AddMonths(-1);
            var datey = DateTime.Now.AddYears(-1);
            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            AllPost = dataHelper.GetDataByUser(userid).Count;
            PostLastMouth = dataHelper.GetDataByUser(userid).Where(x => x.AddedDate >= datem).Count();
            PostThisYear = dataHelper.GetDataByUser(userid).Where(x => x.AddedDate >= datey).Count();
        }
    }
}
