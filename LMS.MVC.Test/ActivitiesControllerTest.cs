using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using AutoMapper;

using Lms.MVC.Core.Entities;
using Lms.MVC.Core.Repositories;
using Lms.MVC.UI.Controllers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Moq;

using Xunit;

namespace Lms.MVC.Test
{
    public class ActivitiesControllerTest
    {
        private readonly Mock<IUoW> MockUow;
        private readonly IMapper Mapper;
        private readonly IUoW UoW;

        private readonly ActivitiesController controller;

        public ActivitiesControllerTest()
        {   
            controller = new ActivitiesController(Mapper, UoW);
        }        

        [Fact]
        public void ActivitiesControllerReturnsAViewIfIdIsNullAndUserIsAStudent()
        {
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

            var viewResult = controller.Index(null).Result;
            Assert.NotNull(viewResult);
            Assert.IsType<RedirectToActionResult>(viewResult);
        }

        [Fact]
        public void ActivitiesControllerReturnsNullIfIdIsNullAndUserIsNotAStudent()
        {
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

            var viewResult = controller.Index(null).Result;
            Assert.NotNull(viewResult);
            Assert.IsType<RedirectToActionResult>(viewResult);
        }
    }
}