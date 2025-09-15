using System;
using System.IO;
using System.Reflection;
using KMod;
using UnityEngine.Diagnostics;


internal class KUtils
{
    public static string ModPath
    {
        get
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
    }


    public static string AssetsPath
    {
        get
        {
            return Path.Combine(KUtils.ModPath, "assets");
        }
    }


    public static string AssemblyVersion
    {
        get
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }

    public static string AssemblyName
    {
        get
        {
            return Assembly.GetExecutingAssembly().GetName().Name;
        }
    }

    public static bool IsModLoaded(string modID)
    {
        // 打印所有 Mod 的信息
        foreach (Mod mod in Global.Instance.modManager.mods)
        {
            LogUtil.Log($"Mod ID: {mod.staticID}, Active: {mod.IsActive()}");
        }

        // 检查指定 Mod 是否加载
        foreach (Mod mod in Global.Instance.modManager.mods)
        {
            if (mod.staticID == modID && mod.IsActive())
            {
                return true;
            }
        }
        return false;
    }





}

