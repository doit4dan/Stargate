using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using Stargate.Server.Business.Commands;
using Stargate.Server.Business.Queries;
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

public class CreatePersonTests
{
    [Fact]
    public async Task CreatePerson_ShouldCallCreateAsync_Once()
    {
        // Arrange
        var services = new ServiceCollection();

        var mockedPersonRepo = new Mock<IPersonRepository>();                

        var serviceProvider = services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreatePersonHandler).Assembly))
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .AddValidatorsFromAssemblyContaining<CreatePersonValidator>()            
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var request = new CreatePerson() { Name = "Dan Carson" };

        // Act
        var response = await mediator.Send(request);

        // Assert
        mockedPersonRepo.Verify(m => m.CreateAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreatePerson_ShouldReturnOKResponse_WhenCreateIsSuccessful()
    {
        // Arrange
        var services = new ServiceCollection();
       
        var mockedPersonRepo = new Mock<IPersonRepository>();
        mockedPersonRepo.Setup(x => x.CreateAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));        

        var serviceProvider = services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreatePersonHandler).Assembly))
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .AddValidatorsFromAssemblyContaining<CreatePersonValidator>()            
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();
        
        var request = new CreatePerson() { Name = "Dan Carson" };

        // Act
        var response = await mediator.Send(request);

        // Assert
        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.ResponseCode.Should().Be((int)HttpStatusCode.OK);
        response.Message.Should().Be("Person successfully saved in the system");
    }

    [Fact]
    public async Task CreatePerson_ShouldReturnInternalServerErrorResponse_WhenCreateIsNotSuccessful()
    {
        // Arrange
        var services = new ServiceCollection();

        var mockedPersonRepo = new Mock<IPersonRepository>();
        mockedPersonRepo.Setup(x => x.CreateAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(false));        

        var serviceProvider = services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreatePersonHandler).Assembly))
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .AddValidatorsFromAssemblyContaining<CreatePersonValidator>()            
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var request = new CreatePerson() { Name = "Dan Carson" };

        // Act
        var response = await mediator.Send(request);

        // Assert
        response.Should().NotBeNull();
        response.Success.Should().BeFalse();
        response.ResponseCode.Should().Be((int)HttpStatusCode.InternalServerError);
        response.Message.Should().Be("Error occured when attempting to save person in system..");
    }

    [Fact]
    public async Task CreatePerson_ShouldCallExistsByName_Once()
    {
        // Arrange
        var services = new ServiceCollection();

        var name = "Dan Carson";
        var mockedPersonRepo = new Mock<IPersonRepository>();        

        var serviceProvider = services
            .AddMediatR(cfg =>
            {
                cfg.AddRequestPreProcessor<CreatePersonPreProcessor>();
                cfg.RegisterServicesFromAssemblies(typeof(CreatePersonHandler).Assembly);
            })
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .AddValidatorsFromAssemblyContaining<CreatePersonValidator>()
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var request = new CreatePerson() { Name = name };
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
        mockedPersonRepo.Verify(m => m.ExistsByNameAsync(name, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData("")] // Empty String
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")] // Length > 50
    [InlineData("Dan1 Carson2!")] // Special Charcters, numbers not allowed
    [InlineData("Dan  Carson")] // Name should only have one space
    public async Task CreatePerson_WithInvalidNameInRequest_ThrowsValidationException(string name)
    {
        // Arrange
        var services = new ServiceCollection();

        var mockedPersonRepo = new Mock<IPersonRepository>();        

        var serviceProvider = services
            .AddMediatR(cfg =>
            {                
                cfg.AddRequestPreProcessor<CreatePersonPreProcessor>();                
                cfg.RegisterServicesFromAssemblies(typeof(CreatePersonHandler).Assembly);
            })
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .AddValidatorsFromAssemblyContaining<CreatePersonValidator>()
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var request = new CreatePerson() { Name = name };
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
    [InlineData("Dan Carson")]
    [InlineData("Peter Pan")]
    [InlineData("Ana Carson")]
    [InlineData("Kylie Carson")]
    public async Task CreatePerson_WithValidNameInRequest_DoesNotThrowValidationException_WhenUserDoesNotExist(string name)
    {
        // Arrange
        var services = new ServiceCollection();

        var mockedPersonRepo = new Mock<IPersonRepository>();
        mockedPersonRepo.Setup(x => x.ExistsByNameAsync(name, It.IsAny<CancellationToken>())).Returns(Task.FromResult(false));

        var serviceProvider = services
            .AddMediatR(cfg =>
            {
                cfg.AddRequestPreProcessor<CreatePersonPreProcessor>();
                cfg.RegisterServicesFromAssemblies(typeof(CreatePersonHandler).Assembly);
            })
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .AddValidatorsFromAssemblyContaining<CreatePersonValidator>()
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var request = new CreatePerson() { Name = name };
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

    [Fact]
    public async Task CreatePerson_WithValidNameInRequest_ThrowsValidationException_WhenUserExists()
    {
        // Arrange
        var services = new ServiceCollection();

        var name = "Dan Carson";
        var mockedPersonRepo = new Mock<IPersonRepository>();
        mockedPersonRepo.Setup(x => x.ExistsByNameAsync(name, It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));

        var serviceProvider = services
            .AddMediatR(cfg =>
            {
                cfg.AddRequestPreProcessor<CreatePersonPreProcessor>();
                cfg.RegisterServicesFromAssemblies(typeof(CreatePersonHandler).Assembly);
            })
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .AddValidatorsFromAssemblyContaining<CreatePersonValidator>()
            .BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var request = new CreatePerson() { Name = name };
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
}
