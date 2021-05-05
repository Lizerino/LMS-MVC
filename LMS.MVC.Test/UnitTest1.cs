using System.Security.Claims;
using System.Security.Principal;

using AutoFixture;
using AutoFixture.AutoMoq;

using AutoMapper;

using Lms.MVC.Core.Repositories;
using Lms.MVC.Data.Data;
using Lms.MVC.UI;
using Lms.MVC.UI.Controllers;
using Lms.MVC.UI.Models.ViewModels.ApplicationUserViewModels;
using Lms.MVC.UI.Utilities.Pagination;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Moq;

using Xunit;

namespace LMS.MVC.Test
{
    public class HomeControllerTest
    {
        [Fact]
        public void IndexRedirectsAdminsToApplicationUsersIndex()
        {
            //Arrange 
            var fixture = new Fixture();
            var mc = fixture.Create<ModulesController>();
            fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });





            //Act
            var viewResult = mc.Create(fixture.Create<int>());

            //Assert
            Assert.NotNull(viewResult);
            //Assert.NotNull(viewResult.Model);
            //Assert.Equal(model, viewResult.Model);
        }
    }
}