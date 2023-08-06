using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleMongoDBFramework.Models;
using SampleMongoDBFramework.Models.Entities;
using SampleMongoDBFramework.Repository.Interface;

namespace SampleMongoDBFramework.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProvinceController : ControllerBase
	{
		private readonly IDbRepository _repository;

		public ProvinceController(IDbRepository repository)
		{
			_repository = repository;
		}

		[HttpGet]
		public async Task<IEnumerable<ProvinceDto>> GetAsync()
		{
			var provinces = (await _repository.GetProvinces()).Select(q => q.AsDto());
			return provinces;
		}

		[HttpPost]
		public async Task PostAsync(CreateProvinceDto createProvinceDto)
		{
			await _repository.CreateProvince(createProvinceDto);
		}

		[HttpPut]
		public async Task PutAsync(string provinceId, UpdateProvinceDto updateProvinceDto)
		{
			await _repository.UpdateProvince(provinceId, updateProvinceDto);
		}

		[HttpDelete]
		public async Task DeleteAsync(string provinceId)
		{
			await _repository.DeleteProvince(provinceId);
		}
	}
}