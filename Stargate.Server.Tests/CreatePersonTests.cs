using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
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

        //var mockedValidator = new Mock<IValidator<CreatePerson>>();

        var serviceProvider = services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetPersonByNameHandler).Assembly))
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .AddValidatorsFromAssemblyContaining<CreatePersonValidator>()
            //.AddScoped(x => mockedValidator)
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

        //var mockedValidator = new Mock<IValidator<CreatePerson>>();

        var serviceProvider = services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetPersonByNameHandler).Assembly))
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .AddValidatorsFromAssemblyContaining<CreatePersonValidator>()
            //.AddScoped(x => mockedValidator)
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

        //var mockedValidator = new Mock<IValidator<CreatePerson>>();

        var serviceProvider = services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetPersonByNameHandler).Assembly))
            .AddScoped<IPersonRepository>(x => mockedPersonRepo.Object)
            .AddValidatorsFromAssemblyContaining<CreatePersonValidator>()
            //.AddScoped(x => mockedValidator)
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
}
