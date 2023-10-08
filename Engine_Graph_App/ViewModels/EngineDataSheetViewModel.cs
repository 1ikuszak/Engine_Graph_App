using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Engine_Graph_App.ViewModels
{
    public class EngineDataSheetViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private Grid _mainGrid;
       
        public ObservableCollection<MeasurementViewModel> SelectedCylindersMeasurements { get; set; }
        public string EngineName { get; set; }
        public int EngineId { get; set; } 
        public Grid MainGrid => _mainGrid ??= CreateGrid();
        public TableViewModel TableViewModel { get; }
        public LineGraphViewModel LineGraphViewModel { get; }
        public ScatterGraphViewModel ScatterGraphViewModel { get; }
        private bool _anyMeasurementChecked;
        public bool AnyMeasurementChecked
        {
            get => _anyMeasurementChecked;
            set
            {
                if (_anyMeasurementChecked != value)
                {
                    _anyMeasurementChecked = value;
                    OnPropertyChanged(nameof(AnyMeasurementChecked));
                }
            }
        }
        
        // Constructor
        public EngineDataSheetViewModel(
            MeasurementViewModel measurementViewModel, 
            ObservableCollection<MeasurementViewModel> selectedCylindersMeasurements, 
            TableViewModel tableViewModel,
            LineGraphViewModel lineGraphViewModel,
            ScatterGraphViewModel scatterGraphViewModel)
        {
            SelectedCylindersMeasurements = selectedCylindersMeasurements;
            TableViewModel = tableViewModel; // Use passed instance instead of creating new
            LineGraphViewModel = lineGraphViewModel;
            ScatterGraphViewModel = scatterGraphViewModel;
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
                var cylinderTextBlock = new Button
                {
                    Content = name,
                    HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    VerticalContentAlignment = Avalonia.Layout.VerticalAlignment.Center,
                    FontSize = 12,
                    Width = 22,
                    Padding = new Thickness(2, 3),
                };
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
            var dateLabel = new Button
            {
                Content = date.ToShortDateString(),
                HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalContentAlignment = Avalonia.Layout.VerticalAlignment.Center,
                FontSize = 12,
                Width = 86,
                Padding = new Thickness(3),
                FontFamily = new Avalonia.Media.FontFamily("Courier New"),
                FontWeight = FontWeight.Bold,
            };
            Grid.SetColumn(dateLabel, 0);
            Grid.SetRow(dateLabel, rowIndex);
            grid.Children.Add(dateLabel);
        }

        private void AddMeasurementCheckboxToGrid(Grid grid, DateTime date, string name, int rowIndex, int colIndex)
        {
            var measurement = SelectedCylindersMeasurements.FirstOrDefault(m => m.Measurement.Cylinder.Name == name && m.Measurement.Date.Date == date);
            if (measurement != null)
            {
                var checkbox = new CheckBox
                {
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    VerticalAlignment =  Avalonia.Layout.VerticalAlignment.Center,
                    Tag = new Tuple<string, DateTime>(name, date), // Tag to identify cylinder and date,
                    FontSize = 12,
                    Padding = new Thickness(0),
                    Margin = new Thickness(2, -6)  // Set margin to 0
                };
                checkbox.IsCheckedChanged += Checkbox_IsCheckedChanged; // Add event handler
                Grid.SetColumn(checkbox, colIndex);
                Grid.SetRow(checkbox, rowIndex);
                grid.Children.Add(checkbox);  
            }
        }
        
        private void Checkbox_IsCheckedChanged(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            var data = checkbox?.Tag as Tuple<string, DateTime>;
            if (data != null)
            {
                var cylinderName = data.Item1;
                var date = data.Item2;
        
                // Fetch the relevant data based on cylinderName and date
                var relevantMeasurement = SelectedCylindersMeasurements.FirstOrDefault(m => m.Measurement.Cylinder.Name == cylinderName && m.Measurement.Date.Date == date);
                if (relevantMeasurement != null)
                {
                    if (checkbox.IsChecked == true)
                    {
                        TableViewModel.AddMeasurement(relevantMeasurement);
                        LineGraphViewModel.AddMeasurement(relevantMeasurement); 
                        ScatterGraphViewModel.AddMeasurement(relevantMeasurement);
                    }
                    else
                    {
                        TableViewModel.RemoveMeasurement(relevantMeasurement);
                        LineGraphViewModel.RemoveMeasurement(relevantMeasurement); 
                        ScatterGraphViewModel.RemoveMeasurement(relevantMeasurement);
                    }
                }
            }
            
            // Check if any measurement is checked
            AnyMeasurementChecked = _mainGrid.Children.OfType<CheckBox>().Any(cb => cb.IsChecked == true);
        }

        // Utility methods
        private List<string> GetUniqueCylinderNames()
        {
            return SelectedCylindersMeasurements
                .Select(m => m.Measurement.Cylinder.Name)
                .Distinct()
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
        
        // used to expose AnyMeasurementChecked to acess it in MainWindow
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
