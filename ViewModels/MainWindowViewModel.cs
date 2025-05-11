using System;
using System.Windows.Input;
using Avalonia.Media;
using GraphicsAlgorithmsApp.Models;
using System.Collections.Generic;

namespace GraphicsAlgorithmsApp.ViewModels
{
    public class MainWindowViewModel
    {
        public int SelectedAlgorithmIndex { get; set; } = 0;
        public int SelectedColorIndex { get; set; } = 0;
        public bool ShowPreview { get; set; } = true;

        public int SelectedTransformationIndex { get; set; } = 0;
        public double TranslateX { get; set; } = 50;
        public double TranslateY { get; set; } = 50;
        public double ScaleX { get; set; } = 1.5;
        public double ScaleY { get; set; } = 1.5;
        public double RotationAngle { get; set; } = 45;
        public double ShearFactor { get; set; } = 0.5;

        public double UnitX { get; set; } = 0.0;
        public double UnitY { get; set; } = 0.0;

        public ICommand ClearCommand { get; }
        public ICommand ApplyTransformationCommand { get; }

        public Action ClearAction { get; set; }
        public Action ApplyTransformationAction { get; set; }

        public TransformationViewModel TransformationViewModel { get; set; }

        public bool IsCircleDrawing => SelectedAlgorithmIndex == 2 || SelectedAlgorithmIndex == 3;
        public bool IsTranslationSelected => SelectedTransformationIndex == 0;
        public bool IsScalingSelected => SelectedTransformationIndex == 1;
        public bool IsRotationSelected => SelectedTransformationIndex == 2;
        public bool IsShearingSelected => SelectedTransformationIndex == 6 || SelectedTransformationIndex == 7;

        private readonly Color[] _colors = new[] { Colors.Black, Colors.Red, Colors.Blue, Colors.Green };
        public Color SelectedColor => _colors[SelectedColorIndex];
        public Color TransformedColor => Colors.Purple;

        public MainWindowViewModel()
        {
            ClearCommand = new RelayCommand(_ => ClearAction?.Invoke());
            ApplyTransformationCommand = new RelayCommand(_ => ApplyTransformationAction?.Invoke());
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

        public void Execute(object parameter) => _execute(parameter);

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}