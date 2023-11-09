namespace ModeloSimples.Infrastructure.DataAccess.Queries.Mappings;

public static class DapperConstant
{
    public static class PessoaModel
    {
        public const string Pessoa = "Pessoa";
        public const string PessoaId = "PessoaId";
        public const string Tipo = "Tipo";
    }

    public static class PessoaFisicaModel
    {
        public const string PessoaFisica = "PessoaFisica";
        public const string NomeSocial = "NomeSocial";
        public const string DataNascimento = "DataNascimento";
        public const string Genero = "Genero";
    }

    public static class PessoaJuridicaModel
    {
        public const string PessoaJuridica = "PessoaJuridica";
        public const string RazaoSocial = "RazaoSocial";
        public const string NomeFantasia = "NomeFantasia";
        public const string CNAE = "CNAE";
    }
}
