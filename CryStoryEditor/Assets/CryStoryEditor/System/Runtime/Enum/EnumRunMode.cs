/**********************************************************
*Author: wangjiaying
*Date: 2016.7.14
*Func:
**********************************************************/

namespace CryStory.Runtime
{
    /// <summary>
    /// 影响当节点返回Failure或者Success时的处理
    /// </summary>
    public enum EnumRunMode
    {
        /// <summary>
        /// 无论成功失败，皆直接运行下一节点
        /// </summary>
        DirectRuning,
        /// <summary>
        /// 直到节点运行成功，才运行下一节点
        /// </summary>
        UntilSuccess,
        /// <summary>
        /// 当该节点运行失败，则直接返回上一节点
        /// </summary>
        ReturnParentNode,
        /// <summary>
        /// 若节点运行失败，则直接停止，将不再运行余下节点
        /// </summary>
        StopNodeList,
    }
}
