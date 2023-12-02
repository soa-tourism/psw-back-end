﻿using Explorer.API.Controllers.Administrator.Administration;
using Explorer.API.Controllers.Author.Administration;
using Explorer.API.Controllers.Tourist.Shopping;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Payments.Infrastructure.Database;
using Explorer.Payments.Tests;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Infrastructure.Database;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Tests.Integration
{
    [Collection("Sequential")]
    public class SaleTests : BasePaymentsIntegrationTest
    {
        public SaleTests(PaymentsTestFactory factory) : base(factory) { }

        [Fact]
        public void GetSale()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();

            // Act
            var result = ((ObjectResult)controller.Get(-1).Result)?.Value as SaleDto;

            result.ShouldNotBeNull();
            result.Id.ShouldBe(-1);
        }

        [Fact]
        public void Creates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();

            var newEntity = new SaleDto()
            {
                Id = -4,
                ToursIds = new List<long>(),
                Discount = 50
            };

      
            // Act
            var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as SaleDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldBe(-4);
            result.Discount.ShouldBe(50);

            // Assert - Database
            var storedEntity = dbContext.Sales.FirstOrDefault(i => i.Id == newEntity.Id);
            storedEntity.ShouldNotBeNull();
            storedEntity.Id.ShouldBe(result.Id);
        }

        [Fact]
        public void Updates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
            var updatedEntity = new SaleDto
            {
                Id = -1,
                Discount = 10
            };

            // Act
            var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as SaleDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldBe(-1);

            // Assert - Database
            var storedEntity = dbContext.Sales.FirstOrDefault(i => i.Discount == 10);
            storedEntity.ShouldNotBeNull();
        }

        [Fact]
        public void Deletes()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();

            // Act
            var result = (ObjectResult)controller.Delete(-2);

            // Assert - Response
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(200);

            // Assert - Database
            var sale = dbContext.Sales.FirstOrDefault(i => i.Id == -2);
            sale.ShouldBeNull();
        }

        [Fact]
        public void GetToursFromSale_Test()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();

            // Act
            var result = (ObjectResult)controller.GetToursFromSale(-2).Result;

            // Assert - Response
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(200);

            // Assert - Database
            //var sale = dbContext.Sales.FirstOrDefault(i => i.Id == -2);
            //sale.ShouldBeNull();
        }

        private static SaleController CreateController(IServiceScope scope)
        {
            return new SaleController(scope.ServiceProvider.GetRequiredService<ISaleService>())
            {
                ControllerContext = BuildContext("-21")
            };
        }

    }
}