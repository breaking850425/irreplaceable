using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 帳號目前的權限
/// </summary>
public class AccountSecurity
{
    /// <summary>
    /// 新增權限
    /// </summary>
    public bool Add { get; set; } = false;
    /// <summary>
    /// 修改權限
    /// </summary>
    public bool Edit { get; set; } = false;
    /// <summary>
    /// 刪除權限
    /// </summary>
    public bool Delete { get; set; } = false;
    /// <summary>
    /// 列印權限
    /// </summary>
    public bool Print { get; set; } = false;
    /// <summary>
    /// 上傳權限
    /// </summary>
    public bool Upload { get; set; } = false;
    /// <summary>
    /// 下載權限
    /// </summary>
    public bool Download { get; set; } = false;
    /// <summary>
    /// 確認審核權限
    /// </summary>
    public bool Confirm { get; set; } = false;
    /// <summary>
    /// 取消回復權限
    /// </summary>
    public bool Undo { get; set; } = false;
    /// <summary>
    /// 是否有權限
    /// </summary>
    public bool IsSecurity { get { return (Add || Edit || Delete || Print || Upload || Download || Confirm || Undo); } }
}
