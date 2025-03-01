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

public class GetPeopleTests
{
    [Fact]
    public async Task GetPeople_ShouldCall_GetPersonAstronautAllAsync_Once()
    {
        // Arrange
        var services = new ServiceCollection();

        var mockedPersonRepo = new Mock<IPersonRepository>();

        var serviceProvider = services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetPersonByNameHandler).Assembly))
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var query = new GetPeople() { };

        // Act
        var response = await mediator.Send(query);

        // Assert
        mockedPersonRepo.Verify(m => m.GetPersonAstronautAllAsync(default), Times.Once);
    }

    [Fact]
    public async Task GetPeople_ShouldReturnOKStatusCode_WhenRequestIsValid()
    {
        // Arrange
        var services = new ServiceCollection();

        IEnumerable<PersonAstronaut> people = new List<PersonAstronaut>() { };

        var mockedPersonRepo = new Mock<IPersonRepository>();
        mockedPersonRepo.Setup(x => x.GetPersonAstronautAllAsync(default)).Returns(Task.FromResult(people));

        var serviceProvider = services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetPersonByNameHandler).Assembly))
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var query = new GetPeople() { };

        // Act
        var response = await mediator.Send(query);

        // Assert
        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.ResponseCode.Should().Be((int)HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetPeople_ShouldBeValidGetPeopleResult_WhenRequestIsValid()
    {
        // Arrange
        var services = new ServiceCollection();

        var person1 = new PersonAstronaut
        {
            PersonId = 1,
            Name = "Dan Carson"
        };

        var person2 = new PersonAstronaut
        {
            PersonId = 2,
            Name = "John Smith",
            CurrentRank = "2LT",
            CurrentDutyTitle = "Commander",
            CareerStartDate = new DateTime(2025, 3, 1)
        };

        IEnumerable<PersonAstronaut> people = new List<PersonAstronaut>() { person1, person2 };

        var mockedPersonRepo = new Mock<IPersonRepository>();
        mockedPersonRepo.Setup(x => x.GetPersonAstronautAllAsync(default)).Returns(Task.FromResult(people));

        var serviceProvider = services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetPersonByNameHandler).Assembly))
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var query = new GetPeople() { };

        // Act
        var response = await mediator.Send(query);

        // Assert
        response.Should().NotBeNull();
        response.ResponseCode.Should().Be((int)HttpStatusCode.OK);
        response.Success.Should().BeTrue();
        response.People.Count.Should().Be(people.Count());
        CheckEqual(response.People[0], person1).Should().BeTrue();
        CheckEqual(response.People[1], person2).Should().BeTrue();
    }

    private bool CheckEqual(PersonAstronaut a, PersonAstronaut b)
    {
        return a.PersonId == b.PersonId
            && a.Name.Equals(b.Name)
            && (a.CurrentRank ?? "").Equals(b.CurrentRank ?? "")
            && (a.CurrentDutyTitle ?? "").Equals(b.CurrentDutyTitle ?? "")
            && a.CareerStartDate.Equals(b.CareerStartDate)
            && a.CareerEndDate.Equals(b.CareerEndDate);
    }
}
