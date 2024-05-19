using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Postech.PhaseOne.GroupEight.TechChallenge.Api.Setup;
using Postech.PhaseOne.GroupEight.TechChallenge.Domain.Commands.Inputs;
using Postech.PhaseOne.GroupEight.TechChallenge.Domain.Commands.Outputs;
using Postech.PhaseOne.GroupEight.TechChallenge.Domain.Exceptions.Common;
using Postech.PhaseOne.GroupEight.TechChallenge.Domain.Extensions;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IConfigurationRoot configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "ContactManagement API (Tech Challenge)", 
        Version = "v1", 
        Description = "Alunos responsáveis: Breno Gomes, Lucas Pinho, Lucas Ruiz, Ricardo Fulgencio e Tatiana Lima"
    });
    c.EnableAnnotations();
});
builder.Services.AddDbContext(configuration);
builder.Services.AddMediatR();
builder.Services.AddDependencyRepository();
builder.Services.AddDependencyFactory();
builder.Services.AddDependencyHandler();
builder.Services.AddMiniProfiler();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMiniProfiler();
    app.UseSwagger();
    app.MapSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));        
}
app.UseHttpsRedirection();
app.UseExceptionHandler(configure =>
{
    configure.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        if (exceptionHandlerPathFeature?.Error is not null)
        {
            int statusCode = (int) HttpStatusCode.InternalServerError;
            string errorMessage = exceptionHandlerPathFeature.Error.Message;
            if (exceptionHandlerPathFeature?.Error is DomainException)
            {
                statusCode = (int) HttpStatusCode.BadRequest;               
            }
            else if (exceptionHandlerPathFeature?.Error is NotFoundException)
            {
                statusCode = (int) HttpStatusCode.NotFound;
            } 
            else
            {
                errorMessage = "Ocorreu um erro inesperado";
            }        
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(new DefaultOutput(false, errorMessage)); 
        }
    });
});

app.MapPost("/contacts", async (IMediator mediator, [FromBody] AddContactInput request) =>
{
    DomainException.ThrowWhenThereAreErrorMessages(request.Validate());
    return await mediator.Send(request);
})
.WithName("Register Contact")
.WithMetadata(new SwaggerOperationAttribute(
        "Register a new contact",
        "Registers a new contact according to their first and last name, email address and phone number."
    )
)
.WithMetadata(new SwaggerParameterAttribute("New contact information"))
.WithMetadata(new SwaggerResponseAttribute(200, "Contact registered successfully"))
.WithMetadata(new SwaggerResponseAttribute(400, "The data provided for contact registration is invalid"))
.WithMetadata(new SwaggerResponseAttribute(500, "Unexpected error during contact registration"))
.WithOpenApi();

app.MapDelete("/contacts", async (IMediator mediator, [FromBody] DeleteContactInput request) =>
{
    DomainException.ThrowWhenThereAreErrorMessages(request.Validate());
    return await mediator.Send(request);
})
.WithName("Delete Contact")
.WithMetadata(new SwaggerOperationAttribute(
        "Deletes an existing contact",
        "Deletes an existing contact according to its identifier."
    )
)
.WithMetadata(new SwaggerParameterAttribute("Data for deleting the contact"))
.WithMetadata(new SwaggerResponseAttribute(200, "The contact was successfully deleted"))
.WithMetadata(new SwaggerResponseAttribute(400, "The data provided to delete the contact is invalid or the contact has already been deleted"))
.WithMetadata(new SwaggerResponseAttribute(404, "The contact provided for deletion does not exist"))
.WithMetadata(new SwaggerResponseAttribute(500, "Unexpected error while deleting contact"))
.WithOpenApi();

app.MapPut("/contacts", async (IMediator mediator, [FromBody] UpdateContactInput request) =>
{
    return await mediator.Send(request);
})
.WithName("Update Contact")
.WithMetadata(new SwaggerOperationAttribute
                        ("Modify a contact in the Agenda",
                        "Modifies a contact in the agenda according to the provided data"))
.WithMetadata(new SwaggerParameterAttribute("Data of the new contact"))
.WithMetadata(new SwaggerResponseAttribute(200, "Contact updated"))
.WithMetadata(new SwaggerResponseAttribute(400, "Invalid request"))
.WithMetadata(new SwaggerResponseAttribute(500, "Unexpected error"))
.WithOpenApi();
app.Run();

public partial class Program
{
    protected Program() { }
}