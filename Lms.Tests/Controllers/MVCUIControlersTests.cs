using Lms.MVC.UI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lms.API.Core.Entities;
using Lms.MVC.UI.Models.ViewModels.ActivityViewModels;
using Microsoft.AspNetCore.Identity;
using Lms.MVC.Core.Entities;

namespace Lms.Tests.Controllers
{
    [TestClass]
    public class MVCUIControlersTests
    {
        //private readonly object course;
        //private CoursesController Controller;
        private readonly object DetailActivityViewModel;
        private ActivitiesController Controller;

        [TestInitialize]
        public void SetUp()
        {
            UserManager<ApplicationUser> x = new
            //var mockContext
   //         AcourseC = new CoursesController();
        }

        [TestMethod]
        public void TestDetailsView(int Id)
        {
  //          Controller = new ActivitiesController();

  //          var result = Controller.Details(Id) as DetailActivityViewModel;
  //          Assert.AreEqual(DetailActivityViewModel, result.Id);



        }
    }
}
