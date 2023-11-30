using System;
using System.Collections.Generic;
using Volo.Abp;

namespace Amos.Abp.SqlScript
{
    public class SqlScriptOptions
    {
        public Dictionary<string, Type> SqlScriptResources { get; }

        public SqlScriptOptions()
        {
            SqlScriptResources = new Dictionary<string, Type>();
        }

        public void AddResource(string sqlScriptNamespace, Type resourceType)
        {
            if (SqlScriptResources.ContainsKey(sqlScriptNamespace))
            {
                if (SqlScriptResources[sqlScriptNamespace] != resourceType)
                {
                    throw new AbpException($"不能添加重复的SqlScriptNamespace：{sqlScriptNamespace}");
                }
            }
            SqlScriptResources[sqlScriptNamespace] = resourceType;
        }
    }
}
