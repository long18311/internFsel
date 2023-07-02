using System.ComponentModel.DataAnnotations;

namespace ServerSignalData.ViewModel.User
{
    public class CreateAccUser
    {
        [Required(ErrorMessage = "Không được bỏ trống UserName")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống FullName")]
        public string FullName { get; set; }        
        [Required(ErrorMessage = "Không được bỏ trống Password")]
        public string Password { get; set; } 
    }
}
