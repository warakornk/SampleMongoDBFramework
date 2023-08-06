using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleMongoDBFramework.Models;
using SampleMongoDBFramework.Models.Entities;
using SampleMongoDBFramework.Repository.Interface;

namespace SampleMongoDBFramework.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AmphurController : ControllerBase
	{
		private readonly IDbRepository _repository;

		public AmphurController(IDbRepository repository)
		{
			_repository = repository;
		}

		[HttpGet("{provinceId}")]
		public async Task<IEnumerable<AmphurDto>> GetAsync(string provinceId)
		{
			var amphurs = (await _repository.GetAmphurs(provinceId)).Select(q => q.AsDto(provinceId));
			return amphurs;
		}

		[HttpGet("{provinceId}/{amphurId}")]
		public async Task<ActionResult<AmphurDto>> GetAmphurAsync(string provinceId, string amphurId)
		{
			var amphur = (await _repository.GetAmphur(provinceId, amphurId));

			if (amphur == null)
			{
				return NotFound();
			}
			return Ok(amphur.AsDto(provinceId));
		}

		[HttpPost]
		public async Task PostAsync(CreateAmphurDto createAmphurDto)
		{
			await _repository.CreateAmphur(createAmphurDto);
		}

		[HttpPut]
		public async Task PutAsync(string provinceId, string amphurId, UpdateAmphurDto updateAmphurDto)
		{
			await _repository.UpdateAmphur(provinceId, amphurId, updateAmphurDto);
		}

		[HttpDelete]
		public async Task DeleteAsync(string provinceId, string amphurId)
		{
			await _repository.DeleteAmphur(provinceId, amphurId);
		}
	}
}