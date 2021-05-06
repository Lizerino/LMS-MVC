using System.Security.Claims;

using AutoFixture;

using Lms.MVC.UI.Controllers;
using Lms.MVC.UI.Models.ViewModels.ApplicationUserViewModels;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

using Xunit;

namespace LMS.MVC.Test
{
    public class HomeControllerTest
    {
        [Fact]
        public void IndexRedirectsAdminsToApplicationUsersIndex()
        {
            //Arrange
            var controller = new HomeController();

            controller.ControllerContext = new ControllerContext();

            var context = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                     new Claim(ClaimTypes.Name, "TestUser"),
                     new Claim(ClaimTypes.Role, "Admin")
                }))
            };

            controller.ControllerContext.HttpContext = context;

            var model = new ApplicationUsersListViewModel();

            //Act
            var viewResult = controller.Index();

            //Assert
            Assert.NotNull(viewResult);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(viewResult);
            Assert.Equal("ApplicationUsers", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void IndexRedirectsTeachersToCourseIndex()
        {
            //Arrange
            var controller = new HomeController();

            controller.ControllerContext = new ControllerContext();

            var context = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                     new Claim(ClaimTypes.Name, "TestUser"),
                     new Claim(ClaimTypes.Role, "Teacher")
                }))
            };

            controller.ControllerContext.HttpContext = context;

            var model = new ApplicationUsersListViewModel();


            //Act
            var viewResult = controller.Index();

            //Assert
            Assert.NotNull(viewResult);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(viewResult);
            Assert.Equal("Courses", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }


        [Fact]
        public void IndexRedirectsStudentsToModulesIndex()
        {
            //Arrange
            var controller = new HomeController();

            controller.ControllerContext = new ControllerContext();

            var context = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                     new Claim(ClaimTypes.Name, "TestUser"),
                     new Claim(ClaimTypes.Role, "Student")
                }))
            };

            controller.ControllerContext.HttpContext = context;

            var model = new ApplicationUsersListViewModel();


            //Act
            var viewResult = controller.Index();

            //Assert
            Assert.NotNull(viewResult);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(viewResult);
            Assert.Equal("Modules", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void IndexRedirectsOtherRolesToModulesIndex()
        {
            //Arrange
            var controller = new HomeController();

            controller.ControllerContext = new ControllerContext();

            var context = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                     new Claim(ClaimTypes.Name, "TestUser"),                     
                }))
            };

            controller.ControllerContext.HttpContext = context;

            var model = new ApplicationUsersListViewModel();


            //Act
            var viewResult = controller.Index();

            //Assert
            Assert.NotNull(viewResult);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(viewResult);
            Assert.Equal("Modules", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}