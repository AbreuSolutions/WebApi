namespace Domain.Entities
{
    public class TB_Usuario_Login
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string DataCadastro { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Status { get; set; }
    }
}
