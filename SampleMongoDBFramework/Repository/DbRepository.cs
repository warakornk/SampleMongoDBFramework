using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Linq;
using MongoFramework;
using MongoFramework.Linq;
using SampleMongoDBFramework.DataAccess;
using SampleMongoDBFramework.Models;
using SampleMongoDBFramework.Models.Entities;
using SampleMongoDBFramework.Repository.Interface;
using System.Collections.Generic;

namespace SampleMongoDBFramework.Repository
{
	public class DbRepository : IDbRepository
	{
		#region DbRepository

		private readonly DbContext _context;

		public DbRepository(DbContext context)
		{
			_context = context;
		}

		#endregion DbRepository

		#region Collection's funcsions

		#region Document

		public async Task<Document> GetDocumentByDocumentIdAsync(Guid documentId)
		{
			return await _context.Documents.FirstOrDefaultAsync(q => q.DocumentId == documentId);
		}

		public async Task<List<Document>> GetDocumentsAsync()
		{
			return await _context.Documents.ToListAsync();
		}

		public async Task CreateDocumentAsync(CreateDocumentDto createDocumentDto)
		{
			Document document = new Document()
			{
				DocumentId = Guid.NewGuid(),
				DocumentName = createDocumentDto.DocumentName,
				PageCount = createDocumentDto.PageCount,
				PublishDate = createDocumentDto.PublishDate,
				SysCreateDate = DateTimeOffset.Now,
				SysCreateIP = createDocumentDto.IP,
				SysCreateUser = createDocumentDto.CreateUserName,
				SysUpdateDate = null,
				SysUpdateIP = "",
				SysUpdateUser = ""
			};

			_context.Documents.Add(document);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateDocumentAsync(Guid documentId, UpdateDocumentDto updateDocumentDto)
		{
			Document document = await _context.Documents.FirstOrDefaultAsync(q => q.DocumentId == documentId);

			if (document != null)
			{
				//document.DocumentId = updateDocumentDto.documentId;
				document.DocumentName = updateDocumentDto.DocumentName;
				document.DocumentName = updateDocumentDto.DocumentName;
				document.PageCount = updateDocumentDto.PageCount;
				document.PublishDate = updateDocumentDto.PublishDate;
				document.SysUpdateDate = DateTimeOffset.Now;
				document.SysUpdateIP = updateDocumentDto.IP;
				document.SysUpdateUser = updateDocumentDto.UpdateUserName;

				_context.Documents.Update(document);
				_context.SaveChanges();
			}
		}

		public async Task DeleteDocumentAsync(Guid documentId)
		{
			Document document = await _context.Documents.FirstOrDefaultAsync(q => q.DocumentId == documentId);

			if (document != null)
			{
				_context.Documents.Remove(document);
				_context.SaveChanges();
			}
		}

		#endregion Document

		#region Province

		public async Task<List<Province>> GetProvincesAsync()
		{
			return await _context.Provinces.OrderBy(q => q.ProvinceName).ToListAsync();
		}

		public async Task<Province> GetProvinceAsync(string provinceId)
		{
			return await _context.Provinces.FirstOrDefaultAsync(q => q.ProvinceId == provinceId);
		}

		public async Task CreateProvinceAsync(CreateProvinceDto createProvinceDto)
		{
			// Add new province and all amphurs in province
			Province province = new Province()
			{
				ProvinceId = createProvinceDto.ProvinceId,
				ProvinceName = createProvinceDto.provinceName,
				Amphurs = createProvinceDto.Amphurs ?? new List<Amphur>()
			};

			_context.Provinces.Add(province);

			await _context.SaveChangesAsync();
		}

		public async Task UpdateProvinceAsync(string provinceId, UpdateProvinceDto updateProvinceDto)
		{
			Province province = await _context.Provinces.FirstOrDefaultAsync(q => q.ProvinceId == provinceId);

			// This function will update province data and replace all amphur.
			// Caution: sub document like amphur will change all by updateProvinceDto.Amphurs. If your condition will update only province data not amphur you will get old amphur list and add back (not use in updateProvinceDto);
			if (province != null)
			{
				province.ProvinceName = updateProvinceDto.provinceName;
				province.Amphurs = updateProvinceDto.Amphurs ?? new List<Amphur>(); ;

				_context.Provinces.Update(province);
				await _context.SaveChangesAsync();
			}
		}

		public async Task DeleteProvinceAsync(string provinceId)
		{
			Province province = await _context.Provinces.FirstOrDefaultAsync(q => q.ProvinceId == provinceId);

			if (province != null)
			{
				_context.Provinces.Remove(province);
				_context.SaveChanges();
			}
		}

		#endregion Province

		#region Amphur

		public async Task<List<Amphur>> GetAmphursAsync(string provinceId)
		{
			Province province = await _context.Provinces.FirstOrDefaultAsync(q => q.ProvinceId == provinceId);

			return province.Amphurs.OrderBy(q => q.AmphurName).ToList();
		}

		public async Task<Amphur?> GetAmphurAsync(string provinceId, string amphurId)
		{
			Province province = await _context.Provinces.FirstOrDefaultAsync(q => q.ProvinceId == provinceId);
			Amphur? amphur = province.Amphurs.FirstOrDefault(q => q.AmphurId == amphurId);

			return amphur;
		}

		public async Task CreateAmphurAsync(CreateAmphurDto createAmphurDto)
		{
			// Prepair new amhphur data
			Amphur amphur = new Amphur()
			{
				AmphurId = createAmphurDto.AmphurId,
				AmphurName = createAmphurDto.AmphurName
			};

			// Get province which this ammphur will add in
			Province province = await GetProvinceAsync(createAmphurDto.provinceId);

			// Check is not null
			if (province != null)
			{
				province.Amphurs.Add(amphur);

				// Convert to update province
				UpdateProvinceDto updateProvinceDto = new UpdateProvinceDto(province.ProvinceId, province.ProvinceName, province.Amphurs);

				// Call update province
				await UpdateProvinceAsync(createAmphurDto.provinceId, updateProvinceDto);
			}
		}

		public async Task UpdateAmphurAsync(string provinceId, string amphurId, UpdateAmphurDto updateAmphurDto)
		{
			Province province = await GetProvinceAsync(provinceId);

			// Loop to updat amphur in list
			// Caution: amphur document can change all data so you shoud add amphurId from function header for find reference in list and amphurId can change to difference value.
			foreach (Amphur amphur in province.Amphurs)
			{
				if (amphur.AmphurId == amphurId)
				{
					amphur.AmphurId = updateAmphurDto.AmphurId;
					amphur.AmphurName = updateAmphurDto.AmphurName;
				}
			}

			UpdateProvinceDto updateProvinceDto = new UpdateProvinceDto(province.ProvinceId, province.ProvinceName, province.Amphurs);

			await UpdateProvinceAsync(provinceId, updateProvinceDto);
		}

		public async Task DeleteAmphurAsync(string provinceId, string amphurId)
		{
			// Delete amphur can't delete from collection so make new collection and add old data except delete data.
			Province province = await GetProvinceAsync(provinceId);

			if (province != null)
			{
				// Create new amphur list without deleted amphur then add back to updateProvince
				List<Amphur> acceptAmphurs = province.Amphurs.Where(q => q.AmphurId != amphurId).ToList();
				UpdateProvinceDto updateProvinceDto = new UpdateProvinceDto(province.ProvinceId, province.ProvinceName, acceptAmphurs);

				await UpdateProvinceAsync(provinceId, updateProvinceDto);
			}
		}

		#endregion Amphur

		#endregion Collection's funcsions
	}
}