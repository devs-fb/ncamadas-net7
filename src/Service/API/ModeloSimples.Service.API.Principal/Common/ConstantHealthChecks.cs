namespace ModeloSimples.Service.API.Principal.Common
{
    public class ConstantHealthChecks
    {
        public const string HealthQuery = "SELECT 1;";
        public const string PrincipalContext = "principalcontext-db";
        public const string RabbitmqBroker = "rabbitmq-broker";
        public const string RedisCache = "redis-cache";
        public const string DataBase = "database";
        public const string Redis = "redis";
        public const string Cache = "cache";
        public const string SQLServer = "sqlserver";
        public const string SQL = "sql";

        public const string RabbitMq = "rabbitmq";
        public const string Broker = "broker";

        public const string Self = "self";
    }
}
