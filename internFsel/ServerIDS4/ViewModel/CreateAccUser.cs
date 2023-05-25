using System.ComponentModel.DataAnnotations;

namespace ServerIDS4.ViewModel
{
    public class CreateAccUser
    {
        [Required(ErrorMessage = "Không được bỏ trống UserName")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống LastName")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống Email"), EmailAddress(ErrorMessage = "Hãy nhập đúng định dạng Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống Password")]
        public string Password { get; set; } 
        /*[Required]
        public string ConfirmPassword { get; set; } = null!;*/
    }
}
