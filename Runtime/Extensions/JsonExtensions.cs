using Newtonsoft.Json;

namespace Theblueway.Core.Runtime
{
    public static class JsonExtensions
    {
        public static float ReadSingle(this JsonReader reader)
        {
            return (float)reader.ReadAsDouble();
        }
    }
}
