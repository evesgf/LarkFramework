using XLua;

[LuaCallCSharp]
public static class LuaHapler
{

    /// <summary>
    /// 资源管理器
    /// </summary>
    /// <returns></returns>
    public static ResManager GetResManager()
    {
        return ResManager.Instance;
    }
}