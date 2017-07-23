using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankoProject.Models;
using Caliburn.Micro;

namespace BankoProject.ViewModels.ConfirmationBoxes
{
  class ConfirmChangeDialogViewModel : Screen
  {
    public ConfirmChangeDialogViewModel()
    {
      DisplayName = "Er du sikker?";
    }


    public void OK()
    {
      TryClose(true);
    }

    public void Cancel()
    {
      TryClose(false);
    }
  }
}