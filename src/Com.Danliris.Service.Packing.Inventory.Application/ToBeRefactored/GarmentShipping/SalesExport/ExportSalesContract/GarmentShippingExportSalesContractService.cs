﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Danliris.Service.Packing.Inventory.Application.CommonViewModelObjectProperties;
using Com.Danliris.Service.Packing.Inventory.Application.Utilities;
using Com.Danliris.Service.Packing.Inventory.Data.Models.Garmentshipping.SalesExport;
using Com.Danliris.Service.Packing.Inventory.Infrastructure.Repositories.GarmentShipping.SalesExport;
using Com.Danliris.Service.Packing.Inventory.Infrastructure.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Com.Danliris.Service.Packing.Inventory.Application.ToBeRefactored.GarmentShipping.SalesExport
{
    public class GarmentShippingExportSalesContractService : IGarmentShippingExportSalesContractService
    {
        private readonly IGarmentShippingExportSalesContractRepository _repository;
        private readonly IServiceProvider serviceProvider;

        public GarmentShippingExportSalesContractService(IServiceProvider serviceProvider)
        {
            _repository = serviceProvider.GetService<IGarmentShippingExportSalesContractRepository>();

            this.serviceProvider = serviceProvider;
        }

        private GarmentShippingExportSalesContractViewModel MapToViewModel(GarmentShippingExportSalesContractModel model)
        {
            GarmentShippingExportSalesContractViewModel viewModel = new GarmentShippingExportSalesContractViewModel
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

                salesContractNo = model.SalesContractNo,
                salesContractDate = model.SalesContractDate,
                transactionType = new TransactionType
                {
                    id = model.TransactionTypeId,
                    code = model.TransactionTypeCode,
                    name = model.TransactionTypeName
                },
                buyer = new Buyer
                {
                    Id = model.BuyerId,
                    Code = model.BuyerCode,
                    Name = model.BuyerName,
                    npwp = model.BuyerNPWP,
                    Address=model.BuyerAddress
                },
                isUseVat = model.IsUseVat,
                vat = new Vat
                {
                    id = model.VatId,
                    rate = model.VatRate,
                },
                isUsed = model.IsUsed,
                sellerAddress=model.SellerAddress,
                sellerName=model.SellerName,
                sellerNPWP=model.SellerNPWP,
                sellerPosition=model.SellerPosition,
                subTotal=model.SubTotal,
                items = (model.Items ?? new List<GarmentShippingExportSalesContractItemModel>()).Select(i => new GarmentShippingExportSalesContractItemViewModel
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

                    product = new ProductViewModel
                    {
                        id = i.ProductId,
                        code = i.ProductCode,
                        name = i.ProductName
                    },
                    quantity = i.Quantity,
                    uom = new UnitOfMeasurement
                    {
                        Id = i.UomId,
                        Unit = i.UomUnit
                    },
                    comodity = new Comodity
                    { 
                        Id = i.ComodityId,
                        Code = i.ComodityCode,
                        Name = i.ComodityName
                    },
                    remark = i.Remark,
                    price = i.Price,
                    remainingQuantity=i.RemainingQuantity
                }).ToList()
            };

            return viewModel;
        }

        private GarmentShippingExportSalesContractModel MapToModel(GarmentShippingExportSalesContractViewModel vm)
        {
            var items = (vm.items ?? new List<GarmentShippingExportSalesContractItemViewModel>()).Select(i =>
            {
                i.product = i.product ?? new ProductViewModel();
                i.uom = i.uom ?? new UnitOfMeasurement();
                i.comodity = i.comodity ?? new Comodity();
                return new GarmentShippingExportSalesContractItemModel(i.product.id, i.product.code, i.product.name, i.quantity,i.remainingQuantity, i.uom.Id.GetValueOrDefault(), i.uom.Unit, i.price, i.comodity.Id, i.comodity.Code, i.comodity.Name, i.remark) { Id = i.Id };
            }).ToList();

            vm.transactionType = vm.transactionType ?? new TransactionType();
            vm.buyer = vm.buyer ?? new Buyer();
            vm.vat = vm.vat ?? new Vat();
            return new GarmentShippingExportSalesContractModel(GenerateNo(vm), vm.salesContractDate.GetValueOrDefault(), vm.transactionType.id, vm.transactionType.code, vm.transactionType.name, vm.sellerName, vm.sellerPosition, vm.sellerAddress, vm.sellerNPWP, vm.buyer.Id, vm.buyer.Code, vm.buyer.Name, vm.buyer.Address, vm.buyer.npwp, vm.isUseVat, vm.vat.id, vm.vat.rate, vm.subTotal, vm.isUsed, items) { Id = vm.Id };
        }

        private string GenerateNo(GarmentShippingExportSalesContractViewModel vm)
        {
            var year = DateTime.Now.ToString("yy");
            var month = DateTime.Now.ToString("MM");

            var prefix = $"DL/GMT/{(vm.transactionType.code ?? "").Trim().ToUpper()}/{month}/{year}";

            var lastInvoiceNo = _repository.ReadAll().Where(w => w.SalesContractNo.StartsWith(prefix))
                .OrderByDescending(o => o.SalesContractNo)
                .Select(s => int.Parse(s.SalesContractNo.Replace(prefix, "")))
                .FirstOrDefault();
            var invoiceNo = $"{prefix}{(lastInvoiceNo + 1).ToString("D4")}";

            return invoiceNo;
        }
        public async Task<int> Create(GarmentShippingExportSalesContractViewModel viewModel)
        {
            var model = MapToModel(viewModel);

            int Created = await _repository.InsertAsync(model);

            return Created;
        }

        public async Task<int> Delete(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public ListResult<GarmentShippingExportSalesContractViewModel> Read(int page, int size, string filter, string order, string keyword)
        {
            var query = _repository.ReadAll();
            List<string> SearchAttributes = new List<string>()
            {
                "SalesContractNo", "BuyerCode", "BuyerName", "SellerName","TransactionTypeCode","TransactionTypeName"
            };
            query = QueryHelper<GarmentShippingExportSalesContractModel>.Search(query, SearchAttributes, keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<GarmentShippingExportSalesContractModel>.Filter(query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<GarmentShippingExportSalesContractModel>.Order(query, OrderDictionary);

            var data = query
                .Skip((page - 1) * size)
                .Take(size)
                .Select(model => MapToViewModel(model))
                .ToList();

            return new ListResult<GarmentShippingExportSalesContractViewModel>(data, page, size, query.Count());
        }

        public async Task<GarmentShippingExportSalesContractViewModel> ReadById(int id)
        {
            var data = await _repository.ReadByIdAsync(id);

            var viewModel = MapToViewModel(data);

            return viewModel;
        }

        public async Task<int> Update(int id, GarmentShippingExportSalesContractViewModel viewModel)
        {
            var model = MapToModel(viewModel);

            return await _repository.UpdateAsync(id, model);
        }
    }
}
