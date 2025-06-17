using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DinheiroSobControle.Data;
using Volo.Abp.DependencyInjection;

namespace DinheiroSobControle.EntityFrameworkCore;

public class EntityFrameworkCoreDinheiroSobControleDbSchemaMigrator
    : IDinheiroSobControleDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreDinheiroSobControleDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the DinheiroSobControleDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<DinheiroSobControleDbContext>()
            .Database
            .MigrateAsync();
    }
}
