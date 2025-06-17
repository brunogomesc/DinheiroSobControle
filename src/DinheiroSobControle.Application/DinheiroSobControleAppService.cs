using System;
using System.Collections.Generic;
using System.Text;
using DinheiroSobControle.Localization;
using Volo.Abp.Application.Services;

namespace DinheiroSobControle;

/* Inherit your application services from this class.
 */
public abstract class DinheiroSobControleAppService : ApplicationService
{
    protected DinheiroSobControleAppService()
    {
        LocalizationResource = typeof(DinheiroSobControleResource);
    }
}
