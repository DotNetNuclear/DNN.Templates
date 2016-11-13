using System;
using System.Linq;
using System.Web.Mvc;
using $ext_rootnamespace$$safeprojectname$.Components;
using $ext_rootnamespace$$safeprojectname$.Models;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Entities.Users;
using DotNetNuke.Framework.JavaScriptLibraries;

namespace $ext_rootnamespace$$safeprojectname$.Controllers
{
    [DnnHandleError]
    public class ItemController : DnnController
    {
        private int _moduleId = 0;

        public ItemController(IItemManager mockManager, int moduleId)
        {
            ItemManager.SetTestableInstance(mockManager);
            _moduleId = moduleId;
        }

        public ActionResult Delete(int itemId)
        {
            if (ModuleContext != null)
            {
                _moduleId = ModuleContext.ModuleId;
            }
            ItemManager.Instance.DeleteItem(itemId, ModuleContext.ModuleId);
            return RedirectToDefaultRoute();
        }
         
        public ActionResult Edit(int itemId = -1)
        {
            try { DotNetNuke.Framework.JavaScriptLibraries.JavaScript.RequestRegistration(CommonJs.DnnPlugins); }
            catch { }

            if (PortalSettings != null)
            {
                var userlist = UserController.GetUsers(PortalSettings.PortalId);
                var users = from user in userlist.Cast<UserInfo>().ToList()
                            select new SelectListItem { Text = user.DisplayName, Value = user.UserID.ToString() };

                ViewBag.Users = users;
            }

            if (ModuleContext != null)
            {
                _moduleId = ModuleContext.ModuleId;
            }

            var item = (itemId == -1)
                 ? new Item { ModuleId = _moduleId }
                 : ItemManager.Instance.GetItem(itemId, _moduleId);

            return View(item);
        }

        [HttpPost]
        [DotNetNuke.Web.Mvc.Framework.ActionFilters.ValidateAntiForgeryToken]
        public ActionResult Edit(Item item)
        {
            if (item.ItemId == -1)
            {
                item.CreatedByUserId = User.UserID;
                item.CreatedOnDate = DateTime.UtcNow;
                item.LastModifiedByUserId = User.UserID;
                item.LastModifiedOnDate = DateTime.UtcNow;

                ItemManager.Instance.CreateItem(item);
            }
            else
            {
                var existingItem = ItemManager.Instance.GetItem(item.ItemId, item.ModuleId);
                existingItem.LastModifiedByUserId = User.UserID;
                existingItem.LastModifiedOnDate = DateTime.UtcNow;
                existingItem.ItemName = item.ItemName;
                existingItem.ItemDescription = item.ItemDescription;
                existingItem.AssignedUserId = item.AssignedUserId;

                ItemManager.Instance.UpdateItem(existingItem);
            }

            return RedirectToDefaultRoute();
        }

        [ModuleAction(ControlKey = "Edit", TitleKey = "AddItem")]
        public ActionResult Index()
        {
            if (ModuleContext != null)
            {
                _moduleId = ModuleContext.ModuleId;
            }
            var items = ItemManager.Instance.GetItems(_moduleId);
            return View(items);
        }
    }
}
