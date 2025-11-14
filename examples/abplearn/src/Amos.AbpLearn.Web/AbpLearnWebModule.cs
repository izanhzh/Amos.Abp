using Amos.AbpLearn.EntityFrameworkCore;
using Amos.AbpLearn.Localization;
using Amos.AbpLearn.MultiTenancy;
using Amos.AbpLearn.OrderManagement;
using Amos.AbpLearn.OrderManagement.Web;
using Amos.AbpLearn.Permissions;
using Amos.AbpLearn.ProductManagement;
using Amos.AbpLearn.ProductManagement.Web;
using Amos.AbpLearn.Web.Menus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity.Web;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement.Web;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement.Web;
using Volo.Abp.UI.Navigation;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.Uow;
using Volo.Abp.VirtualFileSystem;

namespace Amos.AbpLearn.Web
{
    [DependsOn(
        typeof(AbpLearnHttpApiModule),
        typeof(AbpLearnApplicationModule),
        typeof(AbpLearnEntityFrameworkCoreModule),
        typeof(AbpAutofacModule),
        typeof(AbpIdentityWebModule),
        typeof(AbpSettingManagementWebModule),
        typeof(AbpAccountWebIdentityServerModule),
        typeof(AbpAspNetCoreMvcUiBasicThemeModule),
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
        typeof(AbpTenantManagementWebModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpSwashbuckleModule)
        )]
    [DependsOn(typeof(ProductManagementWebModule))]
    [DependsOn(typeof(OrderManagementWebModule))]
    public class AbpLearnWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(
                    typeof(AbpLearnResource),
                    typeof(AbpLearnDomainModule).Assembly,
                    typeof(AbpLearnDomainSharedModule).Assembly,
                    typeof(AbpLearnApplicationModule).Assembly,
                    typeof(AbpLearnApplicationContractsModule).Assembly,
                    typeof(AbpLearnWebModule).Assembly
                );
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            ConfigureTenant();
            ConfigureUrls(configuration);
            ConfigureDbConnectionOptions();
            UConfigureUnitOfWork();
            ConfigureBundles();
            ConfigureAuthentication(context, configuration);
            ConfigureAutoMapper();
            ConfigureVirtualFileSystem(hostingEnvironment);
            ConfigureLocalizationServices();
            ConfigureNavigationServices();
            ConfigureAutoApiControllers();
            ConfigureSwaggerServices(context.Services, configuration);
        }

        private void UConfigureUnitOfWork()
        {
            Configure<AbpUnitOfWorkDefaultOptions>(options =>
            {
                options.TransactionBehavior = UnitOfWorkTransactionBehavior.Enabled;
            });
        }

        private void ConfigureTenant()
        {
            Configure<AbpTenantResolveOptions>(options =>
            {
                options.AddDomainTenantResolver("{0}.localhost");//TODO：改为IConfiguration中配置
            });
        }

        private void ConfigureUrls(IConfiguration configuration)
        {
            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            });
        }

        private void ConfigureDbConnectionOptions()
        {
            //https://docs.abp.io/en/abp/latest/Connection-Strings

            //Configure<AbpDbConnectionOptions>(options =>
            //{
            //    options.ConnectionStrings.Default = "...";
            //    options.ConnectionStrings[ProductManagementDbProperties.ConnectionStringName] = "...";
            //});

            //默认情况下，DbContext使用DefaultConnectionStringResolver进行获取数据库连接字符串
            //如果没有AbpDbConnectionOptions配置，则默认DbContext的ConnectionStringName特性标记查找appsettings.json中的配置，如果找不到则回退查找名为Default的配置
            //如果有AbpDbConnectionOptions配置，则会优先按此配置的行为去获取，如果获取不到则会回退按上面那个规则去获取
            //多租户情况下，DbContext使用MultiTenantConnectionStringResolver 进行获取数据库连接字符串（还没仔细研究，目前猜测会先获取租户配置的连接字符串，如果没有就会回退按上面的逻辑获取，待验证确认）
            //Configure<AbpDbConnectionOptions>(options =>
            //{
            //    //Configuring the database structures, 通过此配置可以实现自由搭配将哪些模块合并使用同一个数据库
            //    options.Databases.Configure("SecondaryDb", db =>
            //    {
            //        db.MappedConnections.Add(ProductManagementDbProperties.ConnectionStringName);
            //    });
            //});
        }

        private void ConfigureBundles()
        {
            Configure<AbpBundlingOptions>(options =>
            {
                options.StyleBundles.Configure(
                    BasicThemeBundles.Styles.Global,
                    bundle =>
                    {
                        bundle.AddFiles("/global-styles.css");
                    }
                );
            });
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                    options.Audience = "AbpLearn";
                });
        }

        private void ConfigureAutoMapper()
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<AbpLearnWebModule>();
            });
        }

        private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
        {
            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<AbpLearnDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Amos.AbpLearn.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<AbpLearnDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Amos.AbpLearn.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<AbpLearnApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Amos.AbpLearn.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<AbpLearnApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Amos.AbpLearn.Application"));
                    options.FileSets.ReplaceEmbeddedByPhysical<AbpLearnWebModule>(hostingEnvironment.ContentRootPath);
                });
            }
        }

        private void ConfigureLocalizationServices()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("ar", "ar", "العربية"));
                options.Languages.Add(new LanguageInfo("cs", "cs", "Čeština"));
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("en-GB", "en-GB", "English (UK)"));
                options.Languages.Add(new LanguageInfo("hu", "hu", "Magyar"));
                options.Languages.Add(new LanguageInfo("fi", "fi", "Finnish"));
                options.Languages.Add(new LanguageInfo("fr", "fr", "Français"));
                options.Languages.Add(new LanguageInfo("hi", "hi", "Hindi"));
                options.Languages.Add(new LanguageInfo("it", "it", "Italian"));
                options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Português"));
                options.Languages.Add(new LanguageInfo("ru", "ru", "Русский"));
                options.Languages.Add(new LanguageInfo("sk", "sk", "Slovak"));
                options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
                options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文"));
                options.Languages.Add(new LanguageInfo("de-DE", "de-DE", "Deutsch"));
                options.Languages.Add(new LanguageInfo("es", "es", "Español"));
            });
        }

        private void ConfigureNavigationServices()
        {
            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new AbpLearnMenuContributor());
            });
        }

        private void ConfigureAutoApiControllers()
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(AbpLearnApplicationModule).Assembly);
                //options.ConventionalControllers.Create(typeof(ProductManagementApplicationModule).Assembly, options => options.RootPath = "product-management");
                options.ConventionalControllers.Create(typeof(ProductManagementApplicationModule).Assembly);
                options.ConventionalControllers.Create(typeof(OrderManagementApplicationModule).Assembly);
            });
        }

        private void ConfigureSwaggerServices(IServiceCollection services, IConfiguration configuration)
        {
            //services.AddAbpSwaggerGen(
            //    options =>
            //    {
            //        options.SwaggerDoc("v1", new OpenApiInfo { Title = "AbpLearn API", Version = "v1" });
            //        options.DocInclusionPredicate((docName, description) => true);
            //        options.CustomSchemaIds(type => type.FullName);
            //    }
            //);

            //services.AddAbpSwaggerGenWithOAuth(
            //    configuration["AuthServer:Authority"],
            //    new Dictionary<string, string>
            //    {
            //         {"AbpLearn", "AbpLearn API"}
            //    },
            //    options =>
            //    {
            //        options.SwaggerDoc("v1", new OpenApiInfo { Title = "AbpLearn API", Version = "v1" });
            //        options.DocInclusionPredicate((docName, description) => true);//这里可以实现分组
            //        options.CustomSchemaIds(type => type.FullName);
            //    });

            var authority = configuration["AuthServer:Authority"];
            var authorizationEndpoint = "/connect/authorize";
            var tokenEndpoint = "/connect/token";
            var authorizationUrl = new Uri($"{authority.TrimEnd('/')}{authorizationEndpoint.EnsureStartsWith('/')}");
            var tokenUrl = new Uri($"{authority.TrimEnd('/')}{tokenEndpoint.EnsureStartsWith('/')}");
            services.AddAbpSwaggerGen(
                options =>
                {
                    ////这里不对Swagger页面设置增加授权按钮，直接对Swagger的相关网页的终结点添加授权控制，这样Swagger调用接口的时候可以直接使用终结点授权时用户登录的信息
                    ////这种方式适合没有和其他后台管理功能页面结合的时候，Swagger是一个独立的部署网站时，可以使用这种方式，而这里因为会和其他功能混在一起，已经包含了登录等功能
                    ////如果给Swagger页面设置增加授权按钮，会导致和登录功能授权混乱，在登录功能中登录后，Swagger页面不需要登录也能操作
                    //options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    //{
                    //    Type = SecuritySchemeType.OAuth2,
                    //    Flows = new OpenApiOAuthFlows
                    //    {
                    //        Password = new OpenApiOAuthFlow
                    //        {
                    //            AuthorizationUrl = authorizationUrl,
                    //            Scopes = new Dictionary<string, string>() { { "AbpLearn", "AbpLearn API" } },
                    //            TokenUrl = tokenUrl
                    //        }
                    //    }
                    //});

                    //options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    //{
                    //    {
                    //        new OpenApiSecurityScheme
                    //        {
                    //            Reference = new OpenApiReference
                    //            {
                    //                Type = ReferenceType.SecurityScheme,
                    //                Id = "oauth2"
                    //            }
                    //        },
                    //        Array.Empty<string>()
                    //    }
                    //});

                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "AbpLearn API", Version = "v1" });
                    options.SwaggerDoc("group1", new OpenApiInfo { Title = "AbpLearn API Group1", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                }
            );
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAbpRequestLocalization();

            if (!env.IsDevelopment())
            {
                app.UseErrorPage();
            }

            app.UseCorrelationId();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseJwtTokenMiddleware();

            if (MultiTenancyConsts.IsEnabled)
            {
                app.UseMultiTenancy();
            }

            app.UseUnitOfWork();
            app.UseIdentityServer();
            app.UseAuthorization();

            #region Swagger
            app.UseSwagger();
            app.UseAbpSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "AbpLearn API");

                //options.OAuthClientId("AbpLearn_Swagger");
                //options.OAuthClientSecret("1q2w3e*");
            });
            var routePrefix = "myswagger";
            app.UseSwagger(options =>
            {
                options.RouteTemplate = routePrefix + "/swagger/{documentName}/swagger.json";
            });
            app.UseAbpSwaggerUI(options =>
            {
                options.RoutePrefix = routePrefix;
                options.SwaggerEndpoint("/myswagger/swagger/group1/swagger.json", "Support APP API");
                options.InjectJavascript("/swagger/ui/abp.js");//默认情况下，UseAbpSwaggerUI会调用options.InjectJavascript("ui/abp.js")，会将ui/abp.js注入到swagger文件夹，这里再次调用，将之前注入在swagger文件夹的复制给RoutePrefix文件夹下
                options.InjectJavascript("/swagger/ui/abp.swagger.js");

                //options.OAuthClientId("AbpLearn_Swagger");
                //options.OAuthClientSecret("1q2w3e*");
            });
            #endregion

            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();

            app.UseEndpoints(endpoints =>
            {
                //对swagger相关终结点进行授权控制（注意：采用这种方式时，不要在services.AddAbpSwaggerGen的时候设置添加授权按钮）
                var pipeline = endpoints.CreateApplicationBuilder().Build();
                var swaggerAuthAttr = new AuthorizeAttribute(AbpLearnPermissions.AbpLearnApiSwagger);
                endpoints.Map("/swagger/{documentName}/swagger.json", pipeline).RequireAuthorization(swaggerAuthAttr);
                endpoints.Map("/swagger/index.html", pipeline).RequireAuthorization(swaggerAuthAttr);
                endpoints.Map("/myswagger/{documentName}/swagger.json", pipeline).RequireAuthorization(swaggerAuthAttr);
                endpoints.Map("/myswagger/index.html", pipeline).RequireAuthorization(swaggerAuthAttr);
            });
        }
    }
}
