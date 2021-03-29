using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SporeServer.Pages.Community.AssetBrowser
{
    [Authorize]
    public class CommentApprovalModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
