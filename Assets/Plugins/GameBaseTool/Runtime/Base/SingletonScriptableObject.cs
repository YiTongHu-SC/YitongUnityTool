using UnityEngine;

namespace HuYitong.GameBaseTool.Base
{
    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<T>(typeof(T).Name);
                    if (_instance == null)
                    {
                        throw new System.Exception("Can not find any singleton scriptable object in the resource.");
                    }
                    // var assets = Resources.LoadAll<T>($"");
                    // if (assets == null || assets.Length == 0)
                    // {
                    //     throw new System.Exception("Can not find any singleton scriptable object in the resource.");
                    // }
                    // else if (assets.Length > 1)
                    // {
                    //     Debug.LogWarning("Multiple singleton scriptable objects found in resource. ");
                    // }
                    //
                    // _instance = assets[0];
                }
                return _instance;
            }
        }
        public abstract void Init();
    }
}