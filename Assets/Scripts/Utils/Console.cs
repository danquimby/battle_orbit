using UnityEngine;

public enum ConsoleType
{
    INFO = 0,
    WARN,
    DEBUG,
    CRITICAL
}

static class Console
{
    public static ConsoleType type = ConsoleType.INFO;
    public static void Init(ConsoleType type)
    {
        Console.type = type;
    }
    public static void info(object message)
    {
#if UNITY_EDITOR
        if (type >= ConsoleType.INFO)
            Debug.Log(message);
#endif
    }
    public static void warn(object message)
    {
#if UNITY_EDITOR
        if (type >= ConsoleType.WARN)
            Debug.LogWarning(message);
#endif
    }
    public static void error(object message)
    {
#if UNITY_EDITOR
        if (type >= ConsoleType.CRITICAL)
            Debug.LogError(message);
#endif
    }
    public static void debug(object message)
    {
#if UNITY_EDITOR
        if (type >= ConsoleType.DEBUG)
            Debug.Log(message);
#endif
    }

}
