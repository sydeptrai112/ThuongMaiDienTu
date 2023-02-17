﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using DoAn_LapTrinhWeb.Model;

namespace DoAn_LapTrinhWeb.Models
{
    [Table("Account")]
    public class Account
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Account()
        {
            Feedbacks = new HashSet<Feedback>();
            Orders = new HashSet<Order>();
            News = new HashSet<News>();
            NewsComments = new HashSet<NewsComments>();
        }
        //Account ID
        [Key] public int account_id { get; set; }
        //Password
        [Required(ErrorMessage = "Nhập mật khẩu")]
        [StringLength(100)]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]))(?=.*[#$^+=!*()@%&]).{8,}$", 
        ErrorMessage = "Mật khẩu tổi thiếu 8 ký tự bao gồm: chữ thường, chữ hoa, chữ số và 1 ký tự đặc biệt")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        //Email
        [Required(ErrorMessage = "Nhập Email")]
        [StringLength(100, ErrorMessage = "Email tối thiểu 6 ký tự", MinimumLength = 1)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Vui lòng nhập đúng định dạng email")]
        public string Email { get; set; }
        //Request Code
        [StringLength(100)]
        public string Requestcode { get; set; }
        //Roles
        [Required(ErrorMessage = "Chọn quyền")]
        [StringLength(1)] public string Role { get; set; }
        //Name
        [Required(ErrorMessage = "Nhập họ tên")]
        [StringLength(20, ErrorMessage = "Họ tên tối đa 20 ký tự",MinimumLength = 1)]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        //Phone Number
        [Required(ErrorMessage = "Nhập số điện thoại")]
        [StringLength(14, ErrorMessage = "Số điện thoại phải đủ 10 chữ số", MinimumLength = 14)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        //Avatar
        [StringLength(500, ErrorMessage = "Ảnh đại diện không được quá 500 ký tự")]
        public string Avatar { get; set; }
        //Address
        [Required(ErrorMessage = "Nhập địa chỉ")]
        [StringLength(100, ErrorMessage = "Địa chỉ không được quá 100 ký tự", MinimumLength = 1)]
        public string Addres_1 { get; set; }
        //Date Of Birth
        [Required(ErrorMessage = "Nhập Ngày sinh")]
        [DisplayFormat(ApplyFormatInEditMode = true,DataFormatString = "{0:MM-dd-yyyy}")]
        [DataType(DataType.Date)]
        public DateTime Dateofbirth { get; set; }
        //Gender
        [StringLength(1)]

        [Required(ErrorMessage = "Chọn giới tính")]
        public string Gender{ get; set; }
        //Create By
        [StringLength(100)]
        public string create_by { get; set; }
        //Create At
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime create_at { get; set; }
        //Upadte By
        [StringLength(100)]
        public string update_by { get; set; }
        //Update At
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime update_at { get; set; }
        //Status
        [StringLength(1)] public string status { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<NewsComments> NewsComments { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }
    }
}