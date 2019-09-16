using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Domain {
    public class User {
        public int UserId { get; set; }

        [MinLength(2)]
        [MaxLength(20)]
        [Required]
        public string UserName { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public Guid Guid { get; set; }

        [NotMapped]
        private int _passwordLength = 0;

        [NotMapped]
        [MinLength(4)]
        public string Password {
            get {
                var sb = new StringBuilder();
                for (int i = 0; i < _passwordLength; i++) sb.Append('*');
                return sb.ToString();
            }
            set {
                using (var sha256 = SHA256.Create()) {
                    PasswordHash = sha256.ComputeHash(Encoding.Unicode.GetBytes(value));
                }
                _passwordLength = value.Length;
            }
        }

        public bool CheckPassword(string password) {
            using (var sha256 = SHA256.Create()) {
                return Enumerable.SequenceEqual(PasswordHash, sha256.ComputeHash(Encoding.Unicode.GetBytes(password)));
            }
        }

    }
}
