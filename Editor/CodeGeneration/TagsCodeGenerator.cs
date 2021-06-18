using System.Collections.Generic;
using UnityEditorInternal;

namespace RedOwl.Editor
{
    public class TagssCodeGenerator : ICodeGenerator
    {
        public string Name => "Tags";

        public string Generate(string @namespace)
        {
            var constants = new List<string>();
            var enums = new List<string>();
            int i = 0;
            foreach (string layer in InternalEditorUtility.tags)
            {
                if (string.IsNullOrEmpty(layer)) continue;
                var layerName = layer.Replace(" ", "");
                constants.Add($"public const int {layerName.ToUpper()} = {i};");
                enums.Add($"{layerName} = {i},");
                i++;
            }

            return $@"{CodeGenerator.AutoGenerated}
namespace {@namespace}
{{
    public class UnityTags
    {{
        {CodeGenerator.Join(constants, 2)}

        public enum Values
        {{
            {CodeGenerator.Join(enums, 3)}
        }}
    }}
}}
";
        }
    }
}