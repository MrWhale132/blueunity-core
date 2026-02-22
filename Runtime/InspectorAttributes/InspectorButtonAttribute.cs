using System;
using UnityEngine;

namespace Theblueway.Core.Runtime
{

    [AttributeUsage(AttributeTargets.Field)]
    public sealed class InspectorButtonAttribute : PropertyAttribute
    {
        public readonly string MethodName;
        public readonly string Label;

        public InspectorButtonAttribute(string methodName, string label = null)
        {
            MethodName = methodName;
            Label = label;
        }
    }
}
