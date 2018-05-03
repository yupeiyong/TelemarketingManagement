using System.Collections.Generic;
using Models;
using Models.Enum;


namespace ViewModels
{

    public class UserEditViewModel
    {

        public User User { get; set; }

        public List<UserStateEnum> UserStates { get; set; }

        public List<Gender> Genders { get; set; }

    }

}