using System.Collections.Specialized;
using System.Web.Mvc;
using System.Web.Routing;
using FMS.Models;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;

namespace FMS.HtmlHelpers
{
    /// <summary>
    /// Helper methods associated with the <see cref="https://github.com/troygoode/PagedList">PagedList</see> paging helper.
    /// </summary>
    public static class HtmlHelpers
    {
        /// <summary>
        /// Generates a string to a fully qualified URL to an action method including all of the querystring values 
        /// for the current page, together with a new (or replaced) one for the requested page number.
        /// </summary>
        /// <param name="urlHelper">Url helper to be used to created the action.</param>
        /// <param name="actionName">Target action name (Asp.NET MVC).</param>
        /// <param name="pageNumber">Target page number.</param>
        /// <param name="queryStrings">Existing query string collection.</param>
        /// <param name="pageQueryStringName">Optional name of the page number query string. (Defaults to "page").</param>
        /// <returns>Requested action string.</returns>
        public static string Action(this UrlHelper urlHelper, string actionName, int pageNumber, NameValueCollection queryStrings, string pageQueryStringName = "page")
        {
            // Add existing query string values to the dictionary, excluding any existing page value.
            RouteValueDictionary queryStringDictionary = new RouteValueDictionary();
            foreach (string key in queryStrings.Keys)
            {
                if (key != pageQueryStringName)
                {
                    queryStringDictionary.Add(key, queryStrings[key]);
                }
            }

            // Add the page value.
            queryStringDictionary.Add(pageQueryStringName, pageNumber);

            // Generate the action using the given UrlHelper.
            return urlHelper.Action(actionName, queryStringDictionary);
        }

        public static bool HasLevel(this IPrincipal user, UserLevel userLevel)
        {
            var hasAccess = false;
            var appUser = user.ApplicationUser();

            if (appUser != null)
                hasAccess = appUser.UserLevel == UserLevel.SystemAdmin || appUser.UserLevel == userLevel;

            return hasAccess;
        }

        public static ApplicationUser ApplicationUser(this IPrincipal user)
        {
            UserManager<ApplicationUser> userManager = new UserManager<Models.ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            if (user.Identity.IsAuthenticated)
                return userManager.FindByName(user.Identity.Name);
            else
                return null;
        }
    }

}
