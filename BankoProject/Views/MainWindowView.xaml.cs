using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BankoProject.ViewModels;
using Catel;
using Catel.MVVM.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace BankoProject.Views
{
  /// <summary>
  /// Interaction logic for MainWindowView.xaml
  /// </summary>
  public partial class MainWindowView : MetroWindow
  {
    public MainWindowView()
    {
      InitializeComponent();
      this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
      this.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
      this.MinHeight = 720;
      this.MinWidth = 1280;
      this.Height = this.MaxHeight;
      this.Width = this.MaxWidth;
    }
  }
}