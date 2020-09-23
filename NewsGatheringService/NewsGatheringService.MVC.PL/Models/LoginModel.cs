using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsGatheringService.MVC.PL.Models
{
    public class LoginModel
    {
        private string _password;

        [Required(ErrorMessage = "Не указан логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { 
            get
            {
                return _password;
            }
            set
            {
                var passwordToBytes = Encoding.UTF8.GetBytes(value);
                var enText = Convert.ToBase64String(passwordToBytes);

                /*var enTextBytes = Convert.FromBase64String(value);
                string deText = Encoding.UTF8.GetString(enTextBytes);*/
                _password = enText;
            }
        }
    }
}
