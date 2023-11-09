namespace ModeloSimples.Infrastructure.DataAccess.Mappings;

public static class MappingConstant
{
    public static class ColumnType
    {
        public const string NVarCharMax = "nvarchar(max)";
    }

    public static class Auditoria
    {
        public const string Criado = "Criado";
        public const string Modificado = "Modificado";
        public const string Removido = "Removido";
        public const string Bloquado = "Bloquado";
    }

    public static class Versionamento
    {
        public const string Versao = "Versao";
        public const string Dados = "Dados";
    }
}
