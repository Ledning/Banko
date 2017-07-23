using MahApps.Metro;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BankoProject
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    protected override void OnStartup(StartupEventArgs e)
    {
      ThemeManager.AddAccent("orchestra",
        new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Clean/CleanWindow.xaml"));
      // get the current app style (theme and accent) from the application
      // you can then use the current theme and custom accent instead set a new theme
      Tuple<AppTheme, Accent> appStyle = ThemeManager.DetectAppStyle(Application.Current);

      // now set the Green accent and dark theme
      ThemeManager.ChangeAppStyle(Application.Current,
        ThemeManager.GetAccent("Yellow"),
        ThemeManager.GetAppTheme("BaseDark")); // or appStyle.Item1

      base.OnStartup(e);
    }
  }
}