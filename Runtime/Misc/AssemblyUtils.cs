using System;
using System.Linq;
using System.Reflection;

public static class AssemblyUtils
{
    //public static bool IsUserAssembly(this Assembly asm)
    //{
    //    try
    //    {
    //        string loc = asm.Location;
    //        if (string.IsNullOrEmpty(loc))
    //            return false;
    //        UnityEngine.Debug.Log($"Assembly Location: {loc}");
    //        loc = loc.Replace('\\', '/');
    //        return loc.Contains("/Library/ScriptAssemblies/") && !loc.Contains("ScriptAssemblies/Unity");
    //    }
    //    catch
    //    {
    //        return false;
    //    }
    //}

    public static bool IsUserAssembly(Assembly asm)
    {
        // Assembly-CSharp always user code
        string name = asm.GetName().Name;
        if (name.StartsWith("Assembly-CSharp"))
            return true;

        // asmdef marker type heuristic
        string marker = name + "." + name + "AssemblyInfo";
        if (asm.GetType(marker) != null)
            return true;

        // metadata company name heuristic
        var attrs = asm.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
        if (attrs.Length == 0)
            return true;

        string company = ((AssemblyCompanyAttribute)attrs[0]).Company;
        if (string.IsNullOrEmpty(company))
            return true;

        if (company.Contains("Unity") || company.Contains("Microsoft"))
            return false;

        return true;
    }


    public static Assembly[] GetUserAssemblies(AppDomain appDomain)
    {
        return appDomain
            .GetAssemblies()
            .Where(IsUserAssembly)
            .ToArray();
    }
}




public static class AppDomainExtensions
{
    public static Assembly[] GetUserAssemblies(this AppDomain appDomain)
    {
        return appDomain
            .GetAssemblies()
            .Where(AssemblyUtils.IsUserAssembly)
            .ToArray();
    }
}