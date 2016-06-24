/**********************************************************
*Author: wangjiaying
*Date: 2016.6.17
*Func:
**********************************************************/
using UnityEditor;
using CryStory.Runtime;
using UnityEngine;
using Event = UnityEngine.Event;
using System;
using System.Collections.Generic;

namespace CryStory.Editor
{
    public class MissionEditor : Singleton<MissionEditor>
    {
        private StoryEditorWindow _window;
        private Rect _contentRect;

        private Vector2 _leftScrollPosition;

        private Vector2 _mouseDownPos = Vector2.zero;
        private bool _mouseIsDown = false;
        private Vector2 _mouseDownCenter = Vector2.zero;


        public Mission _currentMission;
        public void OnGUI(StoryEditorWindow window)
        {
            _contentRect = window._contentRect;
            _window = window;

            ShowGraphCenterInfo();

            //========Right Area===============
            DrawRightGrid();
            ShowMissionNodes();
            //========Left Slider Area===========
            ShowLeftSliderArea();
            CreateMissionNode();

            DragGraph();
        }

        private void ShowGraphCenterInfo()
        {
            GUI.Label(new Rect(_contentRect.x, _contentRect.y, 120, 20), _window._storyObject.StoryCenter.ToString());

            if (GUI.Button(new Rect(_contentRect.x+3, _contentRect.y + 20, 50, 22), "Reset", _window.skin.button))
            {
                _window._storyObject.StoryCenter = Vector2.zero;
            }
        }

        private void ShowMissionNodes()
        {
            List<Mission> missionList = _window._storyObject._story._missionList;
            for (int i = 0; i < missionList.Count; i++)
            {
                Mission m = missionList[i];

                Vector2 pos = Node2Content(m._position);
                Rect missionRect = new Rect(pos.x, pos.y, 200, 50);
                //GUIStyle style = new GUIStyle();
                //style.normal.background = Texture2D.whiteTexture;
                GUI.Box(missionRect, m._name, _window.skin.box);

                //Test for bezier
                if (i + 1 < missionList.Count)
                {
                    Mission m2 = missionList[i + 1];
                    Vector2 pos1 = new Vector2(missionRect.max.x, missionRect.max.y - 25);//Node2Content(missionRect.max);
                    Vector2 pos2 = Node2Content(new Vector2(m2._position.x, m2._position.y + 25));
                    float tgl = Vector2.Distance(pos1, pos2) * 0.4f;
                    Vector3 startTangent = pos1 + Vector2.right * tgl;
                    Vector3 endTangent = pos2 - Vector2.right * tgl;
                    Handles.DrawBezier(pos1, pos2, startTangent, endTangent, new Color(1, 1, 1, 0.4f), null, 2f);
                    Handles.DrawBezier(pos1, pos2, startTangent, endTangent, new Color(1, 1, 1, 0.3f), null, 5f);
                }

                if (Event.current != null)
                {
                    if (Event.current.button == 0)
                    {
                        if (Event.current.type == EventType.MouseDown && Tools.IsValidMouseAABB(missionRect))
                        {
                            _mouseIsDown = true;
                            _mouseDownPos = Event.current.mousePosition;
                            _mouseDownCenter = m._position;
                            _currentMission = m;
                        }

                        if (Event.current.type == EventType.MouseUp)
                        {
                            _mouseIsDown = false;
                        }

                        if (Event.current.type == EventType.MouseDrag && Event.current.button == 0 && _mouseIsDown)
                        {
                            //Debug.Log(Event.current.button);
                            Vector2 offset = Event.current.mousePosition - _mouseDownPos;

                            _currentMission._position = _mouseDownCenter + offset;

                            //Vector2 contentPos = Node2Content(newPos);
                            //if (contentPos.x < _contentRect.x)
                            //{
                            //    _currentMission._position = Content2Node(new Vector2(_contentRect.x, contentPos.y));
                            //}
                            //else _currentMission._position = newPos;
                        }
                    }
                }

            }
        }

        private void CreateMissionNode()
        {
            if (Event.current != null)
            {
                if (Event.current.type == EventType.MouseDown)
                {
                    if (Event.current.button == 1)
                    {
                        //Debug.Log(Event.current.mousePosition + "     Rect:" + _contentRect);
                        Vector2 mousePos = Event.current.mousePosition;
                        if (mousePos.x < _contentRect.x) return;
                        if (mousePos.y < _contentRect.y) return;
                        GenericMenu menu = new GenericMenu();
                        menu.AddItem(new GUIContent("Create/New Mission"), false, () =>
                        {
                            _currentMission = _window._storyObject.AddNewMission();
                            _currentMission._position = Content2Node(mousePos);
                            _currentMission._name = "New Mission";
                        });
                        menu.ShowAsContext();
                    }
                }
            }
        }

        private void ShowLeftSliderArea()
        {
            Rect background = new Rect(0, _window._topHeight, _window._leftWidth, _window._windowRect.height);
            GUI.Box(background, "", _window.StyleBackground);

            _leftScrollPosition = GUI.BeginScrollView(new Rect(0, _window._topHeight, _window._leftWidth, _window._windowRect.height - _window._topHeight), _leftScrollPosition, new Rect(0, _window._topHeight, _window._leftWidth - 30, _window._windowRect.height - _window._topHeight), false, true, _window.skin.horizontalScrollbar, _window.skin.verticalScrollbar);
            //_leftScrollPosition = GUILayout.BeginScrollView(_leftScrollPosition, false, true, _window.skin.horizontalScrollbar, _window.skin.verticalScrollbar, _window.skin.GetStyle("Background"), GUILayout.Width(_window._leftWidth));
            GUILayout.Space(5);
            for (int i = 0; i < 12; i++)
            {
                GUILayout.Label("Test===================");
                GUILayout.Button("Test Button");
            }
            GUI.EndScrollView();
            //GUILayout.EndScrollView();
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
                if (Event.current.button != 2) return;

                if (Event.current.type == EventType.MouseDown)
                {
                    _mouseDownPos = Event.current.mousePosition;
                    _mouseDownCenter = _window._storyObject.StoryCenter;
                }

                if (Event.current.type == EventType.MouseDrag)
                {
                    //_window.Repaint();
                    //                    Debug.Log("Window Center:" + _window._story._mission.graphCenter + "  mousePos:" +
                    //Event.current.mousePosition);
                    Vector2 offset = Event.current.mousePosition - _mouseDownPos;
                    _window._storyObject.StoryCenter = _mouseDownCenter - offset;
                }
            }
        }

        //Tool===========
        Vector2 Content2Node(Vector2 pos)
        {
            Vector2 center = _contentRect.size * 0.5f;
            center.x = (int)(center.x);
            center.y = (int)(center.y);

            return pos + _window._storyObject.StoryCenter - center;
        }

        Vector2 Node2Content(Vector2 pos)
        {
            Vector2 center = _contentRect.size * 0.5f;
            center.x = (int)(center.x);
            center.y = (int)(center.y);

            return center + pos - _window._storyObject.StoryCenter;
        }

    }
}
