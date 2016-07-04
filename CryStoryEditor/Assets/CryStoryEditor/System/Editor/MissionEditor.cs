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

        private Mission _currentMission;
        private Rect _currentNodeRect;
        private Mission _currentHover;
        private bool _isConnecting = false;
        public void OnGUI(StoryEditorWindow window)
        {
            _contentRect = window._contentRect;
            _window = window;

            ShowGraphCenterInfo();

            //========Right Area===============
            DrawRightGrid();
            ShowMissionNodes();
            ShowConnectLine();
            //========Left Slider Area===========
            ShowLeftSliderArea();
            CreateMissionNode();

            DragGraph();
        }

        private void ShowConnectLine()
        {
            if (_isConnecting && _currentMission != null)
            {
                Vector2 pos1 = new Vector2(_currentNodeRect.max.x, _currentNodeRect.max.y - Tools._nodeHalfHeight);
                Tools.DrawBazier(pos1, Event.current.mousePosition);
                if (Tools.MouseUp)
                {
                    _isConnecting = false;
                    if (_currentMission == _currentHover) return;
                    if (Tools.IsValidMouseAABB(Tools.GetNodeRect(CalcRealPosition(_currentHover._position))))
                    {
                        _currentHover.RemoveFromLastNode();
                        _currentHover._lastNode = _currentMission;
                        _currentMission.AddNextNode(_currentHover);
                    }
                }
            }
        }

        private void ShowGraphCenterInfo()
        {
            GUI.Label(new Rect(_contentRect.x, _contentRect.y, 120, 20), _window._storyObject.StoryCenter.ToString());

            if (GUI.Button(new Rect(_contentRect.x + 3, _contentRect.y + 20, 50, 22), "Reset", ResourcesManager.GetInstance.skin.button))
            {
                _window._storyObject.StoryCenter = Vector2.zero;
            }
        }

        private void ShowMissionNodes()
        {
            List<Mission> missionList = _window._storyObject._story._missionList;
            for (int i = 0; i < missionList.Count; i++)
            {
                Mission node = missionList[i];

                Vector2 pos = CalcRealPosition(node._position);
                Rect missionRect = Tools.GetNodeRect(pos);

                GUI.Box(missionRect, node._name, _currentMission == node ? ResourcesManager.GetInstance.MissionNodeOn : ResourcesManager.GetInstance.MissionNode);

                #region Event drag connection line
                if (Event.current != null)
                {
                    if (Tools.IsValidMouseAABB(missionRect))
                    {
                        _currentHover = node;

                        Rect leftRect = Tools.CalcLeftLinkRect(missionRect);
                        Rect rightRect = Tools.CalcRightLinkRect(missionRect);
                        GUIStyle inStyle = new GUIStyle();
                        GUIStyle outStyle = new GUIStyle();
                        inStyle.alignment = TextAnchor.MiddleLeft;
                        outStyle.alignment = TextAnchor.MiddleRight;

                        GUI.Box(leftRect, node._lastNode == null ? ResourcesManager.GetInstance.texInputSlot : ResourcesManager.GetInstance.texInputSlotActive, inStyle);
                        GUI.Box(rightRect, node._nextNodeList.Count < 1 ? ResourcesManager.GetInstance.texOutputSlot : ResourcesManager.GetInstance.texOutputSlotActive, outStyle);

                        if (Event.current.type == EventType.MouseDown && Tools.IsValidMouseAABB(rightRect))
                        {
                            _isConnecting = true;
                            _currentMission = node;
                            _currentNodeRect = missionRect;
                        }
                    }
                }
                #endregion

                //Test for bezier
                //if (i + 1 < missionList.Count)
                //{
                //    Mission m2 = missionList[i + 1];
                //    Vector2 pos1 = new Vector2(missionRect.max.x, missionRect.max.y - Tools._nodeHalfHeight);
                //    Vector2 pos2 = CalcRealPosition(new Vector2(m2._position.x, m2._position.y + Tools._nodeHalfHeight));
                //    Tools.DrawBazier(pos1, pos2);
                //}
                //Draw conection bazier line

                DrawBazierLine(node);

                if (_isConnecting) continue;

                #region Drag Event
                if (Event.current != null)
                {
                    if (Event.current.button == 0)
                    {
                        if (Event.current.type == EventType.MouseDown && Tools.IsValidMouseAABB(missionRect))
                        {
                            _mouseIsDown = true;
                            _mouseDownPos = Event.current.mousePosition;
                            _mouseDownCenter = node._position;
                            _currentMission = node;
                            _currentNodeRect = missionRect;
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
                #endregion

            }
        }

        private void DrawBazierLine(NodeBase node)
        {
            for (int j = 0; j < node._nextNodeList.Count; j++)
            {
                NodeBase node2 = node._nextNodeList[j];
                Rect nodeRect1 = Tools.GetNodeRect(CalcRealPosition(node._position));
                Vector2 pos1 = new Vector2(nodeRect1.max.x, nodeRect1.max.y - Tools._nodeHalfHeight);
                Vector2 pos2 = CalcRealPosition(new Vector2(node2._position.x, node2._position.y + Tools._nodeHalfHeight));
                Tools.DrawBazier(pos1, pos2);

                DrawBazierLine(node2);
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
                            _currentMission._position = CalcVirtualPosition(mousePos);
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
            GUI.Box(background, "", ResourcesManager.GetInstance.StyleBackground);

            _leftScrollPosition = GUI.BeginScrollView(new Rect(0, _window._topHeight, _window._leftWidth, _window._windowRect.height - _window._topHeight), _leftScrollPosition, new Rect(0, _window._topHeight, _window._leftWidth - 30, _window._windowRect.height - _window._topHeight), false, true, ResourcesManager.GetInstance.skin.horizontalScrollbar, ResourcesManager.GetInstance.skin.verticalScrollbar);
            //_leftScrollPosition = GUILayout.BeginScrollView(_leftScrollPosition, false, true, _window.skin.horizontalScrollbar, _window.skin.verticalScrollbar, _window.skin.GetStyle("Background"), GUILayout.Width(_window._leftWidth));

            float height = _window._topHeight + 5;
            GUILayout.Space(height);
            //for (int i = 0; i < 12; i++)
            //{
            //    GUILayout.Label("Name: ");
            //}
            if (_currentMission != null)
            {
                GUILayout.BeginHorizontal();
                EditorGUI.LabelField(new Rect(0, height, 50, 18), "Name: ");
                Rect editNameRect = new Rect(50, height, 150, 18);
                _currentMission._name = EditorGUI.TextField(editNameRect, _currentMission._name);
                GUILayout.EndHorizontal();

                if (Event.current != null)
                {
                    if (Event.current.button == 0)
                    {
                        //Debug.Log(Event.current.type);
                        if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.Ignore)
                        {
                            //Debug.Log(Event.current.type);
                            GUI.FocusControl("Name: ");
                        }
                    }
                }
            }
            GUI.EndScrollView();
            //GUILayout.EndScrollView();
        }

        private void DrawRightGrid()
        {
            Rect coord = new Rect();
            Vector2 nodepos = CalcVirtualPosition(Vector2.zero);
            coord.x = nodepos.x / 128.0f;
            coord.y = nodepos.y / 128.0f;
            coord.width = _contentRect.width / 128.0f;
            coord.height = -_contentRect.height / 128.0f;
            GUI.color = new Color(1f, 1f, 1f, 0.1f);
            GUI.DrawTextureWithTexCoords(_contentRect, ResourcesManager.GetInstance.texGrid, coord);
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
        Vector2 CalcVirtualPosition(Vector2 pos)
        {
            Vector2 center = _contentRect.size * 0.5f;
            center.x = (int)(center.x);
            center.y = (int)(center.y);

            return pos + _window._storyObject.StoryCenter - center;
        }

        Vector2 CalcRealPosition(Vector2 pos)
        {
            Vector2 center = _contentRect.size * 0.5f;
            center.x = (int)(center.x);
            center.y = (int)(center.y);

            return center + pos - _window._storyObject.StoryCenter;
        }

    }
}
