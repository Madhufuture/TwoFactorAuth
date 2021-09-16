using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwoFactorAuth.Models
{
    public class HomeViewModel
    {
        public string Email { get; set; }
        public string QrCodeUrl { get; set; }
        public string AuthCode { get; set; }
        public string SecretKey { get; set; }
    }
}
