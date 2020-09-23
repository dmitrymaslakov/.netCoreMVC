using NewsGatheringService.Models.BLL.Abstract;
using System.ComponentModel.DataAnnotations;

namespace NewsGatheringService.Models.BLL
{
    public class RegisterRequest : SignInDataRequest
    {
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно")]
        public string ConfirmPassword { get; set; }
    }
}
