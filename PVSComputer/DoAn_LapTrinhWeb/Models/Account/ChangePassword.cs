using System.ComponentModel.DataAnnotations;

namespace DoAn_LapTrinhWeb.Models
{
    public class ChangePassword
    {

        [Required(ErrorMessage = "Nhập mật khẩu cũ", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]))(?=.*[#$^+=!*()@%&]).{8,}$",
        ErrorMessage = "Mật khẩu mới tổi thiếu 8 ký tự bao gồm: chữ thường, chữ hoa, chữ số và 1 ký tự đặc biệt")]

        [Required(ErrorMessage = "Nhập mật khẩu mới", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Nhập mật khẩu confirm", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác thực không trùng nhau")]
        public string ConfirmPassword { get; set; }
    }
}