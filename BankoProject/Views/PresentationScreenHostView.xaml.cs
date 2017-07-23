using System.Windows;
using MahApps.Metro.Controls;

namespace BankoProject.Views
{
  /// <summary>
  /// Interaction logic for PresentationScreenHostView.xaml
  /// </summary>
  public partial class PresentationScreenHostView : Window
  {
    public PresentationScreenHostView()
    {
      InitializeComponent();
    }

    private void ActiveItem_TransitionCompleted(object sender, RoutedEventArgs e)
    {
    }

    private void ActiveItem_Unloaded(object sender, RoutedEventArgs e)
    {
    }
  }
}