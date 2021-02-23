using System.ComponentModel.DataAnnotations;

namespace SecuringWebApiUsingJwtAuthentication.Requests
{
    public class LoginRequest
    {
        /// <summary>
        /// Nombre de usuario (alias).
        /// </summary>
        /// <example>aetxabao</example>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Password de usuario.
        /// </summary>
        /// <example>P4t4t4s!</example>
        [Required]
        public string Password { get; set; }
    }
}
