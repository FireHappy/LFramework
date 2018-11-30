using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assets.LFramework.Utility;

namespace Assets.LFramework.Network.Data.Reader
{
    public class NetInfoReaderBase  
    {
         
        protected NetInfoReaderBase(Dictionary<string, object> dict)
        {

        }

        protected NetInfoReaderBase(List<object> list)
        {

        }

        protected NetInfoReaderBase(List<object> list, Action<Dictionary<string, string>> action)
        {

        }

        protected void AutoSetValue<T>(Dictionary<string, object> dict, object obj)
        {
            List<PropertyInfo> infos = typeof(T).GetProperties().ToList();
            foreach (var propertyInfo in infos)
            {
                try
                {
                    if (dict.ContainsKey(propertyInfo.Name.ToLower()))
                    {
                        string type = propertyInfo.PropertyType.ToString();
                        switch (type)
                        {
                            case "System.Int32":
                                propertyInfo.SetValue(obj, Convert.ToInt32(dict[propertyInfo.Name.ToLower()]), BindingFlags.GetProperty, binder: null, index: null, culture: null);
                                break;
                            case "System.String":
                                propertyInfo.SetValue(obj, dict[propertyInfo.Name.ToLower()], BindingFlags.GetProperty, binder: null, index: null, culture: null);
                                break;
                            case "System.Single":
                                propertyInfo.SetValue(obj, Convert.ToSingle(dict[propertyInfo.Name.ToLower()]), BindingFlags.GetProperty, binder: null, index: null, culture: null);
                                break;
                            case "System.Boolean":
                                propertyInfo.SetValue(obj, Convert.ToBoolean(dict[propertyInfo.Name.ToLower()]), BindingFlags.GetProperty, binder: null, index: null, culture: null);
                                break;
                            case "System.Nullable`1[System.Int32]":
                                propertyInfo.SetValue(obj, Convert.ToInt32(dict[propertyInfo.Name.ToLower()]), BindingFlags.GetProperty, binder: null, index: null, culture: null);
                                break;
                        }
                    }
                }
                catch (Exception exception)
                {
                    DebugUtil.Log(propertyInfo.Name + ":找不到对应的类型：" + propertyInfo.PropertyType);
                    DebugUtil.Log(exception.ToString());
                }
            }
        }



        protected void AddInfoToFactory<T>(object obj)
        {
            Dictionary<Type, object> modelDict = NetReaderFactory.Instance().NetModelDict;
            if (modelDict.ContainsKey(typeof(T)))
            {
                modelDict.Remove(typeof(T));
            }
            modelDict.Add(typeof(T), obj);
        }
    }
}