/*---------------------------------------------------------------
 * 作者：evesgf    创建时间：2017-2-17 15:44:05
 * 修改：evesgf    修改时间：2017-2-17 15:44:11
 *
 * 版本：V0.0.1
 * 
 * 描述：组件单例，挂到对象上即可实现对象单例
 ---------------------------------------------------------------*/

using System.Reflection;
using System;

namespace LarkFramework
{
    public class SingletonComponent<T> where T : class
    {
        protected static T mInstance = null;

        public static T Instance
        {

            get
            {
                if (mInstance == null)
                {
                    // 先获取所有非public的构造方法
                    ConstructorInfo[] ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                    // 从ctors中获取无参的构造方法
                    ConstructorInfo ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);
                    if (ctor == null)
                        throw new Exception("Non-public ctor() not found!");
                    // 调用构造方法
                    mInstance = ctor.Invoke(null) as T;
                }

                return mInstance;
            }
        }

        public static void Dispose()
        {
            mInstance = null;
        }
    }
}
