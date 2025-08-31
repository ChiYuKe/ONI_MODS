using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class LogUtil
{
    private static string Format(string message, string file, string member, int line)
    {
        string fileName = System.IO.Path.GetFileNameWithoutExtension(file);
        return $"[{fileName}.{member}:{line}] {message}";
    }

    public static void Log(string message,
        [CallerMemberName] string member = "",
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0)
    {
        Debug.Log(Format(message?.ToString(), file, member, line));
    }

    public static void Warning(string message,
        [CallerMemberName] string member = "",
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0)
    {
        Debug.LogWarning(Format(message?.ToString(), file, member, line));
    }

    public static void Error(string message, Exception ex = null,
        [CallerMemberName] string member = "",
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0)
    {
        string msg = Format(message?.ToString(), file, member, line);
        if (ex != null)
            msg += $"\n异常: {ex}";
        Debug.LogError(msg);
    }

    public static void Exception(Exception ex,
        string message = "",
        [CallerMemberName] string member = "",
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0)
    {
        string msg = Format(message?.ToString(), file, member, line);
        Debug.LogError(msg);
        Debug.LogException(ex);
    }
}
