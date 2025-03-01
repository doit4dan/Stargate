using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Stargate.Server.Business.Commands;
using Stargate.Server.Business.Queries;
using Stargate.Server.Data.Models;
using Stargate.Server.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stargate.Server.Tests;

public class CreateAstronautDutyTests
{
    [Fact]
    public async Task CreateAstronautDuty_ShouldCallGetPersonIdByNameAsync_Once()
    {
        // Arrange
        var services = new ServiceCollection();

        var name = "Dan Carson";        

        var mockedPersonRepo = new Mock<IPersonRepository>();        

        var mockedAstronautRepo = new Mock<IAstronautRepository>();

        var serviceProvider = services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateAstronautDutyHandler).Assembly))
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .AddScoped<IAstronautRepository>(x => mockedAstronautRepo.Object)
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var request = new CreateAstronautDuty
        {
            Name = name,
            Rank = "2LT",
            DutyTitle = "Commander",
            DutyStartDate = new DateTime(2025,3,1)
        };

        // Act
        
        // Method is not fully set up, only testing beggining piece so adding Try Catch
        try
        {
            var response = await mediator.Send(request);
        }
        catch (Exception ex)
        {
        }        

        // Assert
        mockedPersonRepo.Verify(m => m.GetPersonIdByNameAsync(name, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAstronautDuty_ShouldThrowBadHttpRequestException_WhenPersonDoesNotExist()
    {
        // Arrange
        var services = new ServiceCollection();

        var name = "Dan Carson";

        var mockedPersonRepo = new Mock<IPersonRepository>();

        var mockedAstronautRepo = new Mock<IAstronautRepository>();

        var serviceProvider = services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateAstronautDutyHandler).Assembly))
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .AddScoped<IAstronautRepository>(x => mockedAstronautRepo.Object)
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var request = new CreateAstronautDuty
        {
            Name = name,
            Rank = "2LT",
            DutyTitle = "Commander",
            DutyStartDate = new DateTime(2025, 3, 1)
        };

        BadHttpRequestException? badHttpRequestException = null;

        // Act        
        try
        {
            var response = await mediator.Send(request);
        }
        catch (BadHttpRequestException ex)
        {
            badHttpRequestException = ex;
        }

        // Assert
        badHttpRequestException.Should().NotBeNull();
        badHttpRequestException.Message.Should().Be("Provided Person name does not exist in system");
    }

    [Fact]
    public async Task CreateAstronautDuty_ShouldCallGetDetailByPersonIdAsync_Once_WhenPersonExists()
    {
        // Arrange
        var services = new ServiceCollection();

        var name = "Dan Carson";
        var personId = 1;

        var mockedPersonRepo = new Mock<IPersonRepository>();
        mockedPersonRepo.Setup(x => x.GetPersonIdByNameAsync(name, It.IsAny<CancellationToken>())).Returns(Task.FromResult(personId));

        var mockedAstronautRepo = new Mock<IAstronautRepository>();

        var serviceProvider = services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateAstronautDutyHandler).Assembly))
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .AddScoped<IAstronautRepository>(x => mockedAstronautRepo.Object)
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var request = new CreateAstronautDuty
        {
            Name = name,
            Rank = "2LT",
            DutyTitle = "Commander",
            DutyStartDate = new DateTime(2025, 3, 1)
        };

        // Act

        // Method is not fully set up, only testing GetDetailByPersonIdAsync call so adding Try Catch
        try
        {
            var response = await mediator.Send(request);
        }
        catch (Exception ex)
        {            
        }

        // Assert
        mockedAstronautRepo.Verify(m => m.GetDetailByPersonIdAsync(personId, It.IsAny<CancellationToken>()), Times.Once);
    }
}
