using System;

namespace Thebluway.Core
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class AllowEditAttribute : System.Attribute
    {
        public RandomIdEditMode RandomIdEditMode;
        public AllowEditAttribute()
        {
            
        }
        public AllowEditAttribute(RandomIdEditMode editMode)
        {
            RandomIdEditMode = editMode;
        }
    }

    [Flags]
    public enum RandomIdEditMode
    {
        None = 0,
        Generate = 1,
        Paste = 2,
    }
}
