using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankoProject.Tools.Events
{
  class CommunicationObject
  {
    /// <summary>
    /// Used for communicating between viewmodels.
    /// Remember to add Viewmodels if additional ones are added. See ApplicationWideEnums for more. 
    /// </summary>
    /// <param name="msgType"></param>
    /// <param name="dispatcher"></param>
    public CommunicationObject(ApplicationWideEnums.MessageTypes msgType, ApplicationWideEnums.SenderTypes dispatcher)
    {
      Message = msgType;
      DispatcherName = dispatcher;
      SessionName = "NOTUSED";
    }

    //used if it is a session load message, we need to know the name of the seesion
    //Check if the file exists will be done in WelcomeViewModel
    public CommunicationObject(ApplicationWideEnums.MessageTypes msgType, ApplicationWideEnums.SenderTypes dispatcher,
      string sessionName)
    {
      Message = msgType;
      DispatcherName = dispatcher;
      SessionName = sessionName;
    }


    public ApplicationWideEnums.MessageTypes Message { get; }

    public ApplicationWideEnums.SenderTypes DispatcherName { get; }

    public string SessionName { get; }
  }
}