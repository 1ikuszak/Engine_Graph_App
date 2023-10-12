using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DynamicData;
using ReactiveUI;

namespace Engine_Graph_App.ViewModels
{
    public class TreeMenuViewModel : ViewModelBase
    {
        public ObservableCollection<object> TreeViewData { get; } = new ObservableCollection<object>();
        public ObservableCollection<ShipViewModel> Ships { get; } = new ObservableCollection<ShipViewModel>();
        public ObservableCollection<EngineViewModel> Engines { get; } = new ObservableCollection<EngineViewModel>();
        
        private ViewModelBase _contentToDisplay;
        public ViewModelBase ContentToDisplay
        {
            get => _contentToDisplay;
            set => this.RaiseAndSetIfChanged(ref _contentToDisplay, value);
        }
        
        public IEnumerable<TreeFilter> TreeFilterValues => Enum.GetValues(typeof(TreeFilter)).Cast<TreeFilter>();
        public enum TreeFilter
        {
            Ships,
            Engines
        }

        private TreeFilter _selectedFilter;
        public TreeFilter SelectedFilter
        {
            get => _selectedFilter;
            set => this.RaiseAndSetIfChanged(ref _selectedFilter, value);
        }

        public TreeMenuViewModel(IEnumerable<ShipViewModel> ships, IEnumerable<EngineViewModel> engines)
        {
            Ships = new ObservableCollection<ShipViewModel>(ships);
            Engines = new ObservableCollection<EngineViewModel>(engines);

            // Make the updating reactive. Every time SelectedFilter changes, we'll call UpdateTreeViewContent.
            this.WhenAnyValue(x => x.SelectedFilter)
                .Subscribe(_ => UpdateTreeViewContent());
        }

        private void UpdateTreeViewContent()
        {
            TreeViewData.Clear();

            switch (SelectedFilter)
            {
                case TreeFilter.Ships:
                    TreeViewData.AddRange(Ships);
                    break;
                case TreeFilter.Engines:
                    TreeViewData.AddRange(Engines);
                    break;
            }
        }
    }
}