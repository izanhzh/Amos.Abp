using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Amos.Abp.SqlScript
{
    /// <summary>
    /// TODO: 可以考虑增加数据库支持，如果数据库没有，就从本地缓存读取，AddResourceAsync的时候，如果数据库没有就自动追加，异步处理，不需要等待保存完成，xml资源文件可以对每一个sql脚本增加版本号，版本有变化自动更新数据库的
    /// </summary>
    public class SqlScriptStore : ISqlScriptStore, ISingletonDependency
    {
        /// <summary>
        /// Sql脚本缓存
        /// Key: sqlScriptNamespace|databaseProvider; Value: sql脚本字符串字典（key: sqlScriptKey; value: sql脚本字符串）
        /// </summary>
        private static readonly ConcurrentDictionary<string, Dictionary<string, string>> _sqlScriptResourceCache = new ConcurrentDictionary<string, Dictionary<string, string>>();

        public async Task AddResourceAsync(string sqlScriptNamespace, Type sqlScriptResourceType)
        {
            var sqlScriptResourceNamePrefix = GetSqlScriptResourceNamePrefix(sqlScriptResourceType) + ".";
            var manifestResourceNames = sqlScriptResourceType.Assembly.GetManifestResourceNames().Where(w => w.StartsWith(sqlScriptResourceNamePrefix) && w.EndsWith(".xml"));
            foreach (var manifestResourceName in manifestResourceNames)
            {
                try
                {
                    var resourceNameDefinedDatabaseProvider = Path.GetFileNameWithoutExtension(manifestResourceName)[sqlScriptResourceNamePrefix.Length..];
                    using var stream = sqlScriptResourceType.Assembly.GetManifestResourceStream(manifestResourceName);
                    using var xmlReader = XmlReader.Create(stream);
                    var xmlSerializer = new XmlSerializer(typeof(SqlScriptResourceData));
                    var sqlScriptResource = (SqlScriptResourceData)xmlSerializer.Deserialize(xmlReader);

                    if (sqlScriptNamespace != sqlScriptResource.Namespace || resourceNameDefinedDatabaseProvider != sqlScriptResource.DatabaseProvider)
                    {
                        continue;
                    }

                    var cacheKey = $"{sqlScriptResource.Namespace}|{ sqlScriptResource.DatabaseProvider}";
                    var sqlScriptDict = sqlScriptResource.Items.ToDictionary(k => $"{sqlScriptResource.Namespace}:{k.Key}", v => v.Value);
                    _sqlScriptResourceCache.AddOrUpdate(cacheKey, sqlScriptDict, (k, v) => sqlScriptDict);
                }
                catch
                {
                    //吞掉不符合约定格式等情况出现未知异常
                }
            }
            await Task.CompletedTask;
        }

        public Task<string> GetSqlScriptAsync(string sqlScriptNamespace, string databaseProvider, string sqlScriptKey)
        {
            var cacheKey = $"{sqlScriptNamespace}|{databaseProvider}";
            if (!_sqlScriptResourceCache.ContainsKey(cacheKey))
            {
                throw new AbpException($"No found defined script resource(SqlScriptNamespace: {sqlScriptNamespace}; DatabaseProvider: {databaseProvider})");
            }
            if (!_sqlScriptResourceCache[cacheKey].ContainsKey(sqlScriptKey))
            {
                throw new AbpException($"No found defined script resource(SqlScriptNamespace: {sqlScriptNamespace}; DatabaseProvider: {databaseProvider}; SqlScriptKey: {sqlScriptKey})");
            }
            return Task.FromResult(_sqlScriptResourceCache[cacheKey][sqlScriptKey]);
        }

        private string GetSqlScriptResourceNamePrefix(Type sqlScriptResourceType)
        {
            //TODO：可以参照多语言资源类型那样，通过特性标记资源名称，然后这里获取到资源名称
            var suffix = "SqlScriptResource";
            if (sqlScriptResourceType.FullName.EndsWith(suffix))
            {
                return sqlScriptResourceType.FullName.Substring(0, sqlScriptResourceType.FullName.Length - suffix.Length);
            }
            else
            {
                return sqlScriptResourceType.FullName;
            }
        }
    }
}
