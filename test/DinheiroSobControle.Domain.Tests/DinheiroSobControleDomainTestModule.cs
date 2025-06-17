using Volo.Abp.Modularity;

namespace DinheiroSobControle;

[DependsOn(
    typeof(DinheiroSobControleDomainModule),
    typeof(DinheiroSobControleTestBaseModule)
)]
public class DinheiroSobControleDomainTestModule : AbpModule
{

}
