using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Stargate.Server.Business.Commands;
using Stargate.Server.Data.Models;
using Stargate.Server.Repositories;
using Stargate.Server.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Stargate.Server.Tests;

public class UpdatePersonTests
{
    [Fact]
    public async Task CreatePerson_ShouldCallGetPersonByNameAsync_Once()
    {
        // Arrange
        var services = new ServiceCollection();

        var name = "Dan Carson";
        var person = new Person
        {
            Id = 1,
            Name = name
        };

        var mockedPersonRepo = new Mock<IPersonRepository>();
        mockedPersonRepo.Setup(x => x.GetPersonByNameAsync(name, It.IsAny<CancellationToken>())).Returns(Task.FromResult((Person?)person));

        var serviceProvider = services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(UpdatePersonHandler).Assembly))
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)            
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();        
        var request = new UpdatePerson() { Name = name, NewName = "Daniel Carson" };

        // Act
        var response = await mediator.Send(request);

        // Assert
        mockedPersonRepo.Verify(m => m.GetPersonByNameAsync(name, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreatePerson_ShouldThrowBadHttpRequestException_WhenPersonDoesNotExist()
    {
        // Arrange
        var services = new ServiceCollection();

        var name = "Dan Carson";
        var person = new Person
        {
            Id = 1,
            Name = name
        };

        var mockedPersonRepo = new Mock<IPersonRepository>();
        mockedPersonRepo.Setup(x => x.GetPersonByNameAsync(name, It.IsAny<CancellationToken>())).Returns(Task.FromResult((Person?)null));

        var serviceProvider = services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(UpdatePersonHandler).Assembly))
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)            
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var request = new UpdatePerson() { Name = name, NewName = "Daniel Carson" };

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
        badHttpRequestException.Message.Should().Be("Bad Request, Person does not exist..");
    }

    [Fact]
    public async Task CreatePerson_ShouldCallUpdateAsyncOnce_WhenPersonExists()
    {
        // Arrange
        var services = new ServiceCollection();

        var name = "Dan Carson";
        var person = new Person
        {
            Id = 1,
            Name = name
        };

        var mockedPersonRepo = new Mock<IPersonRepository>();
        mockedPersonRepo.Setup(x => x.GetPersonByNameAsync(name, It.IsAny<CancellationToken>())).Returns(Task.FromResult((Person?)person));

        var serviceProvider = services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(UpdatePersonHandler).Assembly))
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)            
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var request = new UpdatePerson() { Name = name, NewName = "Daniel Carson" };

        // Act
        var response = await mediator.Send(request);

        // Assert
        mockedPersonRepo.Verify(m => m.UpdateAsync(person, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreatePerson_ShouldReturnSuccessfulResponse_WhenPersonUpdatedSuccessfully()
    {
        // Arrange
        var services = new ServiceCollection();

        var personId = 1;
        var name = "Dan Carson";
        var newName = "Daniel Carson";

        var person = new Person
        {
            Id = personId,
            Name = name
        };

        var mockedPersonRepo = new Mock<IPersonRepository>();
        mockedPersonRepo.Setup(x => x.GetPersonByNameAsync(name, It.IsAny<CancellationToken>())).Returns(Task.FromResult((Person?)person));
        mockedPersonRepo.Setup(x => x.UpdateAsync(It.Is<Person>(p => p.Id == personId && p.Name == newName), It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));

        var serviceProvider = services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(UpdatePersonHandler).Assembly))
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)            
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var request = new UpdatePerson() { Name = name, NewName = newName };

        // Act
        var response = await mediator.Send(request);

        // Assert
        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.ResponseCode.Should().Be((int)HttpStatusCode.OK);
        response.Message.Should().Be("Person successfully updated in the system");
        response.Id.Should().Be(personId);
        response.Name.Should().Be(newName);
    }

    [Fact]
    public async Task CreatePerson_ShouldReturnFailedResponse_WhenPersonUpdateFailed()
    {
        // Arrange
        var services = new ServiceCollection();

        var personId = 1;
        var name = "Dan Carson";
        var newName = "Daniel Carson";

        var person = new Person
        {
            Id = personId,
            Name = name
        };

        var mockedPersonRepo = new Mock<IPersonRepository>();
        mockedPersonRepo.Setup(x => x.GetPersonByNameAsync(name, It.IsAny<CancellationToken>())).Returns(Task.FromResult((Person?)person));
        mockedPersonRepo.Setup(x => x.UpdateAsync(It.Is<Person>(p => p.Id == personId && p.Name == newName), It.IsAny<CancellationToken>())).Returns(Task.FromResult(false));

        var serviceProvider = services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(UpdatePersonHandler).Assembly))
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)            
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var request = new UpdatePerson() { Name = name, NewName = newName };

        // Act
        var response = await mediator.Send(request);

        // Assert
        response.Should().NotBeNull();
        response.Success.Should().BeFalse();
        response.ResponseCode.Should().Be((int)HttpStatusCode.InternalServerError);
        response.Message.Should().Be("System error, failed to update Person in the system..");
        response.Id.Should().Be(personId);
        response.Name.Should().Be(newName);
    }

    [Fact]
    public async Task UpdatePerson_ThrowsValidationException_WhenPersonDoesNotExist()
    {
        // Arrange
        var services = new ServiceCollection();

        var name = "Dan Carson";
        var validNewName = "Daniel Carson";

        var mockedPersonRepo = new Mock<IPersonRepository>();
        mockedPersonRepo.Setup(x => x.ExistsByNameAsync(name, It.IsAny<CancellationToken>())).Returns(Task.FromResult(false));
        mockedPersonRepo.Setup(x => x.ExistsByNameAsync(validNewName, It.IsAny<CancellationToken>())).Returns(Task.FromResult(false));

        var serviceProvider = services
            .AddMediatR(cfg =>
            {
                cfg.AddRequestPreProcessor<UpdatePersonPreProcessor>();
                cfg.RegisterServicesFromAssemblies(typeof(UpdatePersonHandler).Assembly);
            })
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .AddValidatorsFromAssemblyContaining<UpdatePersonValidator>()
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var request = new UpdatePerson() { Name = name, NewName = validNewName };
        ValidationException? validationException = null;

        // Act
        try
        {
            var response = await mediator.Send(request);
        }
        catch (ValidationException ex)
        {
            validationException = ex;
        }

        // Assert
        validationException.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdatePerson_ThrowsValidationException_WhenPersonExists_And_NewNameExists()
    {
        // Arrange
        var services = new ServiceCollection();

        var name = "Dan Carson";
        var newName = "Teresa Carson";

        var mockedPersonRepo = new Mock<IPersonRepository>();
        mockedPersonRepo.Setup(x => x.ExistsByNameAsync(name, It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));
        mockedPersonRepo.Setup(x => x.ExistsByNameAsync(newName, It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));

        var serviceProvider = services
            .AddMediatR(cfg =>
            {
                cfg.AddRequestPreProcessor<UpdatePersonPreProcessor>();
                cfg.RegisterServicesFromAssemblies(typeof(UpdatePersonHandler).Assembly);
            })
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .AddValidatorsFromAssemblyContaining<UpdatePersonValidator>()
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var request = new UpdatePerson() { Name = name, NewName = newName };
        ValidationException? validationException = null;

        // Act
        try
        {
            var response = await mediator.Send(request);
        }
        catch (ValidationException ex)
        {
            validationException = ex;
        }

        // Assert
        validationException.Should().NotBeNull();
    }

    [Theory]
    [InlineData("")] // Empty String
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")] // Length > 50
    [InlineData("Dan1 Carson2!")] // Special Charcters, numbers not allowed
    [InlineData("Dan  Carson")] // Name should only have one space
    public async Task UpdatePerson_ThrowsValidationException_WhenNewNameIsInvalid(string newName)
    {
        // Arrange
        var services = new ServiceCollection();

        var name = "Dan Carson";        

        var mockedPersonRepo = new Mock<IPersonRepository>();
        mockedPersonRepo.Setup(x => x.ExistsByNameAsync(name, It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));
        mockedPersonRepo.Setup(x => x.ExistsByNameAsync(newName, It.IsAny<CancellationToken>())).Returns(Task.FromResult(false));

        var serviceProvider = services
            .AddMediatR(cfg =>
            {
                cfg.AddRequestPreProcessor<UpdatePersonPreProcessor>();
                cfg.RegisterServicesFromAssemblies(typeof(UpdatePersonHandler).Assembly);
            })
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .AddValidatorsFromAssemblyContaining<UpdatePersonValidator>()
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var request = new UpdatePerson() { Name = name, NewName = newName };
        ValidationException? validationException = null;

        // Act
        try
        {
            var response = await mediator.Send(request);
        }
        catch (ValidationException ex)
        {
            validationException = ex;
        }

        // Assert
        validationException.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdatePerson_DoesNotThrowValidationException_WhenRequestIsValid()
    {
        // Arrange
        var services = new ServiceCollection();

        var name = "Dan Carson";
        var newName = "Daniel Carson";

        var person = new Person
        {
            Id = 1,
            Name = name
        };

        var mockedPersonRepo = new Mock<IPersonRepository>();                
        mockedPersonRepo.Setup(x => x.GetPersonByNameAsync(name, It.IsAny<CancellationToken>())).Returns(Task.FromResult((Person?)person));
        mockedPersonRepo.Setup(x => x.ExistsByNameAsync(name, It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));
        mockedPersonRepo.Setup(x => x.ExistsByNameAsync(newName, It.IsAny<CancellationToken>())).Returns(Task.FromResult(false));

        var serviceProvider = services
            .AddMediatR(cfg =>
            {
                cfg.AddRequestPreProcessor<UpdatePersonPreProcessor>();
                cfg.RegisterServicesFromAssemblies(typeof(UpdatePersonHandler).Assembly);
            })
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .AddValidatorsFromAssemblyContaining<UpdatePersonValidator>()
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var request = new UpdatePerson() { Name = name, NewName = newName };
        ValidationException? validationException = null;

        // Act
        try
        {
            var response = await mediator.Send(request);
        }
        catch (ValidationException ex)
        {
            validationException = ex;
        }

        // Assert
        validationException.Should().BeNull();
    }
}
