/**********************************************************
*Author: wangjiaying
*Date: 2016.7.27
*Func:
**********************************************************/

namespace CryStory.Runtime
{
    public enum ValueCompare
    {
        [ValueFunc(VarType.INT | VarType.FLOAT | VarType.BOOL | VarType.STRING)]
        /// <summary>
        /// 等于
        /// </summary>
        Equal,
        [ValueFunc(VarType.INT | VarType.FLOAT | VarType.BOOL | VarType.STRING)]
        /// <summary>
        /// 不等于
        /// </summary>
        NotEqual,
        [ValueFunc(VarType.INT | VarType.FLOAT)]
        /// <summary>
        /// 小于
        /// </summary>
        Less,
        [ValueFunc(VarType.INT | VarType.FLOAT)]
        /// <summary>
        /// 大于
        /// </summary>
        Greater,
        [ValueFunc(VarType.INT | VarType.FLOAT)]
        /// <summary>
        /// 小于等于
        /// </summary>
        LessEqual,
        [ValueFunc(VarType.INT | VarType.FLOAT)]
        /// <summary>
        /// 大于等于
        /// </summary>
        GreaterEqual
    }
}
