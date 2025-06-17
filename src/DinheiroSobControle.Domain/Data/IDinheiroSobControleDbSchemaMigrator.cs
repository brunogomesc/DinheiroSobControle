using System.Threading.Tasks;

namespace DinheiroSobControle.Data;

public interface IDinheiroSobControleDbSchemaMigrator
{
    Task MigrateAsync();
}
