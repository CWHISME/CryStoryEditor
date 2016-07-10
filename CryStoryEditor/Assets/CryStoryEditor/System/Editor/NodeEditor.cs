/**********************************************************
*Author: wangjiaying
*Date: 2016.7.6
*Func:
**********************************************************/
using UnityEngine;
using CryStory.Runtime;
using Event = UnityEngine.Event;
using System.Collections.Generic;
using UnityEditor;

namespace CryStory.Editor
{
    public class NodeEditor<T> : Singleton<T> where T : class, new()
    {

        protected StoryEditorWindow _window;
        protected Rect _contentRect;

        protected Vector2 _leftScrollPosition;

        protected Vector2 _mouseDownPos = Vector2.zero;
        protected bool _mouseIsDown = false;
        protected Vector2 _mouseDownCenter = Vector2.zero;

        protected NodeModifier _currentNode;
        protected Rect _currentNodeRect;
        protected NodeModifier _currentHover;
        private bool _isConnecting = false;
        public void OnGUI(StoryEditorWindow window, NodeModifier[] nodes)
        {
            _contentRect = window._contentRect;
            _window = window;

            ShowGraphCenterInfo();

            //========Right Area===============
            DrawRightGrid();
            DrawNodes(nodes);
            ShowConnectLine();
            //========Left Slider Area===========
            ShowLeftSliderArea();
            //Make graph dragable
            DragGraph();

            InternalOnGUI();
        }

        private void ShowConnectLine()
        {
            if (_isConnecting && _currentNode != null)
            {
                Vector2 pos1 = new Vector2(_currentNodeRect.max.x, _currentNodeRect.max.y - Tools._nodeHalfHeight);
                Tools.DrawBazier(pos1, Event.current.mousePosition);
                if (Tools.MouseUp)
                {
                    _isConnecting = false;
                    if (_currentNode == _currentHover) return;
                    if (Tools.IsValidMouseAABB(Tools.GetNodeRect(CalcRealPosition(_currentHover._position))))
                    {
                        if (_currentHover.SetParent(_currentNode))
                            OnLinkNode(_currentNode, _currentHover);
                        else EditorUtility.DisplayDialog("Error", "Not allow connect to twice parent! You must break one connect with parent and then change it.", "OK");
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

        private void DrawNodes(NodeModifier[] nodeList)
        {
            for (int i = 0; i < nodeList.Length; i++)
            {
                NodeModifier node = nodeList[i];
                Rect nodeRect = DrawNodeRect(node);

                //Draw conection bazier line
                DrawBazierLine(node);
                //Draw Node
                DrawNodeSlot(node, nodeRect);

                DrawNodes(node.GetNextNodes(node));

                if (_isConnecting) continue;

                DragNodeEvent(node, nodeRect);
            }
        }

        private void DragNodeEvent(NodeModifier node, Rect nodeRect)
        {
            #region Drag Event
            if (Event.current != null)
            {
                if (Event.current.button == 0)
                {
                    if (Event.current.type == EventType.MouseDown && Tools.IsValidMouseAABB(nodeRect))
                    {
                        _mouseIsDown = true;
                        _mouseDownPos = Event.current.mousePosition;
                        _mouseDownCenter = node._position;
                        _currentNode = node;
                        _currentNodeRect = nodeRect;
                    }

                    if (Event.current.type == EventType.MouseUp)
                    {
                        _mouseIsDown = false;
                    }

                    if (Event.current.type == EventType.MouseDrag && Event.current.button == 0 && _mouseIsDown)
                    {
                        //Debug.Log(Event.current.button);
                        Vector2 offset = Event.current.mousePosition - _mouseDownPos;

                        _currentNode._position = _mouseDownCenter + offset;

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

        private void ShowLeftSliderArea()
        {
            Rect background = new Rect(0, _window._topHeight, _window._leftWidth, _window._windowRect.height);
            GUI.Box(background, "", ResourcesManager.GetInstance.StyleBackground);

            _leftScrollPosition = GUI.BeginScrollView(new Rect(0, _window._topHeight, _window._leftWidth, _window._windowRect.height - _window._topHeight), _leftScrollPosition, new Rect(0, _window._topHeight, _window._leftWidth - 30, _window._windowRect.height - _window._topHeight), false, true, ResourcesManager.GetInstance.skin.horizontalScrollbar, ResourcesManager.GetInstance.skin.verticalScrollbar);
            //_leftScrollPosition = GUILayout.BeginScrollView(_leftScrollPosition, false, true, _window.skin.horizontalScrollbar, _window.skin.verticalScrollbar, _window.skin.GetStyle("Background"), GUILayout.Width(_window._leftWidth));

            //Reset Left Scroll Area Height
            _heightLeft = _window._topHeight + 5;
            //GUILayout.Space(height);
            //for (int i = 0; i < 12; i++)
            //{
            //    GUILayout.Label("Name: ");
            //}
            if (_currentNode != null)
            {
                //EditorGUILayout.BeginHorizontal();
                #region Modify Name
                EditorGUI.LabelField(GetGUILeftScrollAreaRect(50, 18, false), "Name: ");
                Rect editNameRect = GetGUILeftScrollAreaRect(50, 150, 18);// new Rect(50, _heightLeft, 150, 18);
                //LeftHeightSpace(18f);

                string oldName = _currentNode._name;
                EditorGUI.BeginChangeCheck();
                _currentNode._name = EditorGUI.TextField(editNameRect, _currentNode._name);
                if (EditorGUI.EndChangeCheck())
                {
                    OnNodeNameChange(_currentNode, oldName);
                }
                //EditorGUILayout.EndHorizontal();

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
                #endregion

                #region Next Nodes
                //GUILayout.Space(20);
                NodeModifier[] nodes = _currentNode.NextNodes;
                if (nodes.Length > 0)
                {
                    LeftHeightSpace(5);
                    GUIStyle style = new GUIStyle();
                    style.normal.textColor = Color.white;
                    style.fontSize = 14;
                    EditorGUI.LabelField(GetGUILeftScrollAreaRect(_window._leftWidth, 30), "Next Nodes:", style);
                    for (int i = 0; i < nodes.Length; i++)
                    {
                        //EditorGUILayout.BeginHorizontal();
                        Rect rect = GetGUILeftScrollAreaRect(_window._leftWidth, 20, false);
                        EditorGUI.LabelField(rect, nodes[i]._name);
                        //Rect rect = EditorGUILayout.GetControlRect(false, 20);
                        //rect.width = 80f;
                        if (GUI.Button(GetGUILeftScrollAreaRect(_window._leftWidth - 68f, 50f, 20f), "<color=red>Delete</color>", ResourcesManager.GetInstance.skin.button))
                        {
                            _currentNode.Remove(nodes[i]);
                            break;
                        }
                        //EditorGUILayout.EndHorizontal();
                    }
                }
                #endregion
            }


            GUI.EndScrollView();
            //GUILayout.EndScrollView();
        }

        private float _heightLeft;
        private Rect GetGUILeftScrollAreaRect(float width, float height, bool addHeight = true)
        {
            Rect rect = new Rect(0, _heightLeft, width, height);
            if (addHeight)
                _heightLeft += height;
            return rect;
        }

        private Rect GetGUILeftScrollAreaRect(float leftPos, float width, float height, bool addHeight = true)
        {
            Rect rect = new Rect(leftPos, _heightLeft, width, height);
            if (addHeight)
                _heightLeft += height;
            return rect;
        }

        private void LeftHeightSpace(float height)
        {
            _heightLeft += height;
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


        //Virtual Method===========================

        protected virtual void InternalOnGUI() { }

        protected virtual void OnLinkNode(NodeModifier parent, NodeModifier child) { }

        protected virtual void OnNodeNameChange(NodeModifier node, string oldName) { }

        protected virtual void DrawBazierLine(NodeModifier node)
        {
            NodeModifier[] nextNodes = node.NextNodes;
            for (int j = 0; j < nextNodes.Length; j++)
            {
                NodeModifier node2 = nextNodes[j];
                Rect nodeRect1 = Tools.GetNodeRect(CalcRealPosition(node._position));
                Vector2 pos1 = new Vector2(nodeRect1.max.x, nodeRect1.max.y - Tools._nodeHalfHeight);
                Vector2 pos2 = CalcRealPosition(new Vector2(node2._position.x, node2._position.y + Tools._nodeHalfHeight));

                bool isParent = node.IsParent(node2);
                if (isParent)
                    Tools.DrawBazier(pos1, pos2, Color.green);
                else Tools.DrawBazier(pos1, pos2);

                //if (!isParent)
                //    DrawBazierLine(node2);
            }
        }

        protected virtual Rect DrawNodeRect(NodeModifier node)
        {
            Vector2 pos = CalcRealPosition(node._position);
            Rect nodeRect = Tools.GetNodeRect(pos);

            GUI.Box(nodeRect, node._name, _currentNode == node ? ResourcesManager.GetInstance.MissionNodeOn : ResourcesManager.GetInstance.MissionNode);

            return nodeRect;
        }

        protected virtual void DrawNodeSlot(NodeModifier node, Rect nodeRect)
        {
            #region Event drag connection line
            if (Event.current != null)
            {
                if (Tools.IsValidMouseAABB(nodeRect))
                {
                    _currentHover = node;

                    Rect leftRect = Tools.CalcLeftLinkRect(nodeRect);
                    Rect rightRect = Tools.CalcRightLinkRect(nodeRect);
                    GUIStyle inStyle = new GUIStyle();
                    GUIStyle outStyle = new GUIStyle();
                    inStyle.alignment = TextAnchor.MiddleLeft;
                    outStyle.alignment = TextAnchor.MiddleRight;

                    GUI.Box(leftRect, node.Parent == null ? ResourcesManager.GetInstance.texInputSlot : ResourcesManager.GetInstance.texInputSlotActive, inStyle);
                    GUI.Box(rightRect, node.NextNodes.Length < 1 ? ResourcesManager.GetInstance.texOutputSlot : ResourcesManager.GetInstance.texOutputSlotActive, outStyle);

                    //Connect
                    if (Event.current.type == EventType.MouseDown && Tools.IsValidMouseAABB(rightRect))
                    {
                        _isConnecting = true;
                        _currentNode = node;
                        _currentNodeRect = nodeRect;
                    }
                    //ReConnect
                    if (Event.current.type == EventType.MouseDown && Tools.IsValidMouseAABB(leftRect))
                    {
                        if (node.HaveParent)
                        {
                            _isConnecting = true;
                            _currentNode = node.Parent as Mission;
                            Vector2 realPos = CalcRealPosition(_currentNode._position);
                            _currentNodeRect = Tools.GetNodeRect(realPos);
                            node.DeleteParent();
                        }
                    }
                }
            }
            #endregion
        }

        //Tool====================================
        protected Vector2 CalcVirtualPosition(Vector2 pos)
        {
            Vector2 center = _contentRect.size * 0.5f;
            center.x = (int)(center.x);
            center.y = (int)(center.y);

            return pos + _window._storyObject.StoryCenter - center;
        }

        protected Vector2 CalcRealPosition(Vector2 pos)
        {
            Vector2 center = _contentRect.size * 0.5f;
            center.x = (int)(center.x);
            center.y = (int)(center.y);

            return center + pos - _window._storyObject.StoryCenter;
        }
    }
}