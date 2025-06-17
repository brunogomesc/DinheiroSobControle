using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace DinheiroSobControle.Data;

/* This is used if database provider does't define
 * IDinheiroSobControleDbSchemaMigrator implementation.
 */
public class NullDinheiroSobControleDbSchemaMigrator : IDinheiroSobControleDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
