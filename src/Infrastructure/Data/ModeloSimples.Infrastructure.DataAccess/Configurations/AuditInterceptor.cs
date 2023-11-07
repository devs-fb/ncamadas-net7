//namespace ModeloSimples.Infrastructure.DataAccess.Configurations;

//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Diagnostics;
//using ModeloSimples.Domain.Aggregates;
//using ModeloSimples.Domain.ValueObjects;
//using Newtonsoft.Json;

//public class Version<T>
//{
//    public List<T> EntityVersions { get; set; } = new List<T>();
//}

//public class AuditInterceptor<T> : SaveChangesInterceptor where T : class
//{
//    private bool _changesApplied = false;

//    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
//    {
//        if (eventData.Context is null)
//            return ValueTask.FromResult(InterceptionResult<int>.SuppressWithResult(0));

//        if (_changesApplied)
//        {
//            // Se as alterações já foram aplicadas, retorne imediatamente sem fazer mais nada para evitar a recursão infinita
//            return base.SavingChangesAsync(eventData, result, cancellationToken);
//        }

//        var entries = eventData.Context.ChangeTracker.Entries<T>();

//        foreach (var entry in entries)
//        {
//            if (entry.State == EntityState.Modified)
//            {
//                var originalValues = entry.OriginalValues.ToObject();
//                var currentValues = entry.CurrentValues.ToObject();

//                var version = new Version<T>
//                {
//                    EntityVersions = { (T)originalValues }
//                };
//                version.EntityVersions.Add((T)currentValues);

//                string jsonVersion = JsonConvert.SerializeObject(version);

//                if (entry.Entity is Pessoa pessoa)
//                {
//                    pessoa.Versionamento.Dados = jsonVersion;

//                    eventData.Context.Entry(pessoa).State = EntityState.Modified;
//                }
//            }
//        }

//        var resultfinal = base.SavingChangesAsync(eventData, result, cancellationToken);

//        _changesApplied = true;
        

//        return resultfinal;
//    }
//}