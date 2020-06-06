﻿using Com.Danliris.Service.Packing.Inventory.Application.CommonViewModelObjectProperties;
using Com.Danliris.Service.Packing.Inventory.Application.Utilities;
using Com.Danliris.Service.Packing.Inventory.Data.Models.Garmentshipping.GarmentPackingList;
using Com.Danliris.Service.Packing.Inventory.Infrastructure.Repositories.GarmentShipping.GarmentPackingList;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Packing.Inventory.Application.ToBeRefactored.GarmentShipping.GarmentPackingList
{
    public class GarmentPackingListService : IGarmentPackingListService
    {
        private readonly IGarmentPackingListRepository _packingListRepository;

        public GarmentPackingListService(IServiceProvider serviceProvider)
        {
            _packingListRepository = serviceProvider.GetService<IGarmentPackingListRepository>();
        }

        private GarmentPackingListViewModel MapToViewModel(GarmentPackingListModel model)
        {
            var vm =  new GarmentPackingListViewModel()
            {
                Active = model.Active,
                Id = model.Id,
                CreatedAgent = model.CreatedAgent,
                CreatedBy = model.CreatedBy,
                CreatedUtc = model.CreatedUtc,
                DeletedAgent = model.DeletedAgent,
                DeletedBy = model.DeletedBy,
                DeletedUtc = model.DeletedUtc,
                IsDeleted = model.IsDeleted,
                LastModifiedAgent = model.LastModifiedAgent,
                LastModifiedBy = model.LastModifiedBy,
                LastModifiedUtc = model.LastModifiedUtc,

                InvoiceNo = model.InvoiceNo,
                PackingListType = model.PackingListType,
                InvoiceType = model.InvoiceType,
                Section = new Section
                {
                    Id = model.SectionId,
                    Code = model.SectionCode
                },
                Date = model.Date,
                LCNo = model.LCNo,
                IssuedBy = model.IssuedBy,
                BuyerAgent = new Buyer
                {
                    Id = model.BuyerAgentId,
                    Code = model.BuyerAgentCode,
                    Name = model.BuyerAgentName,
                },
                Destination = model.Destination,
                TruckingDate = model.TruckingDate,
                ExportEstimationDate = model.ExportEstimationDate,
                Omzet = model.Omzet,
                Accounting = model.Accounting,
                Items = model.Items.Select(i => new GarmentPackingListItemViewModel
                {
                    Active = i.Active,
                    Id = i.Id,
                    CreatedAgent = i.CreatedAgent,
                    CreatedBy = i.CreatedBy,
                    CreatedUtc = i.CreatedUtc,
                    DeletedAgent = i.DeletedAgent,
                    DeletedBy = i.DeletedBy,
                    DeletedUtc = i.DeletedUtc,
                    IsDeleted = i.IsDeleted,
                    LastModifiedAgent = i.LastModifiedAgent,
                    LastModifiedBy = i.LastModifiedBy,
                    LastModifiedUtc = i.LastModifiedUtc,

                    RONo = i.RONo,
                    SCNo = i.SCNo,
                    BuyerBrand = new Buyer
                    {
                        Id = i.BuyerBrandId,
                        Name = i.BuyerBrandName
                    },
                    Comodity = new Comodity
                    {
                        Id = i.ComodityId,
                        Code = i.ComodityCode,
                        Name = i.ComodityName
                    },
                    ComodityDescription = i.ComodityDescription,
                    Quantity = i.Quantity,
                    Uom = new UnitOfMeasurement
                    {
                        Id = i.UomId,
                        Unit = i.UomUnit
                    },
                    PriceRO = i.PriceRO,
                    Price = i.Price,
                    Amount = i.Amount,
                    Valas = i.Valas,
                    Unit = new Unit
                    {
                        Id = i.UnitId,
                        Code = i.UnitCode
                    },
                    Article = i.Article,
                    OrderNo = i.OrderNo,
                    Description = i.Description,

                    Details = i.Details.Select(d => new GarmentPackingListDetailViewModel
                    {
                        Active = d.Active,
                        Id = d.Id,
                        CreatedAgent = d.CreatedAgent,
                        CreatedBy = d.CreatedBy,
                        CreatedUtc = d.CreatedUtc,
                        DeletedAgent = d.DeletedAgent,
                        DeletedBy = d.DeletedBy,
                        DeletedUtc = d.DeletedUtc,
                        IsDeleted = d.IsDeleted,
                        LastModifiedAgent = d.LastModifiedAgent,
                        LastModifiedBy = d.LastModifiedBy,
                        LastModifiedUtc = d.LastModifiedUtc,

                        Carton1 = d.Carton1,
                        Carton2 = d.Carton2,
                        Colour = d.Colour,
                        CartonQuantity = d.CartonQuantity,
                        QuantityPCS = d.QuantityPCS,
                        TotalQuantity = d.TotalQuantity,

                    }).ToList(),

                    AVG_GW = i.AVG_GW,
                    AVG_NW = i.AVG_NW,
                }).ToList(),

                GrossWeight = model.GrossWeight,
                NettWeight = model.NettWeight,
                TotalCartons = model.TotalCartons,
                Measurements = model.Measurements.Select(m => new GarmentPackingListMeasurementViewModel
                {
                    Active = m.Active,
                    Id = m.Id,
                    CreatedAgent = m.CreatedAgent,
                    CreatedBy = m.CreatedBy,
                    CreatedUtc = m.CreatedUtc,
                    DeletedAgent = m.DeletedAgent,
                    DeletedBy = m.DeletedBy,
                    DeletedUtc = m.DeletedUtc,
                    IsDeleted = m.IsDeleted,
                    LastModifiedAgent = m.LastModifiedAgent,
                    LastModifiedBy = m.LastModifiedBy,
                    LastModifiedUtc = m.LastModifiedUtc,

                    Length = m.Length,
                    Width = m.Width,
                    Height = m.Height,
                    CartonsQuantity = m.CartonsQuantity,

                }).ToList(),

                ShippingMark = model.ShippingMark,
                SideMark = model.SideMark,
                Remark = model.Remark,
            };
            return vm;
        }

        public async Task<int> Create(GarmentPackingListViewModel viewModel)
        {
            var items = (viewModel.Items ?? new List<GarmentPackingListItemViewModel>()).Select(i =>
            {
                var details = (i.Details ?? new List<GarmentPackingListDetailViewModel>()).Select(d =>
                {
                    var sizes = (d.Sizes ?? new List<GarmentPackingListDetailSizeViewModel>()).Select(s =>
                    {
                        s.Size = s.Size ?? new SizeViewModel();
                        return new GarmentPackingListDetailSizeModel(s.Size.Id, s.Size.Size, s.Quantity);
                    }).ToList();

                    return new GarmentPackingListDetailModel(d.Carton1, d.Carton2, d.Colour, d.CartonQuantity, d.QuantityPCS, d.TotalQuantity, sizes);
                }).ToList();

                i.BuyerBrand = i.BuyerBrand ?? new Buyer();
                i.Uom = i.Uom ?? new UnitOfMeasurement();
                i.Unit = i.Unit ?? new Unit();
                i.Comodity = i.Comodity ?? new Comodity();
                return new GarmentPackingListItemModel(i.RONo, i.SCNo, i.BuyerBrand.Id, i.BuyerBrand.Name, i.Comodity.Id, i.Comodity.Code, i.Comodity.Name, i.ComodityDescription, i.Quantity, i.Uom.Id.GetValueOrDefault(), i.Uom.Unit, i.PriceRO, i.Price, i.Amount, i.Valas, i.Unit.Id, i.Unit.Code, i.Article, i.OrderNo, i.Description, details, i.AVG_GW, i.AVG_NW);
            }).ToList();

            var measurements = (viewModel.Measurements ?? new List<GarmentPackingListMeasurementViewModel>()).Select(m => new GarmentPackingListMeasurementModel(m.Length, m.Width, m.Height, m.CartonsQuantity)).ToList();

            viewModel.Section = viewModel.Section ?? new Section();
            viewModel.BuyerAgent = viewModel.BuyerAgent ?? new Buyer();
            GarmentPackingListModel garmentPackingListModel = new GarmentPackingListModel("", viewModel.PackingListType, viewModel.InvoiceType, viewModel.Section.Id, viewModel.Section.Code, viewModel.Date.GetValueOrDefault(), viewModel.LCNo, viewModel.IssuedBy, viewModel.BuyerAgent.Id, viewModel.BuyerAgent.Code, viewModel.BuyerAgent.Name, viewModel.Destination, viewModel.TruckingDate.GetValueOrDefault(), viewModel.ExportEstimationDate.GetValueOrDefault(), viewModel.Omzet, viewModel.Accounting, items, viewModel.GrossWeight, viewModel.NettWeight, viewModel.TotalCartons, measurements, viewModel.ShippingMark, viewModel.SideMark, viewModel.Remark);

            int Created = await _packingListRepository.InsertAsync(garmentPackingListModel);

            return Created;
        }

        public ListResult<GarmentPackingListViewModel> Read(int page, int size, string filter, string order, string keyword)
        {
            var data = _packingListRepository.ReadAll()
                .Select(model => MapToViewModel(model))
                .ToList();

            return new ListResult<GarmentPackingListViewModel>(data, 1, 1, 1);
        }

        public async Task<GarmentPackingListViewModel> ReadById(int id)
        {
            var data = await _packingListRepository.ReadByIdAsync(id);

            var viewModel = MapToViewModel(data);

            return viewModel;
        }

        public Task<int> Update(int id, GarmentPackingListViewModel viewModel)
        {
            return Task.FromResult(1);
        }

        public Task<int> Delete(int id)
        {
            return Task.FromResult(1);
        }
    }
}
