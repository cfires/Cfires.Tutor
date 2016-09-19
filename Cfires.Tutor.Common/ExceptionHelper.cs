using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cfires.Tutor.Common
{
    /// <summary>
    /// 异常协助
    /// </summary>
    public static class ExceptionHelper
    {
        /// <summary>
        /// 表达式为true时抛出异常(提示信息)
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="message">消息</param>
        /// <param name="innerException">内部异常</param>
        public static void TrueThrow(bool expression, string message, System.Exception innerException = null)
        {
            TrueThrow<ApplicationException>(expression, message, innerException);
        }


        /// <summary>
        /// 表达式为true时抛出异常
        /// </summary>
        /// <typeparam name="T">异常类型</typeparam>
        /// <param name="expression">表达式</param>
        /// <param name="message">消息</param>
        /// <param name="innerException">内部异常</param>
        public static void TrueThrow<T>(bool expression, string message, System.Exception innerException = null) where T : Exception
        {
            if (expression)
            {
                object[] argValues = new object[2];
                argValues[0] = message;
                argValues[1] = innerException;

                Type[] argTypes = new Type[2];
                argTypes[0] = typeof(string);
                argTypes[1] = typeof(System.Exception);

                Type t = typeof(T);

                ConstructorInfo ci = t.GetConstructor(argTypes);

                if (ci == null)
                    throw new Exception(string.Format("实例化异常{0}出错", t.FullName), null);

                throw (T)ci.Invoke(argValues);
            }
        }

        /// <summary>
        /// 表达式为False时抛出异常
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="message">消息</param>
        /// <param name="innerException">内部异常</param>
        public static void FalseThrow(bool expression, string message, System.Exception innerException = null)
        {
            FalseThrow<ApplicationException>(expression, message, innerException);
        }

        /// <summary>
        /// 表达式为False时抛出异常
        /// </summary>
        /// <typeparam name="T">异常类型</typeparam>
        /// <param name="expression">表达式</param>
        /// <param name="logType">消息类型</param>
        /// <param name="message">消息</param>
        /// <param name="innerException">内部异常</param>
        public static void FalseThrow<T>(bool expression, string message, System.Exception innerException = null) where T : Exception
        {
            TrueThrow<T>(!expression, message, innerException);
        }

        /// <summary>
        /// Object为null时抛出异常
        /// </summary>
        /// <param name="obj">要检查的实体</param>
        /// <param name="message">消息</param>
        /// <param name="innerException">内部异常</param>
        public static void IsNull(object obj, string message, System.Exception innerException = null)
        {
            IsNull<ApplicationException>(obj, message, innerException);
        }

        /// <summary>
        /// Object为null时抛出异常
        /// </summary>
        /// <typeparam name="T">异常类型</typeparam>
        /// <param name="obj">要检查的实体</param>
        /// <param name="logType">消息类型</param>
        /// <param name="message">消息</param>
        /// <param name="innerException">内部异常</param>
        public static void IsNull<T>(object obj, string message, System.Exception innerException = null) where T : Exception
        {
            TrueThrow<T>(obj == null, message, innerException);
        }

        /// <summary>
        /// 字符串为空时抛出异常
        /// </summary>
        /// <param name="obj">要检查的字符串</param>
        /// <param name="logType">消息类型</param>
        /// <param name="message">消息</param>
        /// <param name="innerException">内部异常</param>
        public static void IsNullOrEmpty(string stringObj, string message, System.Exception innerException = null)
        {
            IsNullOrEmpty<ApplicationException>(stringObj, message, innerException);
        }

        /// <summary>
        /// 字符串为空时抛出异常
        /// </summary>
        /// <typeparam name="T">异常类型</typeparam>
        /// <param name="obj">要检查的字符串</param>
        /// <param name="logType">消息类型</param>
        /// <param name="message">消息</param>
        /// <param name="innerException">内部异常</param>
        public static void IsNullOrEmpty<T>(string stringObj, string message, System.Exception innerException = null) where T : Exception
        {
            TrueThrow<T>(string.IsNullOrWhiteSpace(stringObj), message, innerException);
        }

        /// <summary>
        /// 获取真正的异常信息
        /// </summary>
        /// <param name="ex">外层异常</param>
        /// <returns>真正的异常信息</returns>
        public static System.Exception GetRealException(System.Exception ex)
        {
            System.Exception exception = ex;
            while (ex != null)
            {
                exception = ex.InnerException ?? ex;
                ex = ex.InnerException;
            }
            return exception;
        }

        /// <summary>
        /// 获取InnerException中的HZException的异常信息
        /// </summary>
        /// <param name="ex">外层异常</param>
        /// <returns>真正的异常信息</returns>
        public static Exception GetInnerHZException(System.Exception ex)
        {
            Exception exception = null;
            while (ex != null)
            {
                ex = ex.InnerException;
            }
            return exception;
        }

        /// <summary>
        /// 得到异常的消息串，主要获得了StackTrace属性
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="showFullMessage">是否忽略所有错误信息</param>
        /// <returns></returns>
        public static string GetExceptionMessage(this System.Exception ex, bool showFullMessage = false)
        {
            string msg = ex.Message;
            if (!string.IsNullOrWhiteSpace(ex.StackTrace))
            {
                List<string> list = ex.StackTrace.Split(new char[] { '\r', '\n' }).ToList();
                list = list.Where(s => s.Contains("\\"))
                    .Select(s => s.Substring(s.LastIndexOf('\\') + 1))
                    .Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                if (!showFullMessage && list.Count > 0)
                    msg = string.Concat(msg, ",", list.Aggregate((s1, s2) => string.Concat(s1, "，", s2)));
                else
                    msg = string.Concat(msg, ex.StackTrace);
            }
            if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message) && ex.StackTrace != ex.InnerException.StackTrace)
                msg = string.Concat(msg, "【内部异常】", GetExceptionMessage(ex.InnerException, showFullMessage));
            return msg;
        }
    }
}
