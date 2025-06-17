using DinheiroSobControle.Samples;
using Xunit;

namespace DinheiroSobControle.EntityFrameworkCore.Domains;

[Collection(DinheiroSobControleTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<DinheiroSobControleEntityFrameworkCoreTestModule>
{

}
