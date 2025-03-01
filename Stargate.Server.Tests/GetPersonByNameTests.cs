using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Stargate.Server.Business.Queries;
using Stargate.Server.Data.Models;
using Stargate.Server.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Stargate.Server.Tests;

public class GetPersonByNameTests
{
    [Fact]
    public async Task GetPersonByName_ShouldCall_GetPersonAstronautByNameAsync_Once()
    {
        // Arrange
        var services = new ServiceCollection();

        var mockedPersonRepo = new Mock<IPersonRepository>();        

        var serviceProvider = services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetPersonByNameHandler).Assembly))
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var query = new GetPersonByName() { Name = "Dan Carson" };

        // Act
        var response = await mediator.Send(query);

        // Assert
        mockedPersonRepo.Verify(m => m.GetPersonAstronautByNameAsync(It.IsAny<string>(), default), Times.Once);
    }

    [Fact]
    public async Task GetPersonByName_ShouldReturnNotFound_WhenPersonDoesNotExist()
    {
        // Arrange
        var services = new ServiceCollection();

        var mockedPersonRepo = new Mock<IPersonRepository>();
        mockedPersonRepo.Setup(x => x.GetPersonAstronautByNameAsync(It.IsAny<string>(), default)).Returns(Task.FromResult((PersonAstronaut?)null));

        var serviceProvider = services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetPersonByNameHandler).Assembly))
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var query = new GetPersonByName() { Name = "Dan Carson" };

        // Act
        var response = await mediator.Send(query);

        // Assert
        response.ResponseCode.Should().Be((int)HttpStatusCode.NotFound);
        response.Success.Should().BeFalse();
        response.Person.Should().BeNull();
    }

    [Fact]
    public async Task GetPersonByName_ShouldReturnOk_WhenPersonExists()
    {
        // Arrange
        var services = new ServiceCollection();

        const string expName = "Dan Carson";

        var person = new PersonAstronaut
        {
            PersonId = 1,
            Name = expName
        };

        var mockedPersonRepo = new Mock<IPersonRepository>();
        mockedPersonRepo.Setup(x => x.GetPersonAstronautByNameAsync(expName, default)).Returns(Task.FromResult((PersonAstronaut?)person));

        var serviceProvider = services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetPersonByNameHandler).Assembly))
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var query = new GetPersonByName() { Name = expName };

        // Act
        var response = await mediator.Send(query);

        // Assert
        response.ResponseCode.Should().Be((int)HttpStatusCode.OK);
        response.Success.Should().BeTrue();
        response.Person.Should().NotBeNull();
    }

    [Fact]
    public async Task GetPersonByName_ShouldBeValidGetPersonByNameResult_WhenPersonExists()
    {
        // Arrange
        var services = new ServiceCollection();
        
        const int expPersonId = 1;
        const string expName = "Dan Carson";
        const string expRank = "2LT";
        const string expTitle = "Commander";
        DateTime expCareerStartDate = new DateTime(2025, 3, 1);
        DateTime? expCareerEndDate = null;

        var person = new PersonAstronaut
        {
            PersonId = expPersonId,
            Name = expName,
            CurrentRank = expRank,
            CurrentDutyTitle = expTitle,
            CareerStartDate = expCareerStartDate,
            CareerEndDate = expCareerEndDate
        };

        var mockedPersonRepo = new Mock<IPersonRepository>();
        mockedPersonRepo.Setup(x => x.GetPersonAstronautByNameAsync(expName, default)).Returns(Task.FromResult((PersonAstronaut?)person));

        var serviceProvider = services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetPersonByNameHandler).Assembly))
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var query = new GetPersonByName() { Name = expName };

        // Act
        var response = await mediator.Send(query);

        // Assert
        response.Person.Should().NotBeNull();
        response.Person.PersonId.Should().Be(expPersonId);
        response.Person.Name.Should().Be(expName);
        response.Person.CurrentRank.Should().Be(expRank);
        response.Person.CurrentDutyTitle.Should().Be(expTitle);
        response.Person.CareerStartDate.Should().Be(expCareerStartDate);
        response.Person.CareerEndDate.Should().Be(expCareerEndDate);
    }
}
