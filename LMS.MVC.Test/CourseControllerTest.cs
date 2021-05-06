using System.Security.Claims;

using AutoFixture;

using Lms.MVC.UI.Controllers;
using Lms.MVC.UI.Models.ViewModels.ApplicationUserViewModels;
using Lms.MVC.UI.Models.ViewModels.CourseViewModels;
using Lms.MVC.UI.Utilities.Pagination;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

using Xunit;

namespace LMS.MVC.Test
{
    public class CouresControllerTest
    {
        [Fact]
        public void CoursesIndexShowsListCourses()
        {
            //Arrange
            var controller = new CoursesController();

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

            //Act
            var viewResult = (ViewResult)controller.Index("","",0).Result;

            //Assert
            Assert.NotNull(viewResult);
            Assert.IsType<PaginationResult<ListCourseViewModel>>(viewResult);            
        }
    }
}