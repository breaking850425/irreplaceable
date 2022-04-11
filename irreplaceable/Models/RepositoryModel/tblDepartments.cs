using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using irreplaceable.Models;

public class tblDepartments : BaseClass
{
    public IRepository<Departments> repo;
    public tblDepartments()
    {
        repo = new EFGenericRepository<Departments>(new web110b_07Entities());
    }
    /// <summary>
    /// 用部門代號取得部門名稱
    /// </summary>
    /// <param name="departmentNo">部門代號</param>
    /// <returns></returns>
    public string GetDepartmentName(string departmentNo)
    {
        string str_value = "";
        var data = repo.ReadSingle(m => m.department_no == departmentNo);
        if (data != null) str_value = data.department_name;
        return str_value;
    }
}
