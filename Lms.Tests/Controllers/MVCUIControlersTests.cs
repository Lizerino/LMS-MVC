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
using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
//using NUnit.Framework;

namespace Lms.Tests.Controllers
{
    [TestClass]
    public class MVCUIControlersTests
    {
        //private readonly object course;
        //private CoursesController Controller;
        private readonly object DetailActivityViewModel;
        private ActivitiesController Controller;
        private Fixture fix;
        private readonly IMapper mapper;
        private Mock mock;

        //[SetUp]
        [TestInitialize]
        public void SetUp()
        {
            //UserManager<ApplicationUser> x = new
            fix = new Fixture();
            //mock = new Mock<>();
        }

        [TestMethod]
        public void TestDetailsView()
        {


            //Arrange
  //          var id = 1;
  //          Controller = fix.Create<ActivitiesController>();
  // //         var task = Controller.Index(Id);
  //          //var id = task.Id;
  //              
  //
  //          //var result = Controller.Index(Id) as DetailActivityViewModel;
  //          Assert.AreEqual(DetailActivityViewModel, Id);
  //          //Assert.Equal(DetailActivityViewModel., Id);
  //
  //
        }
        
    }
}
