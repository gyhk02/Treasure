using DevExpress.Web.ASPxTreeList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treasure.Utility.Extend
{
    public static class ASPxTreeListExtend
    {

        #region 选择全部父节点
        /// <summary>
        /// 选择全部父节点
        /// </summary>
        /// <param name="node"></param>
        public static void SelectAllParentNode(TreeListNode node)
        {
            SelectParentNode(node);
        }
        #endregion

        #region 递归选择父节点
        /// <summary>
        /// 递归选择父节点
        /// </summary>
        /// <param name="node"></param>
        private static void SelectParentNode(TreeListNode node)
        {
            if (node.Selected == true && node.ParentNode != null)
            {
                TreeListNode parentNode = node.ParentNode;

                parentNode.Selected = true;

                if (parentNode.ParentNode != null)
                {
                    SelectParentNode(parentNode);
                }
            }
        }
        #endregion

    }
}
