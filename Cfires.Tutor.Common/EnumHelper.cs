using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cfires.Tutor.Common
{
    /// <summary>
    /// 枚举描述信息
    /// </summary>
    public static class EnumHelper
    {
        private static readonly ConcurrentDictionary<string, IList<EnumItemDescription>> itemDescriptionList = new ConcurrentDictionary<string, IList<EnumItemDescription>>();
        private static readonly ConcurrentDictionary<AttrDictEntry, Attribute> attributeDictionary = new ConcurrentDictionary<AttrDictEntry, Attribute>();

        /// <summary>
        /// 解析
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="enumValue">枚举的string值</param>
        /// <returns></returns>
        public static T Parse<T>(string enumValue) where T : struct
        {
            return (T)Enum.Parse(typeof(T), enumValue);
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="enumValue">枚举string值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static T Parse<T>(string enumValue, T defaultValue) where T : struct
        {
            try
            {
                return (T)Enum.Parse(typeof(T), enumValue);
            }
            catch
            {
            }
            return defaultValue;
        }

        /// <summary>
        /// 获取枚举项描述
        /// </summary>
        /// <param name="T">枚举类型</param>
        /// <param name="enumItem">枚举项</param>
        /// <returns>枚举项描述</returns>
        public static string GetDescription<T>(int enumItem)
        {
            EnumItemDescription item = GetEnumItemDescription<T>(enumItem);

            if (item != null)
                return item.Description;

            return string.Empty;
        }

        /// <summary>
        /// 获取枚举项描述
        /// </summary>
        /// <param name="T">枚举类型</param>
        /// <param name="enumItem">枚举项</param>
        /// <returns>枚举项描述</returns>
        public static string GetDescription<T>(string enumItem)
        {
            EnumItemDescription item = GetEnumItemDescription<T>(enumItem);

            if (item != null)
                return item.Description;

            return string.Empty;
        }

        /// <summary>
        /// 获取枚举项描述
        /// </summary>
        /// <param name="enumItem">枚举项</param>
        /// <returns>枚举项描述</returns>
        public static string GetDescription(Enum enumItem)
        {
            EnumItemDescription item = GetEnumItemDescription(enumItem);

            if (item != null)
                return item.Description;

            return string.Empty;
        }

        /// <summary>
        /// 获取枚举项信息
        /// </summary>
        /// <param name="T">枚举类型</param>
        /// <param name="enumItem">枚举项</param>
        /// <returns>枚举项信息</returns>
        public static EnumItemDescription GetEnumItemDescription<T>(int enumItem)
        {
            IList<EnumItemDescription> itemList = GetEnumItemDescriptionList(typeof(T));

            return itemList.FirstOrDefault(o => o.Value == enumItem);
        }

        /// <summary>
        /// 获取枚举项信息
        /// </summary>
        /// <param name="T">枚举类型</param>
        /// <param name="enumItem">枚举项</param>
        /// <returns>枚举项信息</returns>
        public static EnumItemDescription GetEnumItemDescription<T>(string enumItem)
        {
            IList<EnumItemDescription> itemList = GetEnumItemDescriptionList(typeof(T));

            return itemList.FirstOrDefault(o => o.Name == enumItem.Trim());
        }

        /// <summary>
        /// 获取枚举项信息
        /// </summary>
        /// <param name="enumItem">枚举项</param>
        /// <returns>枚举项信息</returns>
        public static EnumItemDescription GetEnumItemDescription(Enum enumItem)
        {
            IList<EnumItemDescription> itemList = GetEnumItemDescriptionList(enumItem.GetType());

            return itemList.FirstOrDefault(o => o.Name == enumItem.ToString());
        }

        /// <summary>
        /// 获取枚举项列表
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns>枚举项列表</returns>
        public static IList<EnumItemDescription> GetEnumItemDescriptionList(Type enumType)
        {
            string key = Getkey(enumType);

            IList<EnumItemDescription> resultList = itemDescriptionList.GetOrAdd(key, (s) =>
            {
                resultList = new List<EnumItemDescription>();

                FieldInfo[] fileds = enumType.GetFields();

                for (int i = 0; i < fileds.Length; i++)
                {
                    FieldInfo fi = fileds[i];

                    if (fi.IsLiteral && fi.IsStatic)
                        resultList.Add(BuilderEnumItemInfo(fi, enumType));
                }

                resultList = resultList.OrderBy(o => o.Value).ToList();

                return resultList;
            });

            return resultList;
        }

        /// <summary>
        /// 构造枚举项信息
        /// </summary>
        /// <param name="fi">FieldInfo</param>
        /// <param name="enumType">枚举类型</param>
        /// <returns>枚举项信息</returns>
        private static EnumItemDescription BuilderEnumItemInfo(FieldInfo fi, Type enumType)
        {
            EnumItemDescription resultItem = new EnumItemDescription();

            resultItem.Name = fi.Name;
            resultItem.Value = (int)fi.GetValue(enumType);

            DescriptionAttribute descAttr = GetCustomAttribute<DescriptionAttribute>(fi);

            if (descAttr != null)
            {
                resultItem.Description = descAttr.Description;
            }

            return resultItem;
        }

        /// <summary>
        /// 获取枚举缓存Key
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns>缓存Key</returns>
        private static string Getkey(Type enumType)
        {
            return GetTypeDescription(enumType).Replace(",", "_").Replace(" ", "");
        }

        /// <summary>
        /// 得到枚举类型的dictionary
        /// </summary>
        /// <param name="enumTypeStr">要获取的枚举类型的名称。</param>
        /// <param name="useEnumString">使用枚举的string值</param>
        /// <returns></returns>
        public static Dictionary<string, string> ToDictionary(string enumTypeStr, bool useEnumString = true)
        {
            Type t = Type.GetType(enumTypeStr);
            ExceptionHelper.IsNull(t, "枚举类型配置错误(系统类型可以通过GetType().AssemblyQualifiedName获得)：" + enumTypeStr);

            return ToDictionary(t, useEnumString);
        }

        /// <summary>
        /// 得到枚举类型的dictionary
        /// </summary>
        /// <param name="enumType">枚举的类型</param>
        /// <param name="useEnumString">使用枚举的string值</param>
        /// <returns></returns>
        public static Dictionary<string, string> ToDictionary(Type enumType, bool useEnumString = true)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (int i in Enum.GetValues(enumType))
            {
                Enum enumObj = (Enum)(Enum.ToObject(enumType, i));
                dict.Add(useEnumString ? enumObj.ToString() : i.ToString(), GetDescription(enumObj));
            }
            return dict;
        }

        /// <summary>
        /// 获取某类型的短类型描述（即如下格式 “命名空间.类名称,程序集名称”，不包括版本，语言等信息）
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string GetTypeDescription(Type type)
        {
            string typeDescription = type.AssemblyQualifiedName;
            return GetTypeDescription(typeDescription);
        }

        /// <summary>
        /// 获取某类型的短类型描述（即如下格式 “命名空间.类名称,程序集名称”，不包括版本，语言等信息）
        /// </summary>
        /// <param name="typeDescription"></param>
        /// <returns></returns>
        private static string GetTypeDescription(string typeDescription)
        {
            string result = string.Empty;
            string[] typeInfoArray = typeDescription.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (typeInfoArray.Length >= 2)
            {
                result = string.Format("{0},{1}", typeInfoArray[0], typeInfoArray[1]);
            }
            else
            {
                result = typeDescription;
            }

            return result;
        }


        /// <summary>
        /// 获取特性
        /// </summary>
        /// <typeparam name="T">Attribute类型</typeparam>
        /// <param name="element">MemberInfo</param>
        /// <returns></returns>
        private static T GetCustomAttribute<T>(MemberInfo element) where T : Attribute
        {
            T result;
            Type attrType = typeof(T);

            AttrDictEntry key = CalculateKey(element, attrType, true);

            result = (T)attributeDictionary.GetOrAdd(key, (t) => (T)Attribute.GetCustomAttribute(element, attrType));

            return result;
        }

        private static AttrDictEntry CalculateKey(object element, System.Type attrType, bool inherited)
        {
            AttrDictEntry key;

            key.MemberInfo = element;
            key.AttributeType = attrType;
            key.Inherited = inherited;

            return key;
        }

        private struct AttrDictEntry
        {
            public object MemberInfo;
            public System.Type AttributeType;
            public bool Inherited;
        }

    }


    /// <summary>
    /// 枚举信息类
    /// </summary>
    public sealed class EnumItemDescription
    {
        /// <summary>
        /// 枚举值
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// 枚举名(英文)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 枚举描述(中文)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public EnumItemDescription()
        {
            this.Value = -100;
            this.Name = string.Empty;
            this.Description = string.Empty;
        }
    }
}
