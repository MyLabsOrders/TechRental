using RentDesktop.Models.Informing;
using RentDesktop.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentDesktop.ViewModels
{
    public class AdminWindowViewModel : ViewModelBase
    {
        public AdminWindowViewModel() : this(new UserInfo())
        {
        }

        public AdminWindowViewModel(IUserInfo userInfo)
        {
        }
    }
}
