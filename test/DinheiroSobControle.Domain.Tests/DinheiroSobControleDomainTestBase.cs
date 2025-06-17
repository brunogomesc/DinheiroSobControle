using Volo.Abp.Modularity;

namespace DinheiroSobControle;

/* Inherit from this class for your domain layer tests. */
public abstract class DinheiroSobControleDomainTestBase<TStartupModule> : DinheiroSobControleTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
