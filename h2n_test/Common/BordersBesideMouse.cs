using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace h2n_test.Common
{
    public static class BordersBesideMouse
    {
        #region Properties

        public static readonly DependencyProperty EnabledProperty =
            DependencyProperty.RegisterAttached("Enabled", typeof(bool), typeof(BordersBesideMouse), new PropertyMetadata(false, OnEnabledChanged));

        public static readonly DependencyProperty RootProperty =
            DependencyProperty.RegisterAttached("Root", typeof(UIElement), typeof(BordersBesideMouse), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty GradientPositionProperty =
            DependencyProperty.RegisterAttached("GradientPosition", typeof(Point), typeof(BordersBesideMouse), new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty IsMouseOverProperty =
            DependencyProperty.RegisterAttached("IsMouseOver", typeof(bool), typeof(BordersBesideMouse), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

        #endregion


        #region Methods

        private static void SetGradientPosition(DependencyObject obj, Point value)
        {
            obj.SetValue(GradientPositionProperty, value);
        }

        public static bool GetIsMouseOver(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsMouseOverProperty);
        }

        private static void SetIsMouseOver(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMouseOverProperty, value);
        }

        private static void SetRootObject(DependencyObject obj, UIElement value)
        {
            obj.SetValue(RootProperty, value);
        }

        public static void SetEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(EnabledProperty, value);
        }

        private static void OnEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newValue = (bool)e.NewValue;
            var oldValue = (bool)e.OldValue;
            if (!(d is UIElement element)) return;

            if (oldValue && !newValue)
            {
                element.MouseEnter -= MouseEnter;
                element.PreviewMouseMove -= PreviewMouseMove;
                element.MouseLeave -= MouseLeave;
                element.ClearValue(BordersBesideMouse.RootProperty);
            }

            if (oldValue || !newValue) return;
            element.MouseEnter += MouseEnter;
            element.PreviewMouseMove += PreviewMouseMove;
            element.MouseLeave += MouseLeave;

            SetRootObject(element, element);
        }

        private static void MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is UIElement element)
            {
                SetIsMouseOver(element, true);
            }
        }

        private static void PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!(sender is UIElement element) || !GetIsMouseOver(element)) return;
            var pos = e.GetPosition(element);
            SetGradientPosition(element, pos);
        }

        private static void MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is UIElement element)
            {
                SetIsMouseOver(element, false);
            }
        }

        #endregion
    }

    public class RadGradientBrushExtension : MarkupExtension
    {
        public Color Color { get; set; } = Colors.Black;
        public double Size { get; set; } = 50;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var target = ((IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget)))?.TargetObject as DependencyObject;

            var bgColor = Color.FromArgb(0, this.Color.R, this.Color.G, this.Color.B);
            var brush = new RadialGradientBrush(this.Color, bgColor)
            {
                MappingMode = BrushMappingMode.Absolute,
                RadiusX = this.Size,
                RadiusY = this.Size
            };
            var opacityBinding = new Binding("Opacity")
            {
                Source = target,
                Path = new PropertyPath(BordersBesideMouse.IsMouseOverProperty),
                Converter = new IsMouseOverToOpacityConverter()
            };

            var binding = new MultiBinding { Converter = new RelativePositionConverter() };
            binding.Bindings.Add(new Binding() { Source = target, Path = new PropertyPath(BordersBesideMouse.RootProperty) });
            binding.Bindings.Add(new Binding() { Source = target });
            binding.Bindings.Add(new Binding() { Source = target, Path = new PropertyPath(BordersBesideMouse.GradientPositionProperty) });

            BindingOperations.SetBinding(brush, RadialGradientBrush.CenterProperty, binding);
            BindingOperations.SetBinding(brush, RadialGradientBrush.GradientOriginProperty, binding);
            BindingOperations.SetBinding(brush, RadialGradientBrush.OpacityProperty, opacityBinding);

            return brush;
        }
    }

    public class RelativePositionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Any(o => o == DependencyProperty.UnsetValue || o == null)) return new Point(0, 0);

            var parent = values[0] as UIElement;
            var ctrl = values[1] as UIElement;
            var pointerPos = (Point)values[2];
            var relativePos = parent?.TranslatePoint(pointerPos, ctrl);

            return relativePos;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsMouseOverToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isEnter = value != null && (bool)value;
            return isEnter ? 0.7 : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
