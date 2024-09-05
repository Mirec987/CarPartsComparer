using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace CarPartsApp.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarPartsController : ControllerBase
    {
        private readonly CarPartsService _carPartsService;

        public CarPartsController(CarPartsService carPartsService)
        {
            _carPartsService = carPartsService;
        }

        [HttpGet("brands")]
        public async Task<IActionResult> GetBrands()
        {
            var brands = await _carPartsService.GetBrandsAsync();
            return Ok(brands);
        }

        [HttpGet("models/{brand}")]
        public async Task<IActionResult> GetModels(string brand)
        {
            var models = await _carPartsService.GetModelsAsync(brand);
            return Ok(models);
        }

        [HttpGet("years/{brand}/{model}")]
        public async Task<IActionResult> GetYears(string brand, string model)
        {
            var years = await _carPartsService.GetProductionYearsAsync(brand, model);
            return Ok(years);
        }

        [HttpGet("parts/{brand}/{model}/{year}")]
        public async Task<IActionResult> GetParts(string brand, string model, int year)
        {
            var parts = await _carPartsService.GetPartsAsync(brand, model, year);
            return Ok(parts);
        }
    }
}
