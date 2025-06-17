using DinheiroSobControle.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace DinheiroSobControle.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class DinheiroSobControleController : AbpControllerBase
{
    protected DinheiroSobControleController()
    {
        LocalizationResource = typeof(DinheiroSobControleResource);
    }
}
