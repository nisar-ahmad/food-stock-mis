using System.Web.Mvc;

namespace FMS.Models
{
    public static class ExtensionsMethods
    {
        public static int ErrorCount(this ModelStateDictionary modelStateDictionary)
        {
            int count = 0;

            foreach (var modelState in modelStateDictionary.Values)
                count += modelState.Errors.Count;

            return count;
        }
    }
}