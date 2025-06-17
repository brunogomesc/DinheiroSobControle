using Microsoft.Extensions.Localization;
using DinheiroSobControle.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace DinheiroSobControle.Blazor;

[Dependency(ReplaceServices = true)]
public class DinheiroSobControleBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<DinheiroSobControleResource> _localizer;

    public DinheiroSobControleBrandingProvider(IStringLocalizer<DinheiroSobControleResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
