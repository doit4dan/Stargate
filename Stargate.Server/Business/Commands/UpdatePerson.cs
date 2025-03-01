using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using Stargate.Server.Controllers;
using Stargate.Server.Data;

namespace Stargate.Server.Business.Commands;

public class UpdatePerson : IRequest<UpdatePersonResult>
{
    public string Name { get; set; }

    public string NewName { get; set; }
}

public class UpdatePersonPreProcessor : IRequestPreProcessor<UpdatePerson>
{
    private readonly StargateContext _context;
    public UpdatePersonPreProcessor(StargateContext context)
    {
        _context = context;
    }
    public async Task Process(UpdatePerson request, CancellationToken cancellationToken)
    {
        var person = await _context.People.AsNoTracking().FirstOrDefaultAsync(z => z.Name == request.Name, cancellationToken);

        if (person is null) throw new BadHttpRequestException("Bad Request, Person does not exist..");        
    }
}

public class UpdatePersonHandler : IRequestHandler<UpdatePerson, UpdatePersonResult>
{
    private readonly StargateContext _context;

    public UpdatePersonHandler(StargateContext context)
    {
        _context = context;
    }
    public async Task<UpdatePersonResult> Handle(UpdatePerson request, CancellationToken cancellationToken)
    {
        var person = await  _context.People.FirstOrDefaultAsync(z => z.Name == request.Name, cancellationToken);

        if (person is null) throw new BadHttpRequestException("Bad Request, Person does not exist..");

        person.Name = request.NewName;

        _context.Update(person);

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdatePersonResult()
        {
            Id = person.Id,
            Name = person.Name
        };
    }
}

public class UpdatePersonResult : BaseResponse
{
    public int Id { get; set; }

    public string Name { get; set; }
}