﻿using Com.Danliris.Service.Packing.Inventory.Application.CommonViewModelObjectProperties;
using Com.Danliris.Service.Packing.Inventory.Application.ToBeRefactored.CommonViewModelObjectProperties;
using Com.Danliris.Service.Packing.Inventory.Application.ToBeRefactored.GarmentShipping.GarmentPackingList;
using Com.Danliris.Service.Packing.Inventory.Application.ToBeRefactored.Utilities;
using Com.Danliris.Service.Packing.Inventory.Application.Utilities;
using Com.Danliris.Service.Packing.Inventory.Data.Models.Garmentshipping.GarmentShippingInvoice;
using Com.Danliris.Service.Packing.Inventory.Infrastructure.IdentityProvider;
using Com.Danliris.Service.Packing.Inventory.Infrastructure.Repositories.GarmentShipping.GarmentPackingList;
using Com.Danliris.Service.Packing.Inventory.Infrastructure.Repositories.GarmentShipping.GarmentShippingInvoice;
using Com.Danliris.Service.Packing.Inventory.Infrastructure.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Com.Danliris.Service.Packing.Inventory.Application.ToBeRefactored.GarmentShipping.GarmentShippingInvoice
{
	public class GarmentShippingInvoiceService : IGarmentShippingInvoiceService
	{
		protected const string IMG_DIR = "GarmentPackingList";
		private readonly IGarmentShippingInvoiceRepository _repository;
        private readonly IServiceProvider serviceProvider;
        private readonly IGarmentPackingListRepository plrepository;
		protected readonly IAzureImageService _azureImageService;
		private readonly IGarmentPackingListService _packingListService;
		protected readonly IIdentityProvider _identityProvider;

		public GarmentShippingInvoiceService(IServiceProvider serviceProvider)
		{
			_repository = serviceProvider.GetService<IGarmentShippingInvoiceRepository>();
            plrepository = serviceProvider.GetService<IGarmentPackingListRepository>();
			_azureImageService = serviceProvider.GetService<IAzureImageService>();
			_identityProvider = serviceProvider.GetService<IIdentityProvider>();
			_packingListService = serviceProvider.GetService<IGarmentPackingListService>();
			this.serviceProvider = serviceProvider;
        }
		private GarmentShippingInvoiceViewModel MapToViewModel(GarmentShippingInvoiceModel model)
		{
			var vm = new GarmentShippingInvoiceViewModel()
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
                PackingListId = model.PackingListId,
				InvoiceNo = model.InvoiceNo,
				InvoiceDate = model.InvoiceDate,
				From = model.From,
				To = model.To,
				BuyerAgent = new BuyerAgent
				{
					Id = model.BuyerAgentId,
					Code = model.BuyerAgentCode,
					Name = model.BuyerAgentName

				},
				Consignee = model.Consignee,
				LCNo = model.LCNo,
				IssuedBy = model.IssuedBy,
				Section = new Section
				{
					Id = model.SectionId,
					Code = model.SectionCode
				},
				ShippingPer = model.ShippingPer,
				SailingDate = model.SailingDate,
				ConfirmationOfOrderNo = model.ConfirmationOfOrderNo,
				ShippingStaffId = model.ShippingStaffId,
				ShippingStaff = model.ShippingStaff,
				FabricTypeId = model.FabricTypeId,
				FabricType = model.FabricType,
				BankAccountId = model.BankAccountId,
				BankAccount = model.BankAccount,
				PaymentDue = model.PaymentDue,
				PEBNo = model.PEBNo,
				PEBDate = model.PEBDate,
				NPENo = model.NPENo,
				NPEDate = model.NPEDate,
				BL = model.BL,
				BLDate = model.BLDate,
				CO = model.CO,
				CODate = model.CODate,
				COTP = model.COTP,
				COTPDate = model.COTPDate,
				Description = model.Description,
				Remark = model.Remark,
				AmountToBePaid = model.AmountToBePaid,
                AmountCA = model.AmountCA,
                CPrice = model.CPrice,
				Memo = model.Memo,
				TotalAmount = model.TotalAmount,
				IsUsed = model.IsUsed,
                ConsigneeAddress=model.ConsigneeAddress,
                DeliverTo=model.DeliverTo,
				GarmentShippingInvoiceAdjustments = model.GarmentShippingInvoiceAdjustment.Select(i=> new GarmentShippingInvoiceAdjustmentViewModel
				{
					AdjustmentDescription = i.AdjustmentDescription,
					AdjustmentValue = i.AdjustmentValue,
					Id = i.Id,
					GarmentShippingInvoiceId = i.GarmentShippingInvoiceId,
                    AdditionalChargesId = i.AdditionalChargesId
                }).ToList(),
				Items = model.Items.Select(i => new GarmentShippingInvoiceItemViewModel
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
					BuyerBrand = new BuyerBrand
					{
						Id=i.BuyerBrandId,
						Name= i.BuyerBrandName
					},
					Quantity = i.Quantity,
					Comodity= new Comodity
					{
						Id= i.ComodityId,
						Code = i.ComodityCode,
						Name = i.ComodityName
					},
					ComodityDesc = i.ComodityDesc,
					MarketingName = i.MarketingName,
					Desc2 =i.Desc2,
                    Desc3=i.Desc3,
                    Desc4=i.Desc4,
					Uom = new UnitOfMeasurement
					{
						Id= i.UomId,
						Unit = i.UomUnit
					},
					Price = i.Price,
					PriceRO = i.PriceRO,
					Amount = i.Amount,
					
					CurrencyCode = i.CurrencyCode,
					CMTPrice = i.CMTPrice,
					Unit = new Unit
					{
						Id = i.UnitId,
						Code = i.UnitCode
					},
					PackingListItemId = i.PackingListItemId
				}).ToList(),
				GarmentShippingInvoiceUnits= model.GarmentShippingInvoiceUnit.Select(i => new GarmentShippingInvoiceUnitViewModel
                {
                    Unit = new Unit
                    {
                        Id = i.UnitId,
                        Code = i.UnitCode
                    },
                    Id = i.Id,
                    GarmentShippingInvoiceId = i.GarmentShippingInvoiceId,
                    AmountPercentage=i.AmountPercentage,
                    QuantityPercentage=i.QuantityPercentage

                }).ToList(),

            };
			return vm;
		}
		private GarmentShippingInvoiceModel MapToModel(GarmentShippingInvoiceViewModel viewModel)
		{
			var items = (viewModel.Items ?? new List<GarmentShippingInvoiceItemViewModel>()).Select(i =>
			{
				
				i.BuyerBrand = i.BuyerBrand ?? new BuyerBrand();
				i.Uom = i.Uom ?? new UnitOfMeasurement();
				i.Unit = i.Unit ?? new Unit();
				i.Comodity = i.Comodity ?? new Comodity();
				return new GarmentShippingInvoiceItemModel( i.RONo, i.SCNo, i.BuyerBrand.Id, i.BuyerBrand.Name, i.Quantity, i.Comodity.Id, i.Comodity.Code, i.Comodity.Name, i.ComodityDesc, i.MarketingName, i.Desc2,i.Desc3, i.Desc4, i.Uom.Id.GetValueOrDefault(), i.Uom.Unit, i.Price, i.PriceRO, i.Amount, i.CurrencyCode, i.Unit.Id, i.Unit.Code, i.CMTPrice, i.PackingListItemId) {
					Id = i.Id
				};

			}).ToList();

			
			viewModel.Section = viewModel.Section ?? new Section();
			viewModel.BuyerAgent = viewModel.BuyerAgent ?? new BuyerAgent();
			var garmentshippinginvoiceadjustment = (viewModel.GarmentShippingInvoiceAdjustments ?? new List<GarmentShippingInvoiceAdjustmentViewModel>()).Select(m => new GarmentShippingInvoiceAdjustmentModel(m.GarmentShippingInvoiceId, m.AdjustmentDescription, m.AdjustmentValue, m.AdditionalChargesId) { Id = m.Id }).ToList();
            var garmentshippinginvoiceUnit = (viewModel.GarmentShippingInvoiceUnits ?? new List<GarmentShippingInvoiceUnitViewModel>()).Select(m => {

                m.Unit = m.Unit ?? new Unit();
                return new GarmentShippingInvoiceUnitModel(m.Unit.Id,m.Unit.Code, m.AmountPercentage,m.QuantityPercentage)
                {
                    Id = m.Id
                };
            }).ToList();

            GarmentShippingInvoiceModel garmentShippingInvoiceModel = new GarmentShippingInvoiceModel(viewModel.PackingListId,viewModel.InvoiceNo, viewModel.InvoiceDate,viewModel.From,viewModel.To,viewModel.BuyerAgent.Id,viewModel.BuyerAgent.Code,viewModel.BuyerAgent.Name,viewModel.Consignee,viewModel.LCNo,viewModel.IssuedBy,viewModel.Section.Id,viewModel.Section.Code,viewModel.ShippingPer,viewModel.SailingDate,viewModel.ConfirmationOfOrderNo,viewModel.ShippingStaffId,viewModel.ShippingStaff,viewModel.FabricTypeId,viewModel.FabricType,viewModel.BankAccountId,viewModel.BankAccount,viewModel.PaymentDue,viewModel.PEBNo,viewModel.PEBDate.GetValueOrDefault(),viewModel.NPENo,viewModel.NPEDate.GetValueOrDefault(),viewModel.Description,viewModel.Remark,items,viewModel.AmountToBePaid,viewModel.AmountCA,viewModel.CPrice,viewModel.Say,viewModel.Memo,viewModel.IsUsed,viewModel.BL,viewModel.BLDate.GetValueOrDefault(),viewModel.CO,viewModel.CODate.GetValueOrDefault(),viewModel.COTP,viewModel.COTPDate.GetValueOrDefault(), garmentshippinginvoiceadjustment,viewModel.TotalAmount,viewModel.ConsigneeAddress, viewModel.DeliverTo, garmentshippinginvoiceUnit);
			 
			return garmentShippingInvoiceModel;
		}
		public async Task<int> Create(GarmentShippingInvoiceViewModel viewModel)
		{
			GarmentShippingInvoiceModel model = MapToModel(viewModel);

			int Created = await _repository.InsertAsync(model);

			return Created;
		}

		public ListResult<GarmentShippingInvoiceViewModel> Read(int page, int size, string filter, string order, string keyword)
		{
			var query = _repository.ReadAll();
			List<string> SearchAttributes = new List<string>()
			{
				"InvoiceNo","From","To","BuyerAgentName" 
			};
			query = QueryHelper<GarmentShippingInvoiceModel>.Search(query, SearchAttributes, keyword);

			Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
			query = QueryHelper<GarmentShippingInvoiceModel>.Filter(query, FilterDictionary);

			Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
			query = QueryHelper<GarmentShippingInvoiceModel>.Order(query, OrderDictionary);

			var data = query
				.Skip((page - 1) * size)
				.Take(size)
				.Select(model => MapToViewModel(model))
				.ToList();

			return new ListResult<GarmentShippingInvoiceViewModel>(data, page, size, query.Count());
		}


		public async Task<GarmentShippingInvoiceViewModel> ReadById(int id)
		{
			var data = await _repository.ReadByIdAsync(id);

			var viewModel = MapToViewModel(data);

			return viewModel;
		}

		public ShippingPackingListViewModel ReadShippingPackingListById(int id)
		{
			var queryInv = _repository.ReadAll();
		 
			var query = (from a in queryInv
						where  a.PackingListId == id
						select new ShippingPackingListViewModel
						{
							 
							InvoiceId = a.Id
						}).FirstOrDefault();

			return query;
		}

		public async Task<int> Update(int id, GarmentShippingInvoiceViewModel viewModel)
		{
			GarmentShippingInvoiceModel garmentPackingListModel = MapToModel(viewModel);

			return await _repository.UpdateAsync(id, garmentPackingListModel);
		}

		public async Task<int> Delete(int id)
		{
			return await _repository.DeleteAsync(id);
		}

        public Buyer GetBuyer(int id)
        {
            string buyerUri = "master/garment-buyers";
            IHttpClientService httpClient = (IHttpClientService)serviceProvider.GetService(typeof(IHttpClientService));

            var response = httpClient.GetAsync($"{ApplicationSetting.CoreEndpoint}{buyerUri}/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
                Buyer viewModel = JsonConvert.DeserializeObject<Buyer>(result.GetValueOrDefault("data").ToString());
                return viewModel;
            }
            else
            {
                return null;
            }
        }

        public BankAccount GetBank(int id)
        {
            string bankUri = "master/account-banks";
            IHttpClientService httpClient = (IHttpClientService)serviceProvider.GetService(typeof(IHttpClientService));

            var response = httpClient.GetAsync($"{ApplicationSetting.CoreEndpoint}{bankUri}/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
                BankAccount viewModel = JsonConvert.DeserializeObject<BankAccount>(result.GetValueOrDefault("data").ToString());
                return viewModel;
            }
            else
            {
                return null;
            }
        }

		//
		public virtual async Task<MemoryStreamResult> ReadInvoiceExcelById(int id)
		{
			var datainv = await _repository.ReadByIdAsync(id);
			var datapl = await plrepository.ReadByIdAsync(datainv.PackingListId);

			var ExcelTemplate = new GarmentShippingInvoiceExcelTemplate(_identityProvider);

			var viewModel = MapToViewModel(datainv);
			var pl = _packingListService.MapToViewModel(datapl);

			Buyer buyer = GetBuyer(datainv.BuyerAgentId);
			BankAccount bank = GetBank(datainv.BankAccountId);

			pl.ShippingMarkImageFile = await _azureImageService.DownloadImage(IMG_DIR, pl.ShippingMarkImagePath);
			pl.SideMarkImageFile = await _azureImageService.DownloadImage(IMG_DIR, pl.SideMarkImagePath);
			pl.RemarkImageFile = await _azureImageService.DownloadImage(IMG_DIR, pl.RemarkImagePath);

			var stream = ExcelTemplate.GenerateExcelTemplate(viewModel, buyer, bank, pl, 7);

			return new MemoryStreamResult(stream, "Invoice " + datainv.InvoiceNo + ".xls");
		}

		public virtual async Task<MemoryStreamResult> ReadInvoiceWithHeaderExcelById(int id)
		{
			var datainv = await _repository.ReadByIdAsync(id);
			var datapl = await plrepository.ReadByIdAsync(datainv.PackingListId);

			var ExcelTemplate = new GarmentShippingInvoiceWithHeaderExcelTemplate(_identityProvider);

			var viewModel = MapToViewModel(datainv);
			var pl = _packingListService.MapToViewModel(datapl);

			Buyer buyer = GetBuyer(datainv.BuyerAgentId);
			BankAccount bank = GetBank(datainv.BankAccountId);

			pl.ShippingMarkImageFile = await _azureImageService.DownloadImage(IMG_DIR, pl.ShippingMarkImagePath);
			pl.SideMarkImageFile = await _azureImageService.DownloadImage(IMG_DIR, pl.SideMarkImagePath);
			pl.RemarkImageFile = await _azureImageService.DownloadImage(IMG_DIR, pl.RemarkImagePath);

			var stream = ExcelTemplate.GenerateExcelTemplate(viewModel, buyer, bank, pl, 7);

			return new MemoryStreamResult(stream, "Invoice " + datainv.InvoiceNo + ".xls");
		}

		public virtual async Task<MemoryStreamResult> ReadInvoiceCMTWithHeaderExcelById(int id)
		{
			var datainv = await _repository.ReadByIdAsync(id);
			var datapl = await plrepository.ReadByIdAsync(datainv.PackingListId);

			var ExcelTemplate = new GarmentShippingInvoiceCMTWithHeaderExcelTemplate(_identityProvider);

			var viewModel = MapToViewModel(datainv);
			var pl = _packingListService.MapToViewModel(datapl);

			Buyer buyer = GetBuyer(datainv.BuyerAgentId);
			BankAccount bank = GetBank(datainv.BankAccountId);

			pl.ShippingMarkImageFile = await _azureImageService.DownloadImage(IMG_DIR, pl.ShippingMarkImagePath);
			pl.SideMarkImageFile = await _azureImageService.DownloadImage(IMG_DIR, pl.SideMarkImagePath);
			pl.RemarkImageFile = await _azureImageService.DownloadImage(IMG_DIR, pl.RemarkImagePath);

			var stream = ExcelTemplate.GenerateExcelTemplate(viewModel, buyer, bank, pl, 7);

			return new MemoryStreamResult(stream, "Invoice CMT" + datainv.InvoiceNo + ".xls");
		}

		public virtual async Task<MemoryStreamResult> ReadInvoiceCMTExcelById(int id)
		{
			var datainv = await _repository.ReadByIdAsync(id);
			var datapl = await plrepository.ReadByIdAsync(datainv.PackingListId);

			var ExcelTemplate = new GarmentShippingInvoiceCMTExcelTemplate(_identityProvider);

			var viewModel = MapToViewModel(datainv);
			var pl = _packingListService.MapToViewModel(datapl);

			Buyer buyer = GetBuyer(datainv.BuyerAgentId);
			BankAccount bank = GetBank(datainv.BankAccountId);

			pl.ShippingMarkImageFile = await _azureImageService.DownloadImage(IMG_DIR, pl.ShippingMarkImagePath);
			pl.SideMarkImageFile = await _azureImageService.DownloadImage(IMG_DIR, pl.SideMarkImagePath);
			pl.RemarkImageFile = await _azureImageService.DownloadImage(IMG_DIR, pl.RemarkImagePath);

			var stream = ExcelTemplate.GenerateExcelTemplate(viewModel, buyer, bank, pl, 7);

			return new MemoryStreamResult(stream, "Invoice CMT " + datainv.InvoiceNo + ".xls");
		}

		public IQueryable<ShippingPackingListViewModel> ReadShippingPackingList(int month, int year)
        {
            var queryInv = _repository.ReadAll();
            var queryPL = plrepository.ReadAll();

            DateTime date = new DateTime(year, month, 1);
            var query = from a in queryInv
                        join b in queryPL
                        on a.PackingListId equals b.Id
                        where b.TruckingDate.AddHours(7).Date < date 
                        && b.Omzet==true
                        select new ShippingPackingListViewModel
                        {
                            BuyerAgentCode = a.BuyerAgentCode,
                            BuyerAgentName = a.BuyerAgentName,
                            Amount = a.TotalAmount,
                            InvoiceId = a.Id,
                            TruckingDate = b.TruckingDate,
                            PEBDate= a.PEBDate,
                            PaymentDue= a.PaymentDue,
                            InvoiceNo= a.InvoiceNo

                        };

             return query.AsQueryable();
        }
        public IQueryable<ShippingPackingListViewModel> ReadShippingPackingListNow(int month, int year)
        {
            var queryInv = _repository.ReadAll();
            var queryPL = plrepository.ReadAll();

            var query = from a in queryInv
                        join b in queryPL
                        on a.PackingListId equals b.Id
                        where b.TruckingDate.AddHours(7).Month == month && b.TruckingDate.AddHours(7).Year == year
                        && b.Omzet == true
                        select new ShippingPackingListViewModel
                        {
                            BuyerAgentCode = a.BuyerAgentCode,
                            BuyerAgentName = a.BuyerAgentName,
                            Amount = a.TotalAmount,
                            InvoiceId = a.Id,
                            InvoiceNo = a.InvoiceNo,
                            TruckingDate = b.TruckingDate,
                            PEBDate = a.PEBDate,
                            PaymentDue = a.PaymentDue
                        };

            return query.AsQueryable();
        }
        public IQueryable<ShippingPackingListViewModel> ReadShippingPackingListForDebtorCard(int month, int year, string buyer)
        {
            var queryInv = _repository.ReadAll();
            var queryPL = plrepository.ReadAll();
            DateTime date = new DateTime(year, month, 1);
            var query = from a in queryInv
                        join b in queryPL
                        on a.PackingListId equals b.Id
                        where b.TruckingDate.AddHours(7).Date < date && b.BuyerAgentCode == buyer
                        && b.Omzet == true
                        select new ShippingPackingListViewModel
                        {
                            BuyerAgentCode = a.BuyerAgentCode,
                            BuyerAgentName = a.BuyerAgentName,
                            Amount = a.TotalAmount,
                            InvoiceId = a.Id,
                            TruckingDate = b.TruckingDate,
                            Date = b.Date,
                            InvoiceNo = a.InvoiceNo
                        };

            return query.AsQueryable();

        }
        public IQueryable<ShippingPackingListViewModel> ReadShippingPackingListForDebtorCardNow(int month, int year, string buyer)
        {
            var queryInv = _repository.ReadAll();
            var queryPL = plrepository.ReadAll();

            var query = from a in queryInv
                        join b in queryPL
                        on a.PackingListId equals b.Id
                        where b.TruckingDate.AddHours(7).Date.Month == month && b.TruckingDate.AddHours(7).Date.Year == year && b.BuyerAgentCode == buyer
                        && b.Omzet == true
                        select new ShippingPackingListViewModel
                        {
                            BuyerAgentCode = a.BuyerAgentCode,
                            BuyerAgentName = a.BuyerAgentName,
                            Amount = a.TotalAmount,
                            InvoiceId = a.Id,
                            TruckingDate = b.TruckingDate,
                            Date = b.Date,
                            InvoiceNo = a.InvoiceNo
                        };

            return query.AsQueryable();
        }

        
    }
}

    

