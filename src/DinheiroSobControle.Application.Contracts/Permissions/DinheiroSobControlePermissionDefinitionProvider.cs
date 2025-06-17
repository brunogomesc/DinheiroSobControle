using DinheiroSobControle.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace DinheiroSobControle.Permissions;

public class DinheiroSobControlePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(DinheiroSobControlePermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(DinheiroSobControlePermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<DinheiroSobControleResource>(name);
    }
}
