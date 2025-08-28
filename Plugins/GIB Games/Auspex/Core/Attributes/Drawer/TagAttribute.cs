using System;

namespace GIB.Auspex
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class TagAttribute : DrawerAttribute
    {
    }
}
