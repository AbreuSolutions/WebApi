namespace Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string DataCadastro { get; set; }
        public string DataNascimento { get; set; }
        public string Nome { get; set; }
        public string SobreNome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Perfil { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
    }
}
