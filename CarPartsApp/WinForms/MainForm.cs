using System;
using System.Windows.Forms;

namespace CarPartsApp
{
    public partial class MainForm : Form
    {
        private readonly CarPartsService _carPartsService;
        public MainForm(CarPartsService carPartsService)
        {
            InitializeComponent();
            _carPartsService = carPartsService;
            PopulateBrands();
        }

        private async void PopulateBrands()
        {
            comboBoxBrand.DataSource = await _carPartsService.GetBrandsAsync();
        }

        private async void comboBoxBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedBrand = comboBoxBrand.SelectedItem.ToString();
            comboBoxModel.DataSource = await _carPartsService.GetModelsAsync(selectedBrand);
        }

        private async void comboBoxModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedBrand = comboBoxBrand.SelectedItem.ToString();
            string selectedModel = comboBoxModel.SelectedItem.ToString();
            comboBoxYear.DataSource = await _carPartsService.GetProductionYearsAsync(selectedBrand, selectedModel);
        }

        private async void buttonSearch_Click(object sender, EventArgs e)
        {
            if (comboBoxBrand.SelectedItem == null || comboBoxModel.SelectedItem == null || comboBoxYear.SelectedItem == null)
            {
                MessageBox.Show("Please select a brand, model, and production year.");
                return;
            }

            string selectedBrand = comboBoxBrand.SelectedItem.ToString();
            string selectedModel = comboBoxModel.SelectedItem.ToString();
            int selectedYear = int.Parse(comboBoxYear.SelectedItem.ToString());

            var parts = await _carPartsService.GetPartsAsync(selectedBrand, selectedModel, selectedYear);

            dataGridViewResults.Rows.Clear();

            foreach (var part in parts)
            {
                dataGridViewResults.Rows.Add(part.PartName, part.Price);
            }

            if (parts.Count == 0)
            {
                MessageBox.Show("No parts found for the selected brand, model, and year.");
            }
        }

    }
}
