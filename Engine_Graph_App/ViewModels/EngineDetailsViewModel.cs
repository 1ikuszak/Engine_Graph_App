using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;

namespace Engine_Graph_App.ViewModels
{
    public class EngineDetailsViewModel : ViewModelBase
    {
        // Fields
        private Grid _mainGrid;

        // Properties
        public ObservableCollection<MeasurementViewModel> SelectedCylindersMeasurements { get; set; }
        public string EngineName { get; set; }
        public int EngineId { get; set; } 
        public Grid MainGrid => _mainGrid ??= CreateGrid();

        // Constructor
        public EngineDetailsViewModel(MeasurementViewModel measurementViewModel, ObservableCollection<MeasurementViewModel> selectedCylindersMeasurements)
        {
            SelectedCylindersMeasurements = selectedCylindersMeasurements;
        }
        
        // Main grid creation
        public Grid CreateGrid()
        {
            var grid = new Grid();
            ClearGrid(grid);
            SetupRowsAndColumns(grid);
            AddCylinderNamesToGrid(grid);
            AddDatesAndMeasurementsToGrid(grid);
            return grid;
        }

        private void ClearGrid(Grid grid)
        {
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();
        }

        private void SetupRowsAndColumns(Grid grid)
        {
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            foreach (var _ in GetUniqueDates())
            {
                grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            }

            grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
            foreach (var _ in GetUniqueCylinderNames())
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
            }
        }

        // Grid population methods
        private void AddCylinderNamesToGrid(Grid grid)
        {
            int colIndex = 1;
            foreach (var name in GetUniqueCylinderNames())
            {
                var cylinderTextBlock = new Button { Content = name };
                Grid.SetColumn(cylinderTextBlock, colIndex++);
                Grid.SetRow(cylinderTextBlock, 0);
                grid.Children.Add(cylinderTextBlock);
            }
        }

        private void AddDatesAndMeasurementsToGrid(Grid grid)
        {
            int rowIndex = 1;
            foreach (var date in GetUniqueDates())
            {
                AddDateLabelToGrid(grid, date, rowIndex);
                
                int colIndex = 1;
                foreach (var name in GetUniqueCylinderNames())
                {
                    AddMeasurementCheckboxToGrid(grid, date, name, rowIndex, colIndex++);
                }

                rowIndex++;
            }
        }

        private void AddDateLabelToGrid(Grid grid, DateTime date, int rowIndex)
        {
            var dateLabel = new Button { Content = date.ToShortDateString() };
            Grid.SetColumn(dateLabel, 0);
            Grid.SetRow(dateLabel, rowIndex);
            grid.Children.Add(dateLabel);
        }

        private void AddMeasurementCheckboxToGrid(Grid grid, DateTime date, string name, int rowIndex, int colIndex)
        {
            var measurement = SelectedCylindersMeasurements.FirstOrDefault(m => m.Measurement.Cylinder.Name == name && m.Measurement.Date.Date == date);
            var checkbox = new CheckBox
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment =  Avalonia.Layout.VerticalAlignment.Center
            };
            Grid.SetColumn(checkbox, colIndex);
            Grid.SetRow(checkbox, rowIndex);
            grid.Children.Add(checkbox);
        }

        // Utility methods
        private List<string> GetUniqueCylinderNames()
        {
            return SelectedCylindersMeasurements
                .Select(m => m.Measurement.Cylinder.Name)
                .OrderBy(name => name)
                .ToList();
        }
        
        private List<DateTime> GetUniqueDates()
        {
            return SelectedCylindersMeasurements
                .Select(m => m.Measurement.Date.Date) 
                .Distinct()
                .OrderBy(date => date)
                .ToList();
        }
    }
}
