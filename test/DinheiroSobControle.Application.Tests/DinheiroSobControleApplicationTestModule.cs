using Volo.Abp.Modularity;

namespace DinheiroSobControle;

[DependsOn(
    typeof(DinheiroSobControleApplicationModule),
    typeof(DinheiroSobControleDomainTestModule)
)]
public class DinheiroSobControleApplicationTestModule : AbpModule
{

}
