using Blog.BLL.Controllers;
using Blog.BLL.ViewModels;
using Blog.Data.Models;
using Blog_withPostgresql.Controllers;
using Blog_withPostgresql.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace BlogTests;

[TestClass]
public class Tests
{
    [TestMethod]
    public async Task Test1()
    {
        User user = new User() 
        {
               Id = 666,
               Name = "John",
               Age = 13,
               Email = "test@mail.ru",
               Password = "10101",
               Role = "User"
        };

        UserRegViewModel userRegViewModel = new UserRegViewModel() 
        {
            Name = "John",
            Email = "test@mail.ru",
            Age = 13,
            Password = "10101",
            ConfirmPassword = "10100"
        };

        //// Arrange
        //var mock = new Mock<IUserRepo>();
        //mock.Setup(s => s.AddUser(user));
        //var mockVM = new Mock<UserRegViewModel>();
        var mockLogg = new Mock<ILogger<AuthRegController>>();
        //AuthRegController controller = new AuthRegController(mock.Object, mockLogg.Object);

        //// Act
        //ViewResult result = await controller.RegUser(userRegViewModel) as ViewResult;
        //RedirectToActionResult resultRA = await controller.RegUser(userRegViewModel) as RedirectToActionResult; 
        //// Assert
        //Assert.AreEqual();
        // Arrange
        var userRepoMock = new Mock<IUserRepo>();

        var controller = new AuthRegController(userRepoMock.Object, mockLogg.Object);
        controller.ModelState.AddModelError("Name", "Name is required");

        // Act
        var result = await controller.RegUser(new UserRegViewModel()) as ViewResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("UserRegViewModel", result.ViewName);
    }
}