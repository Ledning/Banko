using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankoProject.Tools
{
  public class ApplicationWideEnums
  {
    /// <summary>
    /// Refers to the different messages this applicatin can pass around internally, each specifying some kind of action, discernible by the name of the enum member.
    /// </summary>
    public enum MessageTypes
    {
      ChngWelcomeView,
      ChngControlPanelView,
      ChngEventSelectedView,
      ChngMainWindowView,
      RbPrezScreen,
      Save,
      Load,
      GeneratePlates,
      CreateApplicationDirectories,
      FullscreenOverlay,
      BoardOverview,
      LatestNumbers,
      BingoHappened,
      ClosePrez,
      ScrnActivationTriggered,
      FullscreenOverlayBlank,
      Stopwatch
    }

    /// <summary>
    /// Refers to the different places (mainly views) that are allowed to send out the messagetypes specified in MessageTypes. 
    /// </summary>
    public enum SenderTypes
    {
      WelcomeView,
      ControlPanelView,
      EventSelectedView,
      MainWindowView, //this one is mainly the recieving place for all these requests, probably shouldnt be used for like design reasons
      DebuggingView,
    }

    public struct AspectRatio
    {
      int widthRatio;
      int heightRatio;
    }
  }
}