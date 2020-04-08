﻿using Com.Danliris.Service.Packing.Inventory.Application.CommonViewModelObjectProperties;
using Com.Danliris.Service.Packing.Inventory.Application.DyeingPrintingAreaMovement;
using Com.Danliris.Service.Packing.Inventory.Data.Models;
using Com.Danliris.Service.Packing.Inventory.Infrastructure.Repositories.DyeingPrintingAreaMovement;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Packing.Inventory.Test.Service
{
    public class DyeingPrintingAreaMovementServiceTest
    {
        public DyeingPrintingAreaMovementService GetService(IServiceProvider serviceProvider)
        {
            return new DyeingPrintingAreaMovementService(serviceProvider);
        }

        private DyeingPrintingAreaMovementModel Model
        {
            get
            {
                return new DyeingPrintingAreaMovementModel(ViewModel.Area, ViewModel.BonNo, ViewModel.Date, ViewModel.Shift, ViewModel.ProductionOrder.Id,
                    ViewModel.ProductionOrder.Code, ViewModel.ProductionOrder.No, ViewModel.ProductionOrderQuantity, ViewModel.ProductionOrder.Type,
                    ViewModel.Buyer, ViewModel.PackingInstruction, ViewModel.CartNo, ViewModel.Material.Id, ViewModel.Material.Code, ViewModel.Material.Name,
                    ViewModel.MaterialConstruction.Id, ViewModel.MaterialConstruction.Code, ViewModel.MaterialConstruction.Name, ViewModel.MaterialWidth,
                    ViewModel.Unit.Id, ViewModel.Unit.Code, ViewModel.Unit.Name, ViewModel.Color, ViewModel.Motif, ViewModel.Mutation, ViewModel.Length,
                    ViewModel.UOMUnit, ViewModel.Balance, ViewModel.Status, ViewModel.Grade, ViewModel.SourceArea);
            }
        }

        private DyeingPrintingAreaMovementViewModel ViewModel
        {
            get
            {
                return new DyeingPrintingAreaMovementViewModel()
                {
                    Id = 1,
                    Area = "area",
                    Date = DateTimeOffset.UtcNow,
                    Shift = "shift",
                    ProductionOrder = new ProductionOrder()
                    {
                        Id = 1,
                        No = "no"
                    },
                    CartNo = "no",
                    Material = new Material()
                    {
                        Id = 1,
                        Name = "name"
                    },
                    MaterialConstruction = new MaterialConstruction()
                    {
                        Id = 1,
                        Name = "name"
                    },
                    MaterialWidth = "width",
                    Unit = new Unit()
                    {
                        Id = 1,
                        Name = "name"
                    },

                    Color = "color",
                    Mutation = "mutation",
                    UOMUnit = "MTR"
                };
            }
        }

        public Mock<IServiceProvider> GetServiceProvider(IDyeingPrintingAreaMovementRepository service)
        {
            var spMock = new Mock<IServiceProvider>();
            spMock.Setup(s => s.GetService(typeof(IDyeingPrintingAreaMovementRepository)))
                .Returns(service);

            return spMock;
        }

        [Fact]
        public async Task Should_Success_Create()
        {
            var repoMock = new Mock<IDyeingPrintingAreaMovementRepository>();
            repoMock.Setup(s => s.InsertAsync(It.IsAny<DyeingPrintingAreaMovementModel>()))
                .ReturnsAsync(1);

            repoMock.Setup(s => s.ReadAllIgnoreQueryFilter()).Returns(new List<DyeingPrintingAreaMovementModel>() { new DyeingPrintingAreaMovementModel(){
                CreatedUtc = DateTime.UtcNow
            } }.AsQueryable());

            var service = GetService(GetServiceProvider(repoMock.Object).Object);

            var result = await service.Create(ViewModel);

            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task Should_Success_Delete()
        {
            var repoMock = new Mock<IDyeingPrintingAreaMovementRepository>();
            repoMock.Setup(s => s.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync(1);

            var service = GetService(GetServiceProvider(repoMock.Object).Object);

            var result = await service.Delete(1);

            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task Should_Success_ReadById()
        {
            var repoMock = new Mock<IDyeingPrintingAreaMovementRepository>();
            repoMock.Setup(s => s.ReadByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(Model);

            var service = GetService(GetServiceProvider(repoMock.Object).Object);

            var result = await service.ReadById(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Should_Null_ReadById()
        {
            var repoMock = new Mock<IDyeingPrintingAreaMovementRepository>();
            repoMock.Setup(s => s.ReadByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(default(DyeingPrintingAreaMovementModel));

            var service = GetService(GetServiceProvider(repoMock.Object).Object);

            var result = await service.ReadById(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task Should_Success_Update()
        {
            var repoMock = new Mock<IDyeingPrintingAreaMovementRepository>();
            repoMock.Setup(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<DyeingPrintingAreaMovementModel>()))
                .ReturnsAsync(1);


            var service = GetService(GetServiceProvider(repoMock.Object).Object);

            var result = await service.Update(1, ViewModel);

            Assert.NotEqual(0, result);
        }

        [Fact]
        public void Should_Success_Read()
        {
            var repoMock = new Mock<IDyeingPrintingAreaMovementRepository>();
            repoMock.Setup(s => s.ReadAll())
                .Returns(new List<DyeingPrintingAreaMovementModel>() { Model }.AsQueryable());

            var service = GetService(GetServiceProvider(repoMock.Object).Object);

            var result = service.Read(1, 25, "{}", "{}", null);

            Assert.NotEmpty(result.Data);
        }
    }
}
