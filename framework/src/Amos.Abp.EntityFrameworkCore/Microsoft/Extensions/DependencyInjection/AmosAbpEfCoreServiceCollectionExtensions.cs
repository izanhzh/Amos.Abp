using Amos.Abp.EntityFrameworkCore;
using Amos.Abp.EntityFrameworkCore.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;

namespace Amos.Abp.Microsoft.Extensions.DependencyInjection
{
    public static class AmosAbpEfCoreServiceCollectionExtensions
    {
        /// <summary>
        /// 在AddAbpDbContext的基础上，增加扩展支持自动注册自动添加实体的Repository
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="optionsBuilder"></param>
        /// <param name="registerTempTableRepository"></param>
        /// <param name="registerBacthBulkExecuter"></param>
        /// <returns></returns>
        public static IServiceCollection AddAbpDbContextEx<TDbContext>(
            this IServiceCollection services,
            Action<IAbpDbContextRegistrationOptionsBuilder>? optionsBuilder = null)
            where TDbContext : AmosAbpDbContext<TDbContext>
        {
            services.AddAbpDbContext<TDbContext>(optionsBuilder);//Abp默认实现，查找Entity时是通过搜索DbContxt DbSet类型的属性，这个代码必须在最前面

            RegisterAutoAddEntityRepository<TDbContext>(services, optionsBuilder);

            return services;
        }

        private static void RegisterAutoAddEntityRepository<TDbContext>(IServiceCollection services, Action<IAbpDbContextRegistrationOptionsBuilder> optionsBuilder) where TDbContext : AmosAbpDbContext<TDbContext>
        {
            #region 参考Abp的源码 https://github.com/abpframework/abp/blob/dev/framework/src/Volo.Abp.EntityFrameworkCore/Microsoft/Extensions/DependencyInjection/AbpEfCoreServiceCollectionExtensions.cs
            var options = new AbpDbContextRegistrationOptions(typeof(TDbContext), services);

            var replacedDbContextTypes = typeof(TDbContext).GetCustomAttributes<ReplaceDbContextAttribute>(true)
                .SelectMany(x => x.ReplacedDbContextTypes).ToList();

            foreach (var dbContextType in replacedDbContextTypes)
            {
                options.ReplaceDbContext(dbContextType);
            }

            optionsBuilder?.Invoke(options);
            #endregion

            new AutoAddEntityEfCoreRepositoryRegistrar(options).AddRepositories();
        }
    }
}
