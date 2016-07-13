/**********************************************************
*Author: wangjiaying
*Date: 2016.6.23
*Func:
**********************************************************/

using CryStory.Runtime;
using UnityEngine;
using Event = UnityEngine.Event;

namespace CryStory.Editor
{
    public class Tools
    {

        /// <summary>
        /// 检查鼠标是否位于指定Rect之内
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public static bool IsValidMouseAABB(Rect area)
        {
            return Event.current.mousePosition.x > area.x && Event.current.mousePosition.y > area.y && Event.current.mousePosition.x < area.xMax && Event.current.mousePosition.y < area.yMax;
        }

        public static void DrawBazier(Vector2 startPos, Vector2 endPos, Color color)
        {
            DrawBazier(startPos, endPos, color, new Color(1, 1, 1, 0.3f));
        }

        public static void DrawBazier(Vector2 startPos, Vector2 endPos)
        {
            DrawBazier(startPos, endPos, new Color(1, 1, 1, 0.4f), new Color(1, 1, 1, 0.3f));
        }

        public static void DrawBazier(Vector2 startPos, Vector2 endPos, Color color, Color color2, float inSize = 2f, float outSize = 5f)
        {
            float tgl = Vector2.Distance(startPos, endPos) * 0.4f;
            Vector3 startTangent = startPos + Vector2.right * tgl;
            Vector3 endTangent = endPos - Vector2.right * tgl;
            UnityEditor.Handles.DrawBezier(startPos, endPos, startTangent, endTangent, color, null, inSize);
            UnityEditor.Handles.DrawBezier(startPos, endPos, startTangent, endTangent, color2, null, outSize);

            //Draw arrow
            UnityEditor.Handles.DrawLine(endPos + new Vector2(-10, -6), endPos);
            UnityEditor.Handles.DrawLine(endPos + new Vector2(-10, 6), endPos);
        }


        public static bool MouseDown { get { if (Event.current != null) return Event.current.type == EventType.MouseDown; return false; } }
        public static bool MouseUp { get { if (Event.current != null) return Event.current.type == EventType.MouseUp; return false; } }
        public static bool MouseDrag { get { if (Event.current != null) return Event.current.type == EventType.MouseDrag; return false; } }
        public static bool MouseDoubleClick { get { if (MouseDown) { return Event.current.clickCount > 1; } return false; } }


        public static Rect CalcLeftLinkRect(Rect missionRect)
        {
            return new Rect(new Vector2(missionRect.min.x, missionRect.min.y + 12), new Vector2(25, 25));
        }

        public static Rect CalcRightLinkRect(Rect missionRect)
        {
            return new Rect(new Vector2(missionRect.max.x - 25, missionRect.min.y + 12), new Vector2(25, 25));
        }


        public const float _nodeWidth = 200f;
        public const float _nodeHeight = 50f;
        public const float _nodeHalfHeight = 25f;
        public static Rect GetNodeRect(Vector2 realPos)
        {
            return new Rect(realPos.x, realPos.y, 200, 50);
        }




        //EditorGUIUtility.singleLineHeight
    }
}