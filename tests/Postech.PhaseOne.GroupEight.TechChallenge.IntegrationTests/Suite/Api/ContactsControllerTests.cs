﻿using Bogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Postech.PhaseOne.GroupEight.TechChallenge.Domain.Commands.Inputs;
using Postech.PhaseOne.GroupEight.TechChallenge.Domain.Commands.Outputs;
using Postech.PhaseOne.GroupEight.TechChallenge.Domain.Entities;
using Postech.PhaseOne.GroupEight.TechChallenge.Domain.Interfaces.Repositories;
using Postech.PhaseOne.GroupEight.TechChallenge.Domain.ValueObjects;
using Postech.PhaseOne.GroupEight.TechChallenge.Infra.Data.Contexts;
using Postech.PhaseOne.GroupEight.TechChallenge.IntegrationTests.Configurations.Factories;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Postech.PhaseOne.GroupEight.TechChallenge.IntegrationTests.Suite.Api
{
    public class ContactsControllerTests(ContactManagementAppWebApplicationFactory factory) : IClassFixture<ContactManagementAppWebApplicationFactory>, IAsyncLifetime
    {
        private readonly HttpClient _client = factory.CreateClient();

        private readonly ContactManagementAppWebApplicationFactory _factory = factory;
        private readonly Faker _faker = new("pt_BR");

        public async Task DisposeAsync()
        {
            await _factory.DisposeAsync();
        }

        public async Task InitializeAsync()
        {
            await _factory.InitializeContainerAsync();
            using IServiceScope scope = _factory.Services.CreateScope();
            ContactManagementDbContext context = scope.ServiceProvider.GetRequiredService<ContactManagementDbContext>();
            await context.Database.MigrateAsync();
        }

        [Fact]
        public async Task DeleteContactEndpoint_DeleteAnExistingContact_ShouldDeleteTheContact()
        {
            // Arrange
            using IServiceScope scope = _factory.Services.CreateScope();
            IContactRepository contactRepository = scope.ServiceProvider.GetRequiredService<IContactRepository>();
            ContactNameValueObject contactName = new(_faker.Name.FirstName(), _faker.Name.LastName());
            ContactEmailValueObject contactEmail = new(_faker.Internet.Email());
            AreaCodeValueObject areaCode = await contactRepository.GetAreaCodeByValueAsync("11");
            ContactPhoneValueObject contactPhone = new(_faker.Phone.PhoneNumber("9########"), areaCode);
            ContactEntity contactEntity = new(contactName, contactEmail, contactPhone);
            await contactRepository.InsertAsync(contactEntity);
            await contactRepository.SaveChangesAsync();
            DeleteContactInput input = new() { ContactId = contactEntity.Id };

            // Act
            using HttpClient httpClient = _factory.CreateClient();
            using HttpResponseMessage responseMessage = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, "/contacts")
            {
                Content = new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, "application/json")
            });

            // Assert
            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
            DefaultOutput? responseMessageContent = JsonSerializer.Deserialize<DefaultOutput>(await responseMessage.Content.ReadAsStringAsync());
            responseMessageContent.Should().NotBeNull();
            responseMessageContent?.Message.Should().NotBeNullOrEmpty();
            responseMessageContent?.Success.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateContactEndpoint_UpdateAnExistingContact_ShouldUpdateTheContact()
        {
            // Arrange
            using IServiceScope scope = _factory.Services.CreateScope();
            IContactRepository contactRepository = scope.ServiceProvider.GetRequiredService<IContactRepository>();
            ContactNameValueObject contactName = new(_faker.Name.FirstName(), _faker.Name.LastName());
            ContactEmailValueObject contactEmail = new(_faker.Internet.Email());
            AreaCodeValueObject areaCode = await contactRepository.GetAreaCodeByValueAsync("11");
            ContactPhoneValueObject contactPhone = new(_faker.Phone.PhoneNumber("9########"), areaCode);
            ContactEntity contactEntity = new(contactName, contactEmail, contactPhone);
            await contactRepository.InsertAsync(contactEntity);
            await contactRepository.SaveChangesAsync();

            var updateContactInput = new UpdateContactInput
            {
                ContactId = contactEntity.Id,
                ContactFirstName = _faker.Name.FirstName(),
                ContactLastName = _faker.Name.LastName(),
                ContactEmail = _faker.Internet.Email(),
                ContactPhoneNumber = _faker.Phone.PhoneNumber("9########"),
                ContactPhoneNumberAreaCode = "31",
                IsActive = _faker.Random.Bool()
            };

            // Act
            using HttpClient httpClient = _factory.CreateClient();
            using HttpResponseMessage responseMessage = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Put, "/contacts")
            {
                Content = new StringContent(JsonSerializer.Serialize(updateContactInput), Encoding.UTF8, "application/json")
            });

            // Assert
            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
            DefaultOutput? responseMessageContent = JsonSerializer.Deserialize<DefaultOutput>(await responseMessage.Content.ReadAsStringAsync());
            responseMessageContent.Should().NotBeNull();
            responseMessageContent?.Message.Should().NotBeNullOrEmpty();
            responseMessageContent?.Success.Should().BeTrue();
        }
        
        [Theory(DisplayName = "Inserting contact with success")]
        [InlineData("Tatiana", "Lima", "tatidornel@gmail.com", "974025307", "51")]
        [InlineData("Elias", "Rosa", "eliasrosa@gmail.com", "974025308", "11")]
        [InlineData("Veronica", "Freitas", "veronica@gmail.com", "974025309", "38")]
        [Trait("Action", "Controller")]
        public async Task Controller_InsertingContact_ShouldBeOk(string name,
                string lastName, string email, string phone, string areaCode)
        {
            ContactInput input = new()
            {
                AreaCode = areaCode,
                Name = name,
                Email = email,
                Phone = phone,
                LastName = lastName,
            };
            var content = ContentHelper.GetStringContent(input);
            var response = await _client.PostAsync("/contacts", content);
            var result = JsonConvert.DeserializeObject<DefaultOutput>(response.Content.ReadAsStringAsync().Result);

            Assert.NotNull(result);
            Assert.True(result.Success);
        }
    }
}