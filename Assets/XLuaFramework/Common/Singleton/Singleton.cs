/*---------------------------------------------------------------
 * 作者：evesgf    创建时间：2016-8-2 10:58:04
 * 修改：evesgf    修改时间：2016-8-2 10:58:07
 *
 * 版本：V0.0.2
 * 
 * 描述：基础单例模板，不支持显示调用private的构造函数
 ---------------------------------------------------------------*/

using System;

namespace LarkFramework
{
    public class Singleton<T> where T : Singleton<T>
    {
        //实例化参数
        private static T _instance;

        //构造函数
        static Singleton()
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
                _instance = (T)Activator.CreateInstance(typeof(T), true);
                if (_instance == null)
                    throw new Exception("Singleton Instance Defeated!!");
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