using System;
using System.Web.Mvc;
using $ext_rootnamespace$$ext_safeprojectname$.Controllers;
using $ext_rootnamespace$$ext_safeprojectname$.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using $ext_rootnamespace$$ext_safeprojectname$.Testing.Mocks;

namespace $ext_rootnamespace$$ext_safeprojectname$.Testing.UnitTests
{
    [TestClass]
    public class ItemControllerTests
    {
        [TestMethod]
        public void Edit_CreateNewItem_ModuleIdAssignedinModel()
        {
            // 1 - Arrange
            int moduleId = 2;
            var mockData = MockStores.MockItemManager();
            var modTwoItemCntrl = new ItemController(mockData.Object, moduleId); // Create controller for module Id 2

            // 2 - Act
            var actionResult = (ViewResult)modTwoItemCntrl.Edit(); // Call edit view with no item Id (Add New)

            // 3 - Assert
            var itemModel = (Item)actionResult.Model;
            Assert.IsTrue(itemModel != null && itemModel.ModuleId == moduleId);
        }
    }
}
