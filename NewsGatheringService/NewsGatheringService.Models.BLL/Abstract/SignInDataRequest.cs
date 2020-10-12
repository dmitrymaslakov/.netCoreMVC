using System.ComponentModel.DataAnnotations;

namespace NewsGatheringService.Models.BLL.Abstract
{
    public abstract class SignInDataRequest
    {
        [Required(ErrorMessage = "Login not specified")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password not specified")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
