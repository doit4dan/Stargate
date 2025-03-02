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
using System.Net;
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

    [Fact]
    public async Task CreateAstronautDuty_ShouldCallCreateDetailAsync_Once_WhenAstronautDetailsIsNull()
    {
        // Arrange
        var services = new ServiceCollection();

        var name = "Dan Carson";
        var personId = 1;

        var mockedPersonRepo = new Mock<IPersonRepository>();
        mockedPersonRepo.Setup(x => x.GetPersonIdByNameAsync(name, It.IsAny<CancellationToken>())).Returns(Task.FromResult(personId));

        var mockedAstronautRepo = new Mock<IAstronautRepository>();
        mockedAstronautRepo.Setup(x => x.GetDetailByPersonIdAsync(personId, It.IsAny<CancellationToken>())).Returns(Task.FromResult((AstronautDetail?)null));

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
        mockedAstronautRepo.Verify(m => m.CreateDetailAsync(It.Is<AstronautDetail>(x =>        
            x.PersonId == personId &&
            x.CurrentDutyTitle.Equals(request.DutyTitle) &&
            x.CurrentRank.Equals(request.Rank) &&
            x.CareerStartDate.Equals(request.DutyStartDate)
        ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAstronautDuty_WhenAstronauntDetailsIsNull_WithRetiredDutyTitle_ValidateCareerEndDateIsUpdated()
    {
        // Arrange
        var services = new ServiceCollection();

        var name = "Dan Carson";
        var personId = 1;

        var mockedPersonRepo = new Mock<IPersonRepository>();
        mockedPersonRepo.Setup(x => x.GetPersonIdByNameAsync(name, It.IsAny<CancellationToken>())).Returns(Task.FromResult(personId));

        var mockedAstronautRepo = new Mock<IAstronautRepository>();
        mockedAstronautRepo.Setup(x => x.GetDetailByPersonIdAsync(personId, It.IsAny<CancellationToken>())).Returns(Task.FromResult((AstronautDetail?)null));

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
            DutyTitle = "RETIRED",
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
        mockedAstronautRepo.Verify(m => m.CreateDetailAsync(It.Is<AstronautDetail>(x =>
            x.PersonId == personId &&
            x.CurrentDutyTitle.Equals(request.DutyTitle) &&
            x.CurrentRank.Equals(request.Rank) &&
            x.CareerStartDate.Equals(request.DutyStartDate) &&
            x.CareerEndDate.Equals(request.DutyStartDate.AddDays(-1).Date)
        ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAstronautDuty_ShouldCallUpdateDetailAsync_Once_WhenAstronautDetailsIsNotNull()
    {
        // Arrange
        var services = new ServiceCollection();

        var name = "Dan Carson";
        var personId = 1;

        var currentDetail = new AstronautDetail
        {
            Id = 1,
            PersonId = personId,
            CurrentRank = "2LT",
            CurrentDutyTitle = "Pilot",
            CareerStartDate = new DateTime(2023, 3, 1)
        };

        var mockedPersonRepo = new Mock<IPersonRepository>();
        mockedPersonRepo.Setup(x => x.GetPersonIdByNameAsync(name, It.IsAny<CancellationToken>())).Returns(Task.FromResult(personId));

        var mockedAstronautRepo = new Mock<IAstronautRepository>();
        mockedAstronautRepo.Setup(x => x.GetDetailByPersonIdAsync(personId, It.IsAny<CancellationToken>())).Returns(Task.FromResult((AstronautDetail?)currentDetail));

        var serviceProvider = services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateAstronautDutyHandler).Assembly))
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .AddScoped<IAstronautRepository>(x => mockedAstronautRepo.Object)
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var request = new CreateAstronautDuty
        {
            Name = name,
            Rank = "1LT",
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
        mockedAstronautRepo.Verify(m => m.UpdateDetailAsync(It.Is<AstronautDetail>(x =>
            x.PersonId == personId &&
            x.CurrentDutyTitle.Equals(request.DutyTitle) &&
            x.CurrentRank.Equals(request.Rank)             
        ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAstronautDuty_WhenAstronauntDetailsIsNotNull_WithRetiredDutyTitle_ValidateCareerEndDateIsUpdated()
    {
        // Arrange
        var services = new ServiceCollection();

        var name = "Dan Carson";
        var personId = 1;

        var currentDetail = new AstronautDetail
        {
            Id = 1,
            PersonId = personId,
            CurrentRank = "2LT",
            CurrentDutyTitle = "Pilot",
            CareerStartDate = new DateTime(2023, 3, 1)
        };

        var mockedPersonRepo = new Mock<IPersonRepository>();
        mockedPersonRepo.Setup(x => x.GetPersonIdByNameAsync(name, It.IsAny<CancellationToken>())).Returns(Task.FromResult(personId));

        var mockedAstronautRepo = new Mock<IAstronautRepository>();
        mockedAstronautRepo.Setup(x => x.GetDetailByPersonIdAsync(personId, It.IsAny<CancellationToken>())).Returns(Task.FromResult((AstronautDetail?)currentDetail));

        var serviceProvider = services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateAstronautDutyHandler).Assembly))
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .AddScoped<IAstronautRepository>(x => mockedAstronautRepo.Object)
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var request = new CreateAstronautDuty
        {
            Name = name,
            Rank = "1LT",
            DutyTitle = "RETIRED",
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
        mockedAstronautRepo.Verify(m => m.UpdateDetailAsync(It.Is<AstronautDetail>(x =>
            x.PersonId == personId &&
            x.CurrentDutyTitle.Equals(request.DutyTitle) &&
            x.CurrentRank.Equals(request.Rank) && 
            x.CareerEndDate.Equals(request.DutyStartDate.AddDays(-1).Date)
        ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAstronautDuty_ThrowsBadHttpRequestException_WhenAstronautDetailsCreateFails()
    {
        // Arrange
        var services = new ServiceCollection();

        var name = "Dan Carson";
        var personId = 1;

        var mockedPersonRepo = new Mock<IPersonRepository>();
        mockedPersonRepo.Setup(x => x.GetPersonIdByNameAsync(name, It.IsAny<CancellationToken>())).Returns(Task.FromResult(personId));

        var mockedAstronautRepo = new Mock<IAstronautRepository>();
        mockedAstronautRepo.Setup(x => x.GetDetailByPersonIdAsync(personId, It.IsAny<CancellationToken>())).Returns(Task.FromResult((AstronautDetail?)null));

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
        badHttpRequestException.Message.Should().Be("Failed to add/update Astronaunt Detail in the system..");
    }

    [Fact]
    public async Task CreateAstronautDuty_ThrowsBadHttpRequestException_WhenAstronautDetailsUpdateFails()
    {
        // Arrange
        var services = new ServiceCollection();

        var name = "Dan Carson";
        var personId = 1;

        var currentDetail = new AstronautDetail
        {
            Id = 1,
            PersonId = personId,
            CurrentRank = "2LT",
            CurrentDutyTitle = "Pilot",
            CareerStartDate = new DateTime(2023, 3, 1)
        };

        var mockedPersonRepo = new Mock<IPersonRepository>();
        mockedPersonRepo.Setup(x => x.GetPersonIdByNameAsync(name, It.IsAny<CancellationToken>())).Returns(Task.FromResult(personId));

        var mockedAstronautRepo = new Mock<IAstronautRepository>();
        mockedAstronautRepo.Setup(x => x.GetDetailByPersonIdAsync(personId, It.IsAny<CancellationToken>())).Returns(Task.FromResult((AstronautDetail?)currentDetail));

        var serviceProvider = services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateAstronautDutyHandler).Assembly))
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .AddScoped<IAstronautRepository>(x => mockedAstronautRepo.Object)
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var request = new CreateAstronautDuty
        {
            Name = name,
            Rank = "1LT",
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
        badHttpRequestException.Message.Should().Be("Failed to add/update Astronaunt Detail in the system..");
    }

    [Fact]
    public async Task CreateAstronautDuty_ShouldCallCreateDutyAsync_Once_WhenAstronautDetailsCreateSucceeds()
    {
        // Arrange
        var services = new ServiceCollection();

        var name = "Dan Carson";
        var personId = 1;

        var mockedPersonRepo = new Mock<IPersonRepository>();
        mockedPersonRepo.Setup(x => x.GetPersonIdByNameAsync(name, It.IsAny<CancellationToken>())).Returns(Task.FromResult(personId));

        var mockedAstronautRepo = new Mock<IAstronautRepository>();
        mockedAstronautRepo.Setup(x => x.GetDetailByPersonIdAsync(personId, It.IsAny<CancellationToken>())).Returns(Task.FromResult((AstronautDetail?)null));
        mockedAstronautRepo.Setup(x => x.CreateDetailAsync(It.IsAny<AstronautDetail>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));

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
        var response = await mediator.Send(request);

        // Assert
        mockedAstronautRepo.Verify(m => m.CreateDutyAsync(It.Is<AstronautDuty>(x =>
            x.PersonId == personId &&
            x.Rank.Equals(request.Rank) &&
            x.DutyTitle.Equals(request.DutyTitle) &&
            x.DutyStartDate.Equals(request.DutyStartDate.Date) &&
            x.DutyEndDate == null
        ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAstronautDuty_ShouldCallCreateDutyAsync_Once_WhenAstronautDetailsUpdateSucceeds()
    {
        // Arrange
        var services = new ServiceCollection();

        var name = "Dan Carson";
        var personId = 1;

        var currentDetail = new AstronautDetail
        {
            Id = 1,
            PersonId = personId,
            CurrentRank = "2LT",
            CurrentDutyTitle = "Pilot",
            CareerStartDate = new DateTime(2023, 3, 1)
        };

        var mockedPersonRepo = new Mock<IPersonRepository>();
        mockedPersonRepo.Setup(x => x.GetPersonIdByNameAsync(name, It.IsAny<CancellationToken>())).Returns(Task.FromResult(personId));

        var mockedAstronautRepo = new Mock<IAstronautRepository>();
        mockedAstronautRepo.Setup(x => x.GetDetailByPersonIdAsync(personId, It.IsAny<CancellationToken>())).Returns(Task.FromResult((AstronautDetail?)currentDetail));
        mockedAstronautRepo.Setup(x => x.UpdateDetailAsync(It.IsAny<AstronautDetail>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));

        var serviceProvider = services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateAstronautDutyHandler).Assembly))
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .AddScoped<IAstronautRepository>(x => mockedAstronautRepo.Object)
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var request = new CreateAstronautDuty
        {
            Name = name,
            Rank = "1LT",
            DutyTitle = "Commander",
            DutyStartDate = new DateTime(2025, 3, 1)
        };        

        // Act
        var response = await mediator.Send(request);

        // Assert
        mockedAstronautRepo.Verify(m => m.CreateDutyAsync(It.Is<AstronautDuty>(x =>
            x.PersonId == personId &&
            x.Rank.Equals(request.Rank) &&
            x.DutyTitle.Equals(request.DutyTitle) &&
            x.DutyStartDate.Equals(request.DutyStartDate.Date) &&
            x.DutyEndDate == null
        ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAstronautDuty_ReturnsSuccessfulResponse_WhenCreateDutyAsyncSucceeds()
    {
        // Arrange
        var services = new ServiceCollection();

        var name = "Dan Carson";
        var personId = 1;

        var mockedPersonRepo = new Mock<IPersonRepository>();
        mockedPersonRepo.Setup(x => x.GetPersonIdByNameAsync(name, It.IsAny<CancellationToken>())).Returns(Task.FromResult(personId));

        var mockedAstronautRepo = new Mock<IAstronautRepository>();
        mockedAstronautRepo.Setup(x => x.GetDetailByPersonIdAsync(personId, It.IsAny<CancellationToken>())).Returns(Task.FromResult((AstronautDetail?)null));
        mockedAstronautRepo.Setup(x => x.CreateDetailAsync(It.IsAny<AstronautDetail>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));
        mockedAstronautRepo.Setup(x => x.CreateDutyAsync(It.IsAny<AstronautDuty>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));

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
        var response = await mediator.Send(request);

        // Assert
        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.ResponseCode.Should().Be((int)HttpStatusCode.OK);
        response.Message.Should().Be("Successfully recorded Astronaut Duty Details in system");
    }

    [Fact]
    public async Task CreateAstronautDuty_ReturnsFailureResponse_WhenCreateDutyAsyncFails()
    {
        // Arrange
        var services = new ServiceCollection();

        var name = "Dan Carson";
        var personId = 1;

        var mockedPersonRepo = new Mock<IPersonRepository>();
        mockedPersonRepo.Setup(x => x.GetPersonIdByNameAsync(name, It.IsAny<CancellationToken>())).Returns(Task.FromResult(personId));

        var mockedAstronautRepo = new Mock<IAstronautRepository>();
        mockedAstronautRepo.Setup(x => x.GetDetailByPersonIdAsync(personId, It.IsAny<CancellationToken>())).Returns(Task.FromResult((AstronautDetail?)null));
        mockedAstronautRepo.Setup(x => x.CreateDetailAsync(It.IsAny<AstronautDetail>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));
        mockedAstronautRepo.Setup(x => x.CreateDutyAsync(It.IsAny<AstronautDuty>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(false));

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
        var response = await mediator.Send(request);

        // Assert
        response.Should().NotBeNull();
        response.Success.Should().BeFalse();
        response.ResponseCode.Should().Be((int)HttpStatusCode.InternalServerError);
        response.Message.Should().Be("Failed to record Astronaut Duty Details in system...");
    }
}
