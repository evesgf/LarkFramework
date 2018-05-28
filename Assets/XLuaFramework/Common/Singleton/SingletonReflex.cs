/*---------------------------------------------------------------
 * 作者：evesgf    创建时间：2016-8-2 11:39:29
 * 修改：evesgf    修改时间：2016-8-2 11:39:33
 *
 * 版本：V0.0.2
 * 
 * 描述：带反射单例模板，支持显式调用private类型的构造函数
 ---------------------------------------------------------------*/

using UnityEngine;
using System.Reflection;
using System;

namespace LarkFramework
{
    public class SingletonReflex<T> where T : SingletonReflex<T>
    {
        private static T _instance = null;

        protected SingletonReflex()
        {
            return;
        }

        /// <summary>
        /// 实例化单例
        /// </summary>
        /// <returns></returns>
        public static T Create()
        {
            if (_instance == null)
            {
                //为了可以被继承,静态实例和构造方法都使用protect修饰符。以上的问题很显而易见,那就是不能new一个泛型(3月9日补充:并不是不能new一个泛型,参考:http://bbs.csdn.net/topics/390911693 ),(4月5日补充:有同学说可以new一个泛型的实例,不过要求改泛型提供了public的构造函数,好吧,这里不用new的原因是,无法显示调用private的构造函数)

                // 先获取所有非public的构造方法
                ConstructorInfo[] ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                // 从ctors中获取无参的构造方法
                ConstructorInfo ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);
                if (ctor == null)
                    throw new Exception("Non-public ctor() not found!");
                // 调用构造方法
                _instance = ctor.Invoke(null) as T;

                if (_instance == null)
                    throw new Exception("SingletonReflex Instance Defeated!!");
            }

            return _instance;
        }

        /// <summary>
        /// 获取单例
        /// 此处不提供自动创建，旨在明确单例创建时间
        /// </summary>
        public static T Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// 销毁单例
        /// </summary>
        public static void Destroy()
        {

            _instance = null;

            return;
        }
    }
}
