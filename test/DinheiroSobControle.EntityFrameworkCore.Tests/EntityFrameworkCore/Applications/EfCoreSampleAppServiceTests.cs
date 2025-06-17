using DinheiroSobControle.Samples;
using Xunit;

namespace DinheiroSobControle.EntityFrameworkCore.Applications;

[Collection(DinheiroSobControleTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<DinheiroSobControleEntityFrameworkCoreTestModule>
{

}
