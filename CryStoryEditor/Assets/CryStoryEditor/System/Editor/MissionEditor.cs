/**********************************************************
*Author: wangjiaying
*Date: 2016.6.17
*Func:
**********************************************************/
using UnityEditor;
using CryStory.Runtime;
using UnityEngine;
using Event = UnityEngine.Event;

namespace CryStory.Editor
{
    public class MissionEditor : Singleton<MissionEditor>
    {
        private StoryEditorWindow _window;
        private Rect _contentRect;

        private Vector2 _leftScrollPosition;

        private bool mouseDown = false;
        private Vector2 mouseDownPos = Vector2.zero;
        private Vector2 mouseDownCenter = Vector2.zero;

        public void OnGUI(StoryEditorWindow window)
        {
            _contentRect = window._contentRect;
            _window = window;
            //========Left Slider Area===========
            ShowLeftSliderArea();
            //========Right Area===============
            DrawRightGrid();

            DragGraph();
        }

        private void ShowLeftSliderArea()
        {
            _leftScrollPosition = GUILayout.BeginScrollView(_leftScrollPosition, false, true, GUILayout.Width(_window._leftWidth));
            for (int i = 0; i < 12; i++)
            {
                GUILayout.Label("Test===================");
            }
            GUILayout.EndScrollView();
        }

        private void DrawRightGrid()
        {
            Rect coord = new Rect();
            Vector2 nodepos = Content2Node(Vector2.zero);
            coord.x = nodepos.x / 128.0f;
            coord.y = nodepos.y / 128.0f;
            coord.width = _contentRect.width / 128.0f;
            coord.height = -_contentRect.height / 128.0f;
            GUI.color = new Color(1f, 1f, 1f, 0.1f);
            GUI.DrawTextureWithTexCoords(_contentRect, _window.texGrid, coord);
            GUI.color = Color.white;
        }

        private void DragGraph()
        {
            if (Event.current != null)
            {
                if (Event.current.type == EventType.MouseDown)
                {
                    mouseDown = true;
                    mouseDownPos = Event.current.mousePosition;
                    mouseDownCenter = _window._story._mission.graphCenter;
                }
                if (Event.current.rawType == EventType.mouseUp)
                {
                    mouseDown = false;
                }
                if (/*draggingSlot == null && draggingPoseSlot == null && editingSlot == null &&*/ mouseDown && Event.current.button == 2)
                {
                    _window.Repaint();
                    //                    Debug.Log("Window Center:" + _window._story._mission.graphCenter + "  mousePos:" +
                    //Event.current.mousePosition);
                    Vector2 offset = Event.current.mousePosition - mouseDownPos;
                    _window._story._mission.graphCenter = mouseDownCenter - offset;
                }
            }
        }

        //Tool===========
        Vector2 Content2Node(Vector2 pos)
        {
            Vector2 center = _contentRect.size * 0.5f;
            center.x = (int)(center.x);
            center.y = (int)(center.y);

            return pos + _window._story._mission.graphCenter - center;
        }

        //static void DrawConnectionLines()
        //{
        //    int ncnt = modifier.nodes.Count;

        //    // Draw bezier lines
        //    for (int n = 0; n <= ncnt; ++n)
        //    {
        //        Node node = null;
        //        if (n < ncnt)
        //            node = modifier.nodes[n];
        //        else
        //            node = modifier.output;
        //        if (node == null)
        //            continue;

        //        for (int i = 0; i < node.slots.Length; ++i)
        //        {
        //            if (node.slots[i].input != null)
        //            {
        //                for (int k = 0; k < ncnt; ++k)
        //                {
        //                    if (modifier.nodes[k] == node.slots[i].input)
        //                    {
        //                        nodeOutputs[k] = true;
        //                        Vector2 thispos = Node2Content(node.editorPos);
        //                        thispos.y += (30 + i * 18);
        //                        thispos.x += 8;
        //                        Vector2 targetpos = new Vector2(nodeRects[k].xMax - 1, nodeRects[k].yMin + 8);

        //                        float tgl = Vector2.Distance(targetpos, thispos) * 0.5f;
        //                        tgl = Mathf.Min(100, tgl);

        //                        Vector2 thistg = thispos - Vector2.right * tgl;
        //                        Vector2 targettg = targetpos + Vector2.right * tgl;

        //                        Handles.DrawBezier(thispos, targetpos, thistg, targettg, new Color(1, 1, 1, 0.2f), null, 3f);

        //                        float t = ((float)((cfwindow.frame + k * 45) % 400)) / 400f;

        //                        if (CameraForgeWindow.showDirection)
        //                        {
        //                            t -= 0.025f;
        //                            Vector3 p1 = Vector3.Lerp(Vector3.Lerp(Vector3.Lerp(targetpos, targettg, t), Vector3.Lerp(targettg, thistg, t), t),
        //                                                      Vector3.Lerp(Vector3.Lerp(targettg, thistg, t), Vector3.Lerp(thistg, thispos, t), t), t);
        //                            t += 0.05f;
        //                            Vector3 p2 = Vector3.Lerp(Vector3.Lerp(Vector3.Lerp(targetpos, targettg, t), Vector3.Lerp(targettg, thistg, t), t),
        //                                                      Vector3.Lerp(Vector3.Lerp(targettg, thistg, t), Vector3.Lerp(thistg, thispos, t), t), t);

        //                            Color c = NodeColor(k);
        //                            c = Color.Lerp(c, Color.white, 0.5f);
        //                            //c.a = 0.5f;
        //                            Handles.color = c;
        //                            Handles.DrawLine(p1, p2);
        //                            Handles.color = Color.white;
        //                        }
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }  
        //}

    }
}
