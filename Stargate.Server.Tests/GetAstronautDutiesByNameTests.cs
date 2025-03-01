using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Stargate.Server.Business.Commands;
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

public class GetAstronautDutiesByNameTests
{
    [Fact]
    public async Task GetAstronautDutiesByName_ShouldCallGetPersonAstronautByNameAsync_Once()
    {
        // Arrange
        var services = new ServiceCollection();

        var name = "Dan Carson";

        var mockedPersonRepo = new Mock<IPersonRepository>();
        var mockedAstronautRepo = new Mock<IAstronautRepository>();        

        var serviceProvider = services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetAstronautDutiesByNameHandler).Assembly))
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .AddScoped<IAstronautRepository>(x => mockedAstronautRepo.Object)
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var request = new GetAstronautDutiesByName()
        {
            Name = name
        };

        // Act
        var response = await mediator.Send(request);

        // Assert
        mockedPersonRepo.Verify(m => m.GetPersonAstronautByNameAsync(name, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAstronautDutiesByName_ShouldReturnNotFoundResponse_WhenPersonNotFound()
    {
        // Arrange
        var services = new ServiceCollection();

        var name = "Dan Carson";

        var mockedPersonRepo = new Mock<IPersonRepository>();
        var mockedAstronautRepo = new Mock<IAstronautRepository>();
        mockedPersonRepo.Setup(x => x.GetPersonAstronautByNameAsync(name, It.IsAny<CancellationToken>())).Returns(Task.FromResult((PersonAstronaut?)null));

        var serviceProvider = services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetAstronautDutiesByNameHandler).Assembly))
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .AddScoped<IAstronautRepository>(x => mockedAstronautRepo.Object)
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var request = new GetAstronautDutiesByName()
        {
            Name = name
        };

        // Act
        var response = await mediator.Send(request);

        // Assert
        response.Should().NotBeNull();
        response.Success.Should().BeFalse();
        response.ResponseCode.Should().Be((int)HttpStatusCode.NotFound);
        response.Message.Should().Be("Person not found in the system, unable to retrieve astronaut duties");
    }

    [Fact]
    public async Task GetAstronautDutiesByName_WhenPersonExists_ShouldCallGetDutiesByPersonIdAsync_Once()
    {
        // Arrange
        var services = new ServiceCollection();

        var name = "Dan Carson";

        var personAstronaut = new PersonAstronaut
        {
            PersonId = 1,
            Name = name,
            CurrentRank = "2LT",
            CurrentDutyTitle = "Commander",
            CareerStartDate = new DateTime(2025,3,1)
        };

        var mockedPersonRepo = new Mock<IPersonRepository>();
        var mockedAstronautRepo = new Mock<IAstronautRepository>();
        mockedPersonRepo.Setup(x => x.GetPersonAstronautByNameAsync(name, It.IsAny<CancellationToken>())).Returns(Task.FromResult((PersonAstronaut?)personAstronaut));

        var serviceProvider = services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetAstronautDutiesByNameHandler).Assembly))
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .AddScoped<IAstronautRepository>(x => mockedAstronautRepo.Object)
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var request = new GetAstronautDutiesByName()
        {
            Name = name
        };

        // Act
        var response = await mediator.Send(request);

        // Assert
        mockedAstronautRepo.Verify(m => m.GetDutiesByPersonIdAsync(personAstronaut.PersonId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAstronautDutiesByName_WhenPersonExists_ShouldReturnSuccessfulResponse()
    {
        // Arrange
        var services = new ServiceCollection();

        var name = "Dan Carson";

        var personAstronaut = new PersonAstronaut
        {
            PersonId = 1,
            Name = name,
            CurrentRank = "2LT",
            CurrentDutyTitle = "Commander",
            CareerStartDate = new DateTime(2025, 3, 1)
        };

        var duties = new List<AstronautDuty>()
        {
            new AstronautDuty
            {
                Id = 1,
                PersonId = 1,
                Rank = "2LT",
                DutyTitle = "Commander",
                DutyStartDate = new DateTime(2025, 3, 1)
            }
        };

        var mockedPersonRepo = new Mock<IPersonRepository>();        
        mockedPersonRepo.Setup(x => x.GetPersonAstronautByNameAsync(name, It.IsAny<CancellationToken>())).Returns(Task.FromResult((PersonAstronaut?)personAstronaut));
        var mockedAstronautRepo = new Mock<IAstronautRepository>();
        mockedAstronautRepo.Setup(x => x.GetDutiesByPersonIdAsync(personAstronaut.PersonId, It.IsAny<CancellationToken>())).Returns(Task.FromResult(duties.AsEnumerable()));

        var serviceProvider = services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetAstronautDutiesByNameHandler).Assembly))
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .AddScoped<IAstronautRepository>(x => mockedAstronautRepo.Object)
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var request = new GetAstronautDutiesByName()
        {
            Name = name
        };

        // Act
        var response = await mediator.Send(request);

        // Assert
        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.ResponseCode.Should().Be((int)HttpStatusCode.OK);
        response.Person.Should().NotBeNull();
        ArePersonAstronautsEqual(personAstronaut, response.Person).Should().BeTrue();
        response.AstronautDuties.Should().NotBeNull();
        AreAstronautDutiesEqual(duties, response.AstronautDuties).Should().BeTrue();               
    }

    private static bool ArePersonAstronautsEqual(PersonAstronaut a, PersonAstronaut b)
    {
        return a.PersonId == b.PersonId
            && a.Name.Equals(b.Name)
            && (a.CurrentRank ?? "").Equals(b.CurrentRank ?? "")
            && (a.CurrentDutyTitle ?? "").Equals(b.CurrentDutyTitle ?? "")
            && a.CareerStartDate.Equals(b.CareerStartDate)
            && a.CareerEndDate.Equals(b.CareerEndDate);
    }

    private static bool AreAstronautDutyEqual(AstronautDuty a, AstronautDuty b)
    {
        return a.Id == b.Id
            && a.PersonId == b.PersonId
            && a.Rank.Equals(b.Rank)
            && a.DutyTitle.Equals(b.DutyTitle)
            && a.DutyStartDate.Equals(b.DutyStartDate)
            && a.DutyEndDate.Equals(b.DutyEndDate);
    }

    private static bool AreAstronautDutiesEqual(IEnumerable<AstronautDuty> a, IEnumerable<AstronautDuty> b)
    {
        if (a.Count() != b.Count())
            return false;

        foreach(var duty in a)
        {
            var match = b.FirstOrDefault(x => AreAstronautDutyEqual(duty, x));
            if (match is null)
                return false;
        }

        return true;
    }
}