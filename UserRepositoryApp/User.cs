using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRepositoryApp
{
    public class User : Entity
    {
        [StringLength(20, MinimumLength = 3,
            ErrorMessage = "Мин. длина логина должна быть 3, макс. 20.")]
        public string Login { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool IsAdmin { get; set; }
    }
}
