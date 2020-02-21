using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatSignalR.Model
{
    public class RegisterModel
    {
        [StringLength(15,MinimumLength= 3, ErrorMessage = "Логин должен быть от 3 до 15 символов")]
        [Required(ErrorMessage ="Логин не должен быть пустым")]
        public string Login { get; set; }
        
        [Required(ErrorMessage ="Пароль не должен быть пустым")]
        [StringLength(15,MinimumLength =8, ErrorMessage ="Пароль должен быть от 8 до 15 символов")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage ="Пароли не совпадают")]
        public string RePassword { get; set; }
    }
}
