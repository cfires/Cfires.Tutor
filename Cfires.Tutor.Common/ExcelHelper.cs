using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;

class ExcelHelper
{
    string myFileName;
    Application myExcel;
    Workbook myWorkBook;

    /// <summary>
    /// 构造函数，不创建Excel工作薄
    /// </summary>
    public ExcelHelper()
    {
    }

    /// <summary>
    /// 创建Excel工作薄
    /// </summary>
    public void CreateExcel()
    {
        myExcel = new Application();
        myWorkBook = myExcel.Application.Workbooks.Add(true);
    }

    /// <summary>
    /// 显示Excel
    /// </summary>
    public void ShowExcel()
    {
        myExcel.Visible = true;
    }

    /// <summary>
    /// 将数据写入Excel
    /// </summary>
    /// <param name="data">要写入的二维数组数据</param>
    /// <param name="startRow">Excel中的起始行</param>
    /// <param name="startColumn">Excel中的起始列</param>
    public void WriteData(string[,] data, int startRow, int startColumn)
    {
        int rowNumber = data.GetLength(0);
        int columnNumber = data.GetLength(1);

        for (int i = 0; i < rowNumber; i++)
        {
            for (int j = 0; j < columnNumber; j++)
            {
                //在Excel中，如果某单元格以单引号“'”开头，表示该单元格为纯文本，因此，我们在每个单元格前面加单引号。 
                myExcel.Cells[startRow + i, startColumn + j] = "'" + data[i, j];
            }
        }
    }

    /// <summary>
    /// 将数据写入Excel
    /// </summary>
    /// <param name="data">要写入的字符串</param>
    /// <param name="starRow">写入的行</param>
    /// <param name="startColumn">写入的列</param>
    public void WriteData(string data, int row, int column)
    {
        myExcel.Cells[row, column] = data;
    }

    /// <summary>
    /// 将数据写入Excel
    /// </summary>
    /// <param name="data">要写入的数据表</param>
    /// <param name="startRow">Excel中的起始行</param>
    /// <param name="startColumn">Excel中的起始列</param>
    public void WriteData(System.Data.DataTable data, int startRow, int startColumn)
    {
        for (int i = 0; i <= data.Rows.Count - 1; i++)
        {
            for (int j = 0; j <= data.Columns.Count - 1; j++)
            {
                //在Excel中，如果某单元格以单引号“'”开头，表示该单元格为纯文本，因此，我们在每个单元格前面加单引号。 
                myExcel.Cells[startRow + i, startColumn + j] = "'" + data.Rows[i][j].ToString();
            }
        }
    }

    /// <summary>
    /// 读取指定单元格数据
    /// </summary>
    /// <param name="row">行序号</param>
    /// <param name="column">列序号</param>
    /// <returns>该格的数据</returns>
    public string ReadData(int row, int column)
    {
        Range range = myExcel.get_Range(myExcel.Cells[row, column], myExcel.Cells[row, column]);
        return range.Text.ToString();
    }

    /// <summary>
    /// 向Excel中插入图片
    /// </summary>
    /// <param name="pictureName">图片的绝对路径加文件名</param>
    public void InsertPictures(string pictureName)
    {
        Worksheet worksheet = (Worksheet)myExcel.ActiveSheet;
        //后面的数字表示位置，位置默认
        worksheet.Shapes.AddPicture(pictureName, MsoTriState.msoFalse, MsoTriState.msoTrue, 10, 10, 150, 150);
    }

    /// <summary>
    /// 向Excel中插入图片
    /// </summary>
    /// <param name="pictureName">图片的绝对路径加文件名</param>
    /// <param name="left">左边距</param>
    /// <param name="top">右边距</param>
    /// <param name="width">宽</param>
    /// <param name="heigth">高</param>
    public void InsertPictures(string pictureName, int left, int top, int width, int heigth)
    {
        Worksheet worksheet = (Worksheet)myExcel.ActiveSheet;
        worksheet.Shapes.AddPicture(pictureName, MsoTriState.msoFalse, MsoTriState.msoTrue, top, left, heigth, width);
    }


    /// <summary>
    /// 向Excel中插入图片
    /// </summary>
    /// <param name="pictureName">图片的绝对路径加文件名</param>
    /// <param name="left">左边距</param>
    /// <param name="top">右边距</param>
    /// <param name="width">宽</param>
    /// <param name="heigth">高</param>
    /// <param name="cell1">位置</param>
    public void InsertPictures(string pictureName, int left, int top, int width, int heigth, object cell1)
    {
        Worksheet worksheet = (Worksheet)myExcel.ActiveSheet;
        Range image1 = worksheet.get_Range(cell1, Missing.Value);
        image1.Select();
        worksheet.Shapes.AddPicture(pictureName, MsoTriState.msoFalse, MsoTriState.msoTrue, top, left, heigth, width);
    }

    /// <summary>
    /// 重命名工作表
    /// </summary>
    /// <param name="sheetNum">工作表序号，从左到右，从1开始</param>
    /// <param name="newSheetName">新的工作表名</param>
    public void ReNameSheet(int sheetNum, string newSheetName)
    {
        Worksheet worksheet = (Worksheet)myExcel.Worksheets[sheetNum];
        worksheet.Name = newSheetName;
    }

    /// <summary>
    /// 重命名工作表
    /// </summary>
    /// <param name="oldSheetName">原有工作表名</param>
    /// <param name="newSheetName">新的工作表名</param>
    public void ReNameSheet(string oldSheetName, string newSheetName)
    {
        Worksheet worksheet = (Worksheet)myExcel.Worksheets[oldSheetName];
        worksheet.Name = newSheetName;
    }

    /// <summary>
    /// 新建工作表
    /// </summary>
    /// <param name="sheetName">工作表名</param>
    public void CreateWorkSheet(string sheetName)
    {
        Worksheet newWorksheet = (Worksheet)myWorkBook.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        newWorksheet.Name = sheetName;
    }

    /// <summary>
    /// 激活工作表
    /// </summary>
    /// <param name="sheetName">工作表名</param>
    public void ActivateSheet(string sheetName)
    {
        Worksheet worksheet = (Worksheet)myExcel.Worksheets[sheetName];
        worksheet.Activate();
    }

    /// <summary>
    /// 激活工作表
    /// </summary>
    /// <param name="sheetNum">工作表序号</param>
    public void ActivateSheet(int sheetNum)
    {
        Worksheet worksheet = (Worksheet)myExcel.Worksheets[sheetNum];
        worksheet.Activate();
    }

    /// <summary>
    /// 删除一个工作表
    /// </summary>
    /// <param name="SheetName">删除的工作表名</param>
    public void DeleteSheet(int sheetNum)
    {
        ((Worksheet)myWorkBook.Worksheets[sheetNum]).Delete();
    }

    /// <summary>
    /// 删除一个工作表
    /// </summary>
    /// <param name="SheetName">删除的工作表序号</param>
    public void DeleteSheet(string sheetName)
    {
        ((Worksheet)myWorkBook.Worksheets[sheetName]).Delete();
    }

    /// <summary>
    /// 合并单元格
    /// </summary>
    /// <param name="startRow">起始行</param>
    /// <param name="startColumn">起始列</param>
    /// <param name="endRow">结束行</param>
    /// <param name="endColumn">结束列</param>
    public void CellsUnite(int startRow, int startColumn, int endRow, int endColumn)
    {
        Range range = myExcel.get_Range(myExcel.Cells[startRow, startColumn], myExcel.Cells[endRow, endColumn]);
        range.MergeCells = true;
    }

    /// <summary>
    /// 单元格文字对齐方式
    /// </summary>
    /// <param name="startRow">起始行</param>
    /// <param name="startColumn">起始列</param>
    /// <param name="endRow">结束行</param>
    /// <param name="endColumn">结束列</param>
    /// <param name="hAlign">水平对齐</param>
    /// <param name="vAlign">垂直对齐</param>
    public void CellsAlignment(int startRow, int startColumn, int endRow, int endColumn, ExcelHAlign hAlign, ExcelVAlign vAlign)
    {
        Range range = myExcel.get_Range(myExcel.Cells[startRow, startColumn], myExcel.Cells[endRow, endColumn]);
        range.HorizontalAlignment = hAlign;
        range.VerticalAlignment = vAlign;
    }

    /// <summary>
    /// 绘制指定单元格的边框
    /// </summary>
    /// <param name="startRow">起始行</param>
    /// <param name="startColumn">起始列</param>
    /// <param name="endRow">结束行</param>
    /// <param name="endColumn">结束列</param>
    public void CellsDrawFrame(int startRow, int startColumn, int endRow, int endColumn)
    {
        CellsDrawFrame(startRow, startColumn, endRow, endColumn,
            true, true, true, true, true, true, false, false,
            LineStyle.连续直线, BorderWeight.细, ColorIndex.自动);
    }

    /// <summary>
    /// 绘制指定单元格的边框
    /// </summary>
    /// <param name="startRow">起始行</param>
    /// <param name="startColumn">起始列</param>
    /// <param name="endRow">结束行</param>
    /// <param name="endColumn">结束列</param>
    /// <param name="isDrawTop">是否画上外框</param>
    /// <param name="isDrawBottom">是否画下外框</param>
    /// <param name="isDrawLeft">是否画左外框</param>
    /// <param name="isDrawRight">是否画右外框</param>
    /// <param name="isDrawHInside">是否画水平内框</param>
    /// <param name="isDrawVInside">是否画垂直内框</param>
    /// <param name="isDrawDown">是否画斜向下线</param>
    /// <param name="isDrawUp">是否画斜向上线</param>
    /// <param name="lineStyle">线类型</param>
    /// <param name="borderWeight">线粗细</param>
    /// <param name="color">线颜色</param>
    public void CellsDrawFrame(int startRow, int startColumn, int endRow, int endColumn,
        bool isDrawTop, bool isDrawBottom, bool isDrawLeft, bool isDrawRight,
        bool isDrawHInside, bool isDrawVInside, bool isDrawDiagonalDown, bool isDrawDiagonalUp,
        LineStyle lineStyle, BorderWeight borderWeight, ColorIndex color)
    {
        //获取画边框的单元格
        Range range = myExcel.get_Range(myExcel.Cells[startRow, startColumn], myExcel.Cells[endRow, endColumn]);

        //清除所有边框
        range.Borders[XlBordersIndex.xlEdgeTop].LineStyle = LineStyle.无;
        range.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = LineStyle.无;
        range.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = LineStyle.无;
        range.Borders[XlBordersIndex.xlEdgeRight].LineStyle = LineStyle.无;
        range.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = LineStyle.无;
        range.Borders[XlBordersIndex.xlInsideVertical].LineStyle = LineStyle.无;
        range.Borders[XlBordersIndex.xlDiagonalDown].LineStyle = LineStyle.无;
        range.Borders[XlBordersIndex.xlDiagonalUp].LineStyle = LineStyle.无;

        //以下是按参数画边框 
        if (isDrawTop)
        {
            range.Borders[XlBordersIndex.xlEdgeTop].LineStyle = lineStyle;
            range.Borders[XlBordersIndex.xlEdgeTop].Weight = borderWeight;
            range.Borders[XlBordersIndex.xlEdgeTop].ColorIndex = color;
        }

        if (isDrawBottom)
        {
            range.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = lineStyle;
            range.Borders[XlBordersIndex.xlEdgeBottom].Weight = borderWeight;
            range.Borders[XlBordersIndex.xlEdgeBottom].ColorIndex = color;
        }

        if (isDrawLeft)
        {
            range.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = lineStyle;
            range.Borders[XlBordersIndex.xlEdgeLeft].Weight = borderWeight;
            range.Borders[XlBordersIndex.xlEdgeLeft].ColorIndex = color;
        }

        if (isDrawRight)
        {
            range.Borders[XlBordersIndex.xlEdgeRight].LineStyle = lineStyle;
            range.Borders[XlBordersIndex.xlEdgeRight].Weight = borderWeight;
            range.Borders[XlBordersIndex.xlEdgeRight].ColorIndex = color;
        }

        if (isDrawVInside)
        {
            range.Borders[XlBordersIndex.xlInsideVertical].LineStyle = lineStyle;
            range.Borders[XlBordersIndex.xlInsideVertical].Weight = borderWeight;
            range.Borders[XlBordersIndex.xlInsideVertical].ColorIndex = color;
        }

        if (isDrawHInside)
        {
            range.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = lineStyle;
            range.Borders[XlBordersIndex.xlInsideHorizontal].Weight = borderWeight;
            range.Borders[XlBordersIndex.xlInsideHorizontal].ColorIndex = color;
        }

        if (isDrawDiagonalDown)
        {
            range.Borders[XlBordersIndex.xlDiagonalDown].LineStyle = lineStyle;
            range.Borders[XlBordersIndex.xlDiagonalDown].Weight = borderWeight;
            range.Borders[XlBordersIndex.xlDiagonalDown].ColorIndex = color;
        }

        if (isDrawDiagonalUp)
        {
            range.Borders[XlBordersIndex.xlDiagonalUp].LineStyle = lineStyle;
            range.Borders[XlBordersIndex.xlDiagonalUp].Weight = borderWeight;
            range.Borders[XlBordersIndex.xlDiagonalUp].ColorIndex = color;
        }
    }

    /// <summary>
    /// 单元格背景色及填充方式
    /// </summary>
    /// <param name="startRow">起始行</param>
    /// <param name="startColumn">起始列</param>
    /// <param name="endRow">结束行</param>
    /// <param name="endColumn">结束列</param>
    /// <param name="color">颜色索引</param>
    public void CellsBackColor(int startRow, int startColumn, int endRow, int endColumn, ColorIndex color)
    {
        Range range = myExcel.get_Range(myExcel.Cells[startRow, startColumn], myExcel.Cells[endRow, endColumn]);
        range.Interior.ColorIndex = color;
        range.Interior.Pattern = Pattern.Solid;
    }

    /// <summary>
    /// 单元格背景色及填充方式
    /// </summary>
    /// <param name="startRow">起始行</param>
    /// <param name="startColumn">起始列</param>
    /// <param name="endRow">结束行</param>
    /// <param name="endColumn">结束列</param>
    /// <param name="color">颜色索引</param>
    /// <param name="pattern">填充方式</param>
    public void CellsBackColor(int startRow, int startColumn, int endRow, int endColumn, ColorIndex color, Pattern pattern)
    {
        Range range = myExcel.get_Range(myExcel.Cells[startRow, startColumn], myExcel.Cells[endRow, endColumn]);
        range.Interior.ColorIndex = color;
        range.Interior.Pattern = pattern;
    }

    /// <summary>
    /// 设置行高
    /// </summary>
    /// <param name="startRow">起始行</param>
    /// <param name="endRow">结束行</param>
    /// <param name="height">行高</param>
    public void SetRowHeight(int startRow, int endRow, int height)
    {
        //获取当前正在使用的工作表
        Worksheet worksheet = (Worksheet)myExcel.ActiveSheet;
        Range range = (Range)worksheet.Rows[startRow.ToString() + ":" + endRow.ToString(), System.Type.Missing];
        range.RowHeight = height;
    }

    /// <summary>
    /// 自动调整行高
    /// </summary>
    /// <param name="columnNum">列号</param>
    public void RowAutoFit(int rowNum)
    {
        //获取当前正在使用的工作表
        Worksheet worksheet = (Worksheet)myExcel.ActiveSheet;
        Range range = (Range)worksheet.Rows[rowNum.ToString() + ":" + rowNum.ToString(), System.Type.Missing];
        range.EntireColumn.AutoFit();

    }

    /// <summary>
    /// 设置列宽
    /// </summary>
    /// <param name="startColumn">起始列(列对应的字母)</param>
    /// <param name="endColumn">结束列(列对应的字母)</param>
    /// <param name="width"></param>
    public void SetColumnWidth(string startColumn, string endColumn, int width)
    {
        //获取当前正在使用的工作表
        Worksheet worksheet = (Worksheet)myExcel.ActiveSheet;
        Range range = (Range)worksheet.Columns[startColumn + ":" + endColumn, System.Type.Missing];
        range.ColumnWidth = width;
    }

    /// <summary>
    /// 设置列宽
    /// </summary>
    /// <param name="startColumn">起始列</param>
    /// <param name="endColumn">结束列</param>
    /// <param name="width"></param>
    public void SetColumnWidth(int startColumn, int endColumn, int width)
    {
        string strStartColumn = GetColumnName(startColumn);
        string strEndColumn = GetColumnName(endColumn);
        //获取当前正在使用的工作表
        Worksheet worksheet = (Worksheet)myExcel.ActiveSheet;
        Range range = (Range)worksheet.Columns[strStartColumn + ":" + strEndColumn, System.Type.Missing];
        range.ColumnWidth = width;
    }

    /// <summary>
    /// 自动调整列宽
    /// </summary>
    /// <param name="columnNum">列号</param>
    public void ColumnAutoFit(string column)
    {
        //获取当前正在使用的工作表
        Worksheet worksheet = (Worksheet)myExcel.ActiveSheet;
        Range range = (Range)worksheet.Columns[column + ":" + column, System.Type.Missing];
        range.EntireColumn.AutoFit();

    }

    /// <summary>
    /// 自动调整列宽
    /// </summary>
    /// <param name="columnNum">列号</param>
    public void ColumnAutoFit(int columnNum)
    {
        string strcolumnNum = GetColumnName(columnNum);
        //获取当前正在使用的工作表
        Worksheet worksheet = (Worksheet)myExcel.ActiveSheet;
        Range range = (Range)worksheet.Columns[strcolumnNum + ":" + strcolumnNum, System.Type.Missing];
        range.EntireColumn.AutoFit();

    }

    /// <summary>
    /// 字体颜色
    /// </summary>
    /// <param name="startRow">起始行</param>
    /// <param name="startColumn">起始列</param>
    /// <param name="endRow">结束行</param>
    /// <param name="endColumn">结束列</param>
    /// <param name="color">颜色索引</param>
    public void FontColor(int startRow, int startColumn, int endRow, int endColumn, ColorIndex color)
    {
        Range range = myExcel.get_Range(myExcel.Cells[startRow, startColumn], myExcel.Cells[endRow, endColumn]);
        range.Font.ColorIndex = color;
    }

    /// <summary>
    /// 字体样式(加粗,斜体,下划线)
    /// </summary>
    /// <param name="startRow">起始行</param>
    /// <param name="startColumn">起始列</param>
    /// <param name="endRow">结束行</param>
    /// <param name="endColumn">结束列</param>
    /// <param name="isBold">是否加粗</param>
    /// <param name="isItalic">是否斜体</param>
    /// <param name="underline">下划线类型</param>
    public void FontStyle(int startRow, int startColumn, int endRow, int endColumn, bool isBold, bool isItalic, UnderlineStyle underline)
    {
        Range range = myExcel.get_Range(myExcel.Cells[startRow, startColumn], myExcel.Cells[endRow, endColumn]);
        range.Font.Bold = isBold;
        range.Font.Underline = underline;
        range.Font.Italic = isItalic;
    }

    /// <summary>
    /// 单元格字体及大小
    /// </summary>
    /// <param name="startRow">起始行</param>
    /// <param name="startColumn">起始列</param>
    /// <param name="endRow">结束行</param>
    /// <param name="endColumn">结束列</param>
    /// <param name="fontName">字体名称</param>
    /// <param name="fontSize">字体大小</param>
    public void FontNameSize(int startRow, int startColumn, int endRow, int endColumn, string fontName, int fontSize)
    {
        Range range = myExcel.get_Range(myExcel.Cells[startRow, startColumn], myExcel.Cells[endRow, endColumn]);
        range.Font.Name = fontName;
        range.Font.Size = fontSize;
    }

    /// <summary>
    /// 打开一个存在的Excel文件
    /// </summary>
    /// <param name="fileName">Excel完整路径加文件名</param>
    public void Open(string fileName)
    {
        myExcel = new Application();
        myWorkBook = myExcel.Workbooks.Add(fileName);
        myFileName = fileName;
    }

    /// <summary>
    /// 保存Excel
    /// </summary>
    /// <returns>保存成功返回True</returns>
    public bool Save()
    {
        if (myFileName == "")
        {
            return false;
        }
        else
        {
            try
            {
                myWorkBook.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Excel文档另存为
    /// </summary>
    /// <param name="fileName">保存完整路径加文件名</param>
    /// <returns>保存成功返回True</returns>
    public bool SaveAs(string fileName)
    {
        try
        {
            myWorkBook.SaveAs(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            return true;

        }
        catch (Exception ex)
        {
            return false;

        }
    }

    /// <summary>
    /// 关闭Excel
    /// </summary>
    public void Close()
    {
        myWorkBook.Close(Type.Missing, Type.Missing, Type.Missing);
        myExcel.Quit();
        myWorkBook = null;
        myExcel = null;
        GC.Collect();
    }

    /// <summary>
    /// 关闭Excel
    /// </summary>
    /// <param name="isSave">是否保存</param>
    public void Close(bool isSave)
    {
        myWorkBook.Close(isSave, Type.Missing, Type.Missing);
        myExcel.Quit();
        myWorkBook = null;
        myExcel = null;
        GC.Collect();
    }

    /// <summary>
    /// 关闭Excel
    /// </summary>
    /// <param name="isSave">是否保存</param>
    /// <param name="fileName">存储文件名</param>
    public void Close(bool isSave, string fileName)
    {
        myWorkBook.Close(isSave, fileName, Type.Missing);
        myExcel.Quit();
        myWorkBook = null;
        myExcel = null;
        GC.Collect();
    }

    #region 私有成员
    private string GetColumnName(int number)
    {
        int h, l;
        h = number / 26;
        l = number % 26;
        if (l == 0)
        {
            h -= 1;
            l = 26;
        }
        string s = GetLetter(h) + GetLetter(l);
        return s;
    }

    private string GetLetter(int number)
    {
        switch (number)
        {
            case 1:
                return "A";
            case 2:
                return "B";
            case 3:
                return "C";
            case 4:
                return "D";
            case 5:
                return "E";
            case 6:
                return "F";
            case 7:
                return "G";
            case 8:
                return "H";
            case 9:
                return "I";
            case 10:
                return "J";
            case 11:
                return "K";
            case 12:
                return "L";
            case 13:
                return "M";
            case 14:
                return "N";
            case 15:
                return "O";
            case 16:
                return "P";
            case 17:
                return "Q";
            case 18:
                return "R";
            case 19:
                return "S";
            case 20:
                return "T";
            case 21:
                return "U";
            case 22:
                return "V";
            case 23:
                return "W";
            case 24:
                return "X";
            case 25:
                return "Y";
            case 26:
                return "Z";
            default:
                return "";
        }
    }
    #endregion


}

/// <summary>
/// 水平对齐方式
/// </summary>
public enum ExcelHAlign
{
    常规 = 1,
    靠左,
    居中,
    靠右,
    填充,
    两端对齐,
    跨列居中,
    分散对齐
}

/// <summary>
/// 垂直对齐方式
/// </summary>
public enum ExcelVAlign
{
    靠上 = 1,
    居中,
    靠下,
    两端对齐,
    分散对齐
}

/// <summary>
/// 线粗
/// </summary>
public enum BorderWeight
{
    极细 = 1,
    细 = 2,
    粗 = -4138,
    极粗 = 4
}

/// <summary>
/// 线样式
/// </summary>
public enum LineStyle
{
    连续直线 = 1,
    短线 = -4115,
    线点相间 = 4,
    短线间两点 = 5,
    点 = -4118,
    双线 = -4119,
    无 = -4142,
    少量倾斜点 = 13
}

/// <summary>
/// 下划线方式
/// </summary>
public enum UnderlineStyle
{
    无下划线 = -4142,
    双线 = -4119,
    双线充满全格 = 5,
    单线 = 2,
    单线充满全格 = 4
}

/// <summary>
/// 单元格填充方式
/// </summary>
public enum Pattern
{
    Automatic = -4105,
    Checker = 9,
    CrissCross = 16,
    Down = -4121,
    Gray16 = 17,
    Gray25 = -4124,
    Gray50 = -4125,
    Gray75 = -4126,
    Gray8 = 18,
    Grid = 15,
    Horizontal = -4128,
    LightDown = 13,
    LightHorizontal = 11,
    LightUp = 14,
    LightVertical = 12,
    None = -4142,
    SemiGray75 = 10,
    Solid = 1,
    Up = -4162,
    Vertical = -4166
}

/// <summary>
/// 常用颜色定义,对就Excel中颜色名
/// </summary>
public enum ColorIndex
{
    无色 = -4142,
    自动 = -4105,
    黑色 = 1,
    褐色 = 53,
    橄榄 = 52,
    深绿 = 51,
    深青 = 49,
    深蓝 = 11,
    靛蓝 = 55,
    灰色80 = 56,
    深红 = 9,
    橙色 = 46,
    深黄 = 12,
    绿色 = 10,
    青色 = 14,
    蓝色 = 5,
    蓝灰 = 47,
    灰色50 = 16,
    红色 = 3,
    浅橙色 = 45,
    酸橙色 = 43,
    海绿 = 50,
    水绿色 = 42,
    浅蓝 = 41,
    紫罗兰 = 13,
    灰色40 = 48,
    粉红 = 7,
    金色 = 44,
    黄色 = 6,
    鲜绿 = 4,
    青绿 = 8,
    天蓝 = 33,
    梅红 = 54,
    灰色25 = 15,
    玫瑰红 = 38,
    茶色 = 40,
    浅黄 = 36,
    浅绿 = 35,
    浅青绿 = 34,
    淡蓝 = 37,
    淡紫 = 39,
    白色 = 2
}
