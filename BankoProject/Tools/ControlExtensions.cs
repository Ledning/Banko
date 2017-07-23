using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BankoProject.Tools
{
  class ControlExtensions
  {
    public static readonly DependencyProperty IsPickedProperty =
      DependencyProperty.RegisterAttached("IsPicked", typeof(bool), typeof(ControlExtensions),
        new PropertyMetadata(default(bool)));

    public static void SetIsPicked(UIElement element, bool value)
    {
      element.SetValue(IsPickedProperty, value);
    }

    public static bool GetIsPicked(UIElement element)
    {
      return (bool) element.GetValue(IsPickedProperty);
    }
  }
}