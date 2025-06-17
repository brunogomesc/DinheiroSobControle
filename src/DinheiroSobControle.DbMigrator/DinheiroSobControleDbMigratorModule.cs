using DinheiroSobControle.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace DinheiroSobControle.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(DinheiroSobControleEntityFrameworkCoreModule),
    typeof(DinheiroSobControleApplicationContractsModule)
    )]
public class DinheiroSobControleDbMigratorModule : AbpModule
{
}
