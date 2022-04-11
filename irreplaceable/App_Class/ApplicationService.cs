using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class ApplicationService : BaseClass
{
    /// <summary>
    /// 取得 FormCollection 中讀取的 CheckBox 字串 傳回佈林值字串
    /// </summary>
    /// <param name="valueString">CheckBox 字串</param>
    /// <returns></returns>
    public List<bool> GetCheckBoxListValue(string valueString)
    {
        List<bool> valueList = new List<bool>();
        if (!string.IsNullOrEmpty(valueString))
        {
            bool bln_value = false;
            bool addValue = true;
            List<string> checkedList = valueString.Split(',').ToList();
            foreach (var value in checkedList)
            {
                bln_value = bool.Parse(value);
                if (bln_value)
                {
                    valueList.Add(bln_value);
                    addValue = false;
                }
                else
                {
                    if (addValue) valueList.Add(false);
                    addValue = true;
                }
            }
        }
        return valueList;
    }
}