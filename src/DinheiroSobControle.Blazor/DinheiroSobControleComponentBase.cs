using DinheiroSobControle.Localization;
using Volo.Abp.AspNetCore.Components;

namespace DinheiroSobControle.Blazor;

public abstract class DinheiroSobControleComponentBase : AbpComponentBase
{
    protected DinheiroSobControleComponentBase()
    {
        LocalizationResource = typeof(DinheiroSobControleResource);
    }
}
