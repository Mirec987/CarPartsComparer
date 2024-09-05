using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class CarPartsService
{
    private BrandData data;

    public async Task LoadDataAsync(string jsonFilePath)
    {
        string json = await File.ReadAllTextAsync(jsonFilePath);
        data = JsonConvert.DeserializeObject<BrandData>(json);
    }

    public Task<List<string>> GetBrandsAsync()
    {
        return Task.FromResult(data.Brands.Select(b => b.BrandName).ToList());
    }

    public Task<List<string>> GetModelsAsync(string selectedBrand)
    {
        var brand = data.Brands.FirstOrDefault(b => b.BrandName == selectedBrand);
        return Task.FromResult(brand?.Models.Select(m => m.ModelName).ToList() ?? new List<string>());
    }

    public Task<List<int>> GetProductionYearsAsync(string selectedBrand, string selectedModel)
    {
        var model = data.Brands
                        .FirstOrDefault(b => b.BrandName == selectedBrand)?
                        .Models
                        .FirstOrDefault(m => m.ModelName == selectedModel);
        return Task.FromResult(model?.ProductionYears.Select(y => y.Year).ToList() ?? new List<int>());
    }

    public Task<List<CarPart>> GetPartsAsync(string selectedBrand, string selectedModel, int selectedYear)
    {
        var year = data.Brands
                       .FirstOrDefault(b => b.BrandName == selectedBrand)?
                       .Models
                       .FirstOrDefault(m => m.ModelName == selectedModel)?
                       .ProductionYears
                       .FirstOrDefault(y => y.Year == selectedYear);
        return Task.FromResult(year?.Parts ?? new List<CarPart>());
    }
}

public class BrandData
{
    public List<Brand> Brands { get; set; }
}

public class Brand
{
    public string BrandName { get; set; }
    public List<Model> Models { get; set; }
}

public class Model
{
    public string ModelName { get; set; }
    public List<ProductionYear> ProductionYears { get; set; }
}

public class ProductionYear
{
    public int Year { get; set; }
    public List<CarPart> Parts { get; set; }
}

public class CarPart
{
    public string PartName { get; set; }
    public double Price { get; set; }
}
