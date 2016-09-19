using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cfires.Tutor.Common
{
    public class WordHelper
    {
        /// <summary>
        /// 根据一个模板文档（.docx），输出替换特殊字段后的Word文档
        /// </summary>
        /// <param name="templeteFilePath">模板文件</param>
        /// <param name="dic">匹配替换字段集合</param>
        /// <param name="inTable">是否替换Table中存在的关键字段</param>
        /// <returns>替换后的Word文档文件流</returns>
        public static MemoryStream ExportTableWordWithTemplete(string templeteFilePath, Dictionary<string, string> dic, bool inTable)
        {
            XWPFDocument doc = FillWordWithTemplete(templeteFilePath, dic, inTable);

            //MemoryStream ms = new MemoryStream();
            //doc.Write(ms);
            //return ms;

            //NPOI写入流中会关闭流，无法继续操作，因此使用写入文件后再次读取的方式
            return GetWordFileStream(doc);
        }

        /// <summary>
        /// 根据一个模板文档（.docx），输出替换特殊字段后的Word文档，同时添加表格数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="templeteFilePath">模板文件</param>
        /// <param name="dic">匹配替换字段集合</param>
        /// <param name="tableSource">表格数据的数据源</param>
        /// <param name="inTable">是否替换Table中存在的关键字段</param>
        /// <returns></returns>
        public static MemoryStream ExportTableWordWithTemplete<T>(string templeteFilePath, Dictionary<string, string> dic, IEnumerable<T> tableSource, bool inTable) where T : class, new()
        {
            XWPFDocument doc = FillWordWithTemplete(templeteFilePath, dic, inTable);

            if (tableSource != null && doc.Tables.Count > 0)
            {
                AddTableData<T>(doc, tableSource);
            }

            //MemoryStream ms = new MemoryStream();
            //doc.Write(ms);
            //return ms;

            //NPOI写入流中会关闭流，无法继续操作，因此使用写入文件后再次读取的方式
            return GetWordFileStream(doc);
        }


        /// <summary>
        /// 根据一个模板文档（.docx），输出替换特殊字段后的Word文档，同时添加表格数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="templeteFilePath">模板文件</param>
        /// <param name="dic">匹配替换字段集合</param>
        /// <param name="tableSource">表格数据的数据源</param>
        /// <param name="inTable">是否替换Table中存在的关键字段</param>
        /// <returns></returns>
        public static MemoryStream ExportTableWordWithTemplete<T>(string templeteFilePath, Dictionary<string, string> dic, IEnumerable<T> tableSource, Action<XWPFTableRow, T> todo, bool inTable) where T : class, new()
        {
            XWPFDocument doc = FillWordWithTemplete(templeteFilePath, dic, inTable);

            if (tableSource != null && doc.Tables.Count > 0)
            {
                AddTableData<T>(doc, tableSource, todo);
            }

            //MemoryStream ms = new MemoryStream();
            //doc.Write(ms);
            //return ms;

            //NPOI写入流中会关闭流，无法继续操作，因此使用写入文件后再次读取的方式
            return GetWordFileStream(doc);
        }

        /// <summary>
        /// NPOI写入流中会关闭流，无法继续操作，因此使用写入文件后再次读取的方式
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private static MemoryStream GetWordFileStream(XWPFDocument doc)
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".docx");
            FileStream fs = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write);
            doc.Write(fs);
            byte[] bs = File.ReadAllBytes(tempFilePath);
            File.Delete(tempFilePath);
            MemoryStream ms = new MemoryStream();
            ms.Write(bs, 0, bs.Length);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        private static XWPFDocument FillWordWithTemplete(string templeteFilePath, Dictionary<string, string> keyValues, bool inTable)
        {
            if (string.IsNullOrWhiteSpace(templeteFilePath) || !File.Exists(templeteFilePath) || Path.GetExtension(templeteFilePath).ToLower() != ".docx")
                throw new ArgumentException("模板文件无效或不存在");

            if (keyValues == null || keyValues.Count == 0)
                throw new ArgumentException("替换键值对集合不能为空");

            XWPFDocument doc = new XWPFDocument(new FileStream(templeteFilePath, FileMode.Open, FileAccess.ReadWrite));
            List<XWPFParagraph> list = doc.Paragraphs.Where(p => Regex.IsMatch(p.Text, "#\\w+#")).ToList();
            if (inTable)
            {
                //List<XWPFParagraph> listTmp = new List<XWPFParagraph>();
                doc.Tables.ToList().ForEach((t) =>
                {
                    t.Rows.ForEach((row) =>
                    {
                        row.GetTableCells().ForEach((cell) =>
                        {
                            list.AddRange(cell.Paragraphs.Where(p => Regex.IsMatch(p.Text, "#\\w+#")));
                            //listTmp.AddRange(cell.Paragraphs);
                        });
                    });
                });
            }

            foreach (XWPFParagraph p in list)
            {
                if (p.PartType == BodyType.DOCUMENT || p.PartType == BodyType.TABLECELL)
                {
                    string paragraphText = p.Text;
                    MatchCollection matches = Regex.Matches(paragraphText, "#\\w+#");
                    List<XWPFRun> runs = p.Runs.ToList();
                    foreach (Match mch in matches)
                    {
                        string mchKey = mch.Value;
                        string mchKeyNoFlag = mchKey.Replace("#", "");
                        if (keyValues.ContainsKey(mchKeyNoFlag)) //在关键字字典中
                        {
                            string value = keyValues[mchKeyNoFlag];
                            List<XWPFRun> runResult = FindRunsPair(runs);
                            if (runResult.Count == 1)
                            {
                                runResult[0].ReplaceText(mchKey, value);
                            }
                            else
                            {
                                for (int i = 0; i < runResult.Count; i++)
                                {
                                    XWPFRun runFor = runResult[i];
                                    if (i == 0)
                                    {
                                        int idxFlag = runFor.Text.IndexOf('#');
                                        string sub = runFor.Text.Substring(0, idxFlag);
                                        runFor.SetText(sub + value);
                                    }
                                    else if (i == runResult.Count - 1)
                                    {
                                        int idxFlag = runFor.Text.IndexOf('#');
                                        string sub = runFor.Text.Substring(idxFlag + 1, runFor.Text.Length - idxFlag - 1);
                                        runFor.SetText(sub);
                                    }
                                    else
                                    {
                                        runFor.SetText(string.Empty);
                                    }
                                }
                            }

                        }

                    }

                }
            }

            return doc;
        }

        private static void AddTableData<T>(XWPFDocument doc, IEnumerable<T> tableSource) where T : class, new()
        {
            XWPFTable tableAdd = doc.Tables[0];

            T obj = new T();
            PropertyInfo[] properties = obj.GetType().GetProperties().ToArray();

            int colSize = tableAdd.GetRow(0).GetTableCells().Count;
            if (colSize != properties.Length)
                throw new ArgumentException("对象属性数量与表格列数量不一致");

            foreach (T t in tableSource)
            {
                XWPFTableRow row = tableAdd.CreateRow();
                for (int i = 0; i < properties.Length; i++)
                {
                    object valObj = properties[i].GetValue(t);
                    row.GetCell(i).SetText(valObj == null ? string.Empty : valObj.ToString());
                }
            }

        }

        private static void AddTableData<T>(XWPFDocument doc, IEnumerable<T> tableSource, Action<XWPFTableRow, T> todo) where T : class, new()
        {
            XWPFTable tableAdd = doc.Tables[0];

            T obj = new T();

            foreach (T t in tableSource)
            {
                XWPFTableRow row = tableAdd.CreateRow();
                todo.Invoke(row, t);
            }

        }

        private static List<XWPFRun> FindRunsPair(List<XWPFRun> runs)
        {
            XWPFRun runStart = null;
            XWPFRun runEnd = null;
            List<XWPFRun> runResult = new List<XWPFRun>();
            foreach (XWPFRun run in runs)
            {
                MatchCollection matches = Regex.Matches(run.Text, "#");
                if (matches.Count > 1)
                {
                    runResult.Add(run);
                    break;
                }
                else if (matches.Count == 1)
                {
                    if (runStart == null)
                    {
                        runStart = run;
                        runResult.Add(run);
                    }
                    else
                    {
                        runEnd = run;
                        runResult.Add(run);
                        break;
                    }
                }
                else if (runStart != null)
                {
                    runResult.Add(run);
                }
            }

            return runResult;
        }
    }
}
