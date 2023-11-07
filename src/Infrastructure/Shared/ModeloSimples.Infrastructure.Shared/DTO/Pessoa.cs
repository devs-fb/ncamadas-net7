namespace ModeloSimples.Infrastructure.Shared.DTO;

using System.Text.Json.Serialization;

public class PessoaModel
{
    public Guid Id { get; set; }
    public string Tipo { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public PessoaFisicaModel PessoaFisica { get; set; } = null;
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public PessoaJuridicaModel PessoaJuridica { get; set; } = null;
}

public class PessoaFisicaModel 
{
    public string NomeSocial { get; set; }
    public DateTime DataNascimento { get; set; }
    public string Genero { get; set; }
}

public class PessoaJuridicaModel
{
    public string RazaoSocial { get; set; }
    public string NomeFantasia { get; set; }
    public string CNAE { get; set; }
}