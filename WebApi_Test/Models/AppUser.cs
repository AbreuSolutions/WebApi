using Application;
using System;
using System.Security.Claims;

namespace WebApi_Test
{
    public class AppUser : ClaimsPrincipal
    {
        public AppUser(ClaimsPrincipal principal) : base(principal) { }
        public int Key { get { return Convert.ToInt32(Util.Descrypt(this.FindFirst("Key").Value)); } }
        public string Nome { get { return Util.Descrypt(this.FindFirst("Nome").Value); } }
        public string Email { get { return Util.Descrypt(this.FindFirst("Email").Value); } }
        public string Perfil { get { return Util.Descrypt(this.FindFirst("Perfil").Value); } }
        public string Status { get { return Util.Descrypt(this.FindFirst("Status").Value); } }
    }
}
