﻿using Microsoft.AspNetCore.Mvc;
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

		public async Task<Document> GetDcoumentByDocumentId(Guid documentId)
		{
			return await _context.Documents.FirstOrDefaultAsync(q => q.DocumentId == documentId);
		}

		public async Task<List<Document>> GetDocuments()
		{
			return await _context.Documents.ToListAsync();
		}

		public async Task CreateDocument(CreateDocumentDto createDocumentDto)
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

		public async Task UpdateDocument(Guid documentId, UpdateDocumentDto updateDocumentDto)
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

		public async Task DeleteDocument(Guid documentId)
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

		public async Task<List<Province>> GetProvinces()
		{
			return await _context.Provinces.OrderBy(q => q.ProvinceName).ToListAsync();
		}

		public async Task<Province> GetProvince(string provinceId)
		{
			return await _context.Provinces.FirstOrDefaultAsync(q => q.ProvinceId == provinceId);
		}

		public async Task CreateProvince(CreateProvinceDto createProvinceDto)
		{
			Province province = new Province()
			{
				ProvinceId = createProvinceDto.ProvinceId,
				ProvinceName = createProvinceDto.provinceName,
				Amphurs = createProvinceDto.Amphurs
			};

			_context.Provinces.Add(province);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateProvince(string provinceId, UpdateProvinceDto updateProvinceDto)
		{
			Province province = await _context.Provinces.FirstOrDefaultAsync(q => q.ProvinceId == provinceId);

			if (province != null)
			{
				province.ProvinceName = updateProvinceDto.provinceName;
				province.Amphurs = updateProvinceDto.Amphurs;

				_context.Provinces.Update(province);
				await _context.SaveChangesAsync();
			}
		}

		public async Task DeleteProvince(string provinceId)
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

		public async Task<List<Amphur>> GetAmphurs(string provinceId)
		{
			Province province = await _context.Provinces.FirstOrDefaultAsync(q => q.ProvinceId == provinceId);

			return province.Amphurs.OrderBy(q => q.AmphurName).ToList();
		}

		public async Task<Amphur?> GetAmphur(string provinceId, string amphurId)
		{
			Province province = await _context.Provinces.FirstOrDefaultAsync(q => q.ProvinceId == provinceId);
			Amphur? amphur = province.Amphurs.FirstOrDefault(q => q.AmphurId == amphurId);

			return amphur;
		}

		public async Task CreateAmphur(CreateAmphurDto createAmphurDto)
		{
			// Prepair new amhphur data
			Amphur amphur = new Amphur()
			{
				AmphurId = createAmphurDto.AmphurId,
				AmphurName = createAmphurDto.AmphurName
			};

			// Get province which this ammphur will add in
			Province province = await GetProvince(createAmphurDto.provinceId);

			// Check is not null
			if (province != null)
			{
				province.Amphurs.Add(amphur);

				// Convert to update province
				UpdateProvinceDto updateProvinceDto = new UpdateProvinceDto(province.ProvinceId, province.ProvinceName, province.Amphurs);

				await UpdateProvince(createAmphurDto.provinceId, updateProvinceDto);
			}
		}

		public async Task UpdateAmphur(string provinceId, string amphurId, UpdateAmphurDto updateAmphurDto)
		{
			Province province = await GetProvince(provinceId);

			foreach (Amphur amphur in province.Amphurs)
			{
				if (amphur.AmphurId == amphurId)
				{
					amphur.AmphurId = updateAmphurDto.AmphurId;
					amphur.AmphurName = updateAmphurDto.AmphurName;
				}
			}

			UpdateProvinceDto updateProvinceDto = new UpdateProvinceDto(province.ProvinceId, province.ProvinceName, province.Amphurs);

			await UpdateProvince(provinceId, updateProvinceDto);
		}

		public async Task DeleteAmphur(string provinceId, string amphurId)
		{
			// Delete amphur can't delete from collection so make new collection and add old data except delete data.
			Province province = await GetProvince(provinceId);
			List<Amphur> acceptAmphurs = province.Amphurs.Where(q => q.AmphurId != amphurId).ToList();
			UpdateProvinceDto updateProvinceDto = new UpdateProvinceDto(province.ProvinceId, province.ProvinceName, acceptAmphurs);

			await UpdateProvince(provinceId, updateProvinceDto);
		}

		#endregion Amphur

		#endregion Collection's funcsions
	}
}