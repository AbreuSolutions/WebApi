using Domain.Common;

namespace Domain.Entities
{
    public class Produto
    {
        public int Id { get; set; }
        public string DataCadastro { get; set; }
        public string Nome { get; set; }
        public string CodigoDeBarras { get; set; }
        public string Descricao { get; set; }
        public string Preco { get; set; }
        public string CriadoPor { get; set; }
        public string UltimaModificacaoPor { get; set; }
        public string UltimaModificacao { get; set; }
        public string Status { get; set; }
    }
}
