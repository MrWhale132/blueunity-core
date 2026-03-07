using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace Theblueway.Core.Runtime
{
    public class JsonUtil
    {
        public static List<string> ReadObjects(string filePath, bool relative = true)
        {
            if (relative)
            {
                var basePath = Application.persistentDataPath;
                filePath = Path.Combine(basePath, filePath);
            }

            string jsonText = File.ReadAllText(filePath);

            var array = JArray.Parse(jsonText);

            var result = new List<string>();
            foreach (var item in array)
            {
                result.Add(item.ToString(Formatting.None));
            }

            return result;
        }


        public static void WriteObjects(string filePath, IEnumerable<string> objects, bool relative = true)
        {
            string jsonText = "[" + Environment.NewLine;
            jsonText += string.Join("," + Environment.NewLine, objects);
            jsonText += Environment.NewLine + "]";

            if (relative)
            {
                var basePath = Application.persistentDataPath;
                filePath = Path.Combine(basePath, filePath);
            }


            // Extract the directory from the full file path
            string directory = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(filePath, jsonText);
        }
    }
}
