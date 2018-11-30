using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.LFramework.Components.Graph
{
    public class FunctionalGraph:MaskableGraphic
    {
        public FunctionalGraphBase GraphBase =new FunctionalGraphBase();
        public List<FunctionFormula> Formulas=new List<FunctionFormula>();
        private RectTransform _myRect;
        private Vector2 _xPoint;
        private Vector2 _yPoint;

        /// <summary>
        /// 绘制坐标轴上的文字
        /// </summary>
        private void OnGUI()
        {
            if (GraphBase.ShowXYAxisUnit)
            {
                Vector3 result = transform.localPosition;
                Vector3 realPosition = getScreenPosition(transform, ref result);
                GUIStyle guiStyleX = new GUIStyle();
                guiStyleX.normal.textColor = GraphBase.FontColor;
                guiStyleX.fontSize = GraphBase.FontSize;
                guiStyleX.fontStyle = FontStyle.Bold;
                guiStyleX.alignment = TextAnchor.MiddleLeft;
                GUI.Label(new Rect(local2Screen(realPosition, _xPoint) + new Vector2(20, 0), new Vector2(0, 0)), GraphBase.XAxisUnit, guiStyleX);

                GUIStyle guiStyleY = new GUIStyle();
                guiStyleY.normal.textColor = GraphBase.FontColor;
                guiStyleY.fontSize = GraphBase.FontSize;
                guiStyleY.fontStyle = FontStyle.Bold;
                guiStyleY.alignment = TextAnchor.MiddleCenter;
                GUI.Label(new Rect(local2Screen(realPosition, _yPoint) - new Vector2(0, 20), new Vector2(0, 0)), GraphBase.YAxisUnit, guiStyleY);
            }
        }


        /// <summary>
        /// 初始化函数信息,添加函数公式
        /// </summary>
        private void Init()
        {
            Formulas.Clear();
            _myRect = this.rectTransform;
            Formulas.Add(new FunctionFormula(Mathf.Sin,Color.red,3.0f));
            Formulas.Add(new FunctionFormula(Mathf.Sign,Color.blue,2.0f));
            Formulas.Add(new FunctionFormula(Mathf.Sqrt,Color.magenta,2.0f));
            Formulas.Add(new FunctionFormula(xValue=>xValue*1.3f+1,Color.yellow,2.0f));
        }

     
        /// <summary>
        /// 重写这个类以绘制UI,首先初始化数据和清空已有的顶点数据
        /// </summary>
        protected override void OnPopulateMesh(VertexHelper vh)
        {           
            vh.Clear();//清空顶点
            Init();
            #region 基础框架的绘制

            // 绘制X轴
            float lenght = _myRect.sizeDelta.x;
            Vector2 leftPoint = new Vector2(-lenght / 2.0f, 0);
            Vector2 rightPoint = new Vector2(lenght / 2.0f, 0);
            vh.AddUIVertexQuad(GetQuad(leftPoint, rightPoint, GraphBase.XYAxisColor, GraphBase.XYAxisWidth));
            // 绘制X轴的箭头
            float arrowUnit = GraphBase.XYAxisWidth * 3;
            Vector2 firstPointX = rightPoint + new Vector2(0, arrowUnit);
            Vector2 secondPointX = rightPoint;
            Vector2 thirdPointX = rightPoint + new Vector2(0, -arrowUnit);
            Vector2 fourPointX = rightPoint + new Vector2(Mathf.Sqrt(3) * arrowUnit, 0);
            vh.AddUIVertexQuad(GetQuad(firstPointX, secondPointX, thirdPointX, fourPointX, GraphBase.XYAxisColor));
            // 绘制Y轴
            float height = _myRect.sizeDelta.y;
            Vector2 downPoint = new Vector2(0, -height / 2.0f);
            Vector2 upPoint = new Vector2(0, height / 2.0f);
            vh.AddUIVertexQuad(GetQuad(downPoint, upPoint, GraphBase.XYAxisColor, GraphBase.XYAxisWidth));
            // 绘制Y轴的箭头
            Vector2 firstPointY = upPoint + new Vector2(arrowUnit, 0);
            Vector2 secondPointY = upPoint;
            Vector2 thirdPointY = upPoint + new Vector2(-arrowUnit, 0);
            Vector2 fourPointY = upPoint + new Vector2(0, Mathf.Sqrt(3) * arrowUnit);
            vh.AddUIVertexQuad(GetQuad(firstPointY, secondPointY, thirdPointY, fourPointY, GraphBase.XYAxisColor));

            if (GraphBase.ShowXYAxisUnit)
            {
                _xPoint = rightPoint;
                _yPoint = upPoint;
            }

            #region 刻度的绘制

            if (GraphBase.ShowScale)
            {
                // X 轴的正方向
                for (int i = 1; i * GraphBase.ScaleValue < _myRect.sizeDelta.x / 2.0f; i++)
                {
                    Vector2 firstPoint = Vector2.zero + new Vector2(GraphBase.ScaleValue * i, 0);
                    Vector2 secongPoint = firstPoint + new Vector2(0, GraphBase.ScaleLenght);
                    vh.AddUIVertexQuad(GetQuad(firstPoint, secongPoint, GraphBase.XYAxisColor));
                }
                // X 轴的负方向
                for (int i = 1; i * -GraphBase.ScaleValue > -_myRect.sizeDelta.x / 2.0f; i++)
                {
                    Vector2 firstPoint = Vector2.zero + new Vector2(-GraphBase.ScaleValue * i, 0);
                    Vector2 secongPoint = firstPoint + new Vector2(0, GraphBase.ScaleLenght);
                    vh.AddUIVertexQuad(GetQuad(firstPoint, secongPoint, GraphBase.XYAxisColor));
                }
                // Y 轴正方向
                for (int y = 1; y * GraphBase.ScaleValue < _myRect.sizeDelta.y / 2.0f; y++)
                {
                    Vector2 firstPoint = Vector2.zero + new Vector2(0, y * GraphBase.ScaleValue);
                    Vector2 secongPoint = firstPoint + new Vector2(GraphBase.ScaleLenght, 0);
                    vh.AddUIVertexQuad(GetQuad(firstPoint, secongPoint, GraphBase.XYAxisColor));
                }
                // Y 轴负方向
                for (int y = 1; y * -GraphBase.ScaleValue > -_myRect.sizeDelta.y / 2.0f; y++)
                {
                    Vector2 firstPoint = Vector2.zero + new Vector2(0, y * -GraphBase.ScaleValue);
                    Vector2 secongPoint = firstPoint + new Vector2(GraphBase.ScaleLenght, 0);
                    vh.AddUIVertexQuad(GetQuad(firstPoint, secongPoint, GraphBase.XYAxisColor));
                }
            }

            #endregion

            #region 根据网格类型绘制网格

            switch (GraphBase.MeshType)
            {
                case FunctionalGraphBase.E_MeshType.None:
                    break;
                case FunctionalGraphBase.E_MeshType.FullLine:
                    // X 轴的正方向
                    for (int i = 1; i * GraphBase.ScaleValue < _myRect.sizeDelta.x / 2.0f; i++)
                    {
                        Vector2 firstPoint = Vector2.zero + new Vector2(GraphBase.ScaleValue * i, -_myRect.sizeDelta.y / 2.0f);
                        Vector2 secongPoint = firstPoint + new Vector2(0, _myRect.sizeDelta.y);
                        vh.AddUIVertexQuad(GetQuad(firstPoint, secongPoint, GraphBase.MeshColor, GraphBase.MeshLineWidth));
                    }
                    // X 轴的负方向
                    for (int i = 1; i * -GraphBase.ScaleValue > -_myRect.sizeDelta.x / 2.0f; i++)
                    {
                        Vector2 firstPoint = Vector2.zero + new Vector2(-GraphBase.ScaleValue * i, -_myRect.sizeDelta.y / 2.0f);
                        Vector2 secongPoint = firstPoint + new Vector2(0, _myRect.sizeDelta.y);
                        vh.AddUIVertexQuad(GetQuad(firstPoint, secongPoint, GraphBase.MeshColor, GraphBase.MeshLineWidth));
                    }
                    // Y 轴正方向
                    for (int y = 1; y * GraphBase.ScaleValue < _myRect.sizeDelta.y / 2.0f; y++)
                    {
                        Vector2 firstPoint = Vector2.zero + new Vector2(-_myRect.sizeDelta.x / 2.0f, y * GraphBase.ScaleValue);
                        Vector2 secongPoint = firstPoint + new Vector2(_myRect.sizeDelta.x, 0);
                        vh.AddUIVertexQuad(GetQuad(firstPoint, secongPoint, GraphBase.MeshColor, GraphBase.MeshLineWidth));
                    }
                    // Y 轴负方向
                    for (int y = 1; y * -GraphBase.ScaleValue > -_myRect.sizeDelta.y / 2.0f; y++)
                    {
                        Vector2 firstPoint = Vector2.zero + new Vector2(-_myRect.sizeDelta.x / 2.0f, -y * GraphBase.ScaleValue);
                        Vector2 secongPoint = firstPoint + new Vector2(_myRect.sizeDelta.x, 0);
                        vh.AddUIVertexQuad(GetQuad(firstPoint, secongPoint, GraphBase.MeshColor, GraphBase.MeshLineWidth));
                    }
                    break;
                case FunctionalGraphBase.E_MeshType.ImaglinaryLine:
                    // X 轴的正方向
                    for (int i = 1; i * GraphBase.ScaleValue < _myRect.sizeDelta.x / 2.0f; i++)
                    {
                        Vector2 firstPoint = Vector2.zero + new Vector2(GraphBase.ScaleValue * i, -_myRect.sizeDelta.y / 2.0f);
                        Vector2 secondPoint = firstPoint + new Vector2(0, _myRect.sizeDelta.y);
                        GetImaglinaryLine(ref vh, firstPoint, secondPoint, GraphBase.MeshColor, GraphBase.ImaglinaryLineWidth, GraphBase.SpaceingWidth);
                    }
                    // X 轴的负方向
                    for (int i = 1; i * -GraphBase.ScaleValue > -_myRect.sizeDelta.x / 2.0f; i++)
                    {
                        Vector2 firstPoint = Vector2.zero + new Vector2(-GraphBase.ScaleValue * i, -_myRect.sizeDelta.y / 2.0f);
                        Vector2 secondPoint = firstPoint + new Vector2(0, _myRect.sizeDelta.y);
                        GetImaglinaryLine(ref vh, firstPoint, secondPoint, GraphBase.MeshColor, GraphBase.ImaglinaryLineWidth, GraphBase.SpaceingWidth);
                    }
                    // Y 轴正方向
                    for (int y = 1; y * GraphBase.ScaleValue < _myRect.sizeDelta.y / 2.0f; y++)
                    {
                        Vector2 firstPoint = Vector2.zero + new Vector2(-_myRect.sizeDelta.x / 2.0f, y * GraphBase.ScaleValue);
                        Vector2 secondPoint = firstPoint + new Vector2(_myRect.sizeDelta.x, 0);
                        GetImaglinaryLine(ref vh, firstPoint, secondPoint, GraphBase.MeshColor, GraphBase.ImaglinaryLineWidth, GraphBase.SpaceingWidth);
                    }
                    // Y 轴负方向
                    for (int y = 1; y * -GraphBase.ScaleValue > -_myRect.sizeDelta.y / 2.0f; y++)
                    {
                        Vector2 firstPoint = Vector2.zero + new Vector2(-_myRect.sizeDelta.x / 2.0f, -y * GraphBase.ScaleValue);
                        Vector2 secondPoint = firstPoint + new Vector2(_myRect.sizeDelta.x, 0);
                        GetImaglinaryLine(ref vh, firstPoint, secondPoint, GraphBase.MeshColor, GraphBase.ImaglinaryLineWidth, GraphBase.SpaceingWidth);
                    }
                    break;
            }

            #endregion

            #endregion

            #region 函数图的绘制
            float unitPixel = 100 / GraphBase.ScaleValue;
            //遍历函数公式，然后每隔一次像素绘制一个矩形
            foreach (var functionFormula in Formulas)
            {
                Vector2 startPos = GetFormulaPoint(functionFormula.Formula, -_myRect.sizeDelta.x / 2.0f);
                //从X轴的负方向轴开始向正方向轴绘制
                for (float x = -_myRect.sizeDelta.x / 2.0f + 1; x < _myRect.sizeDelta.x / 2.0f; x+=unitPixel)
                {
                    Vector2 endPos = GetFormulaPoint(functionFormula.Formula, x);
                    vh.AddUIVertexQuad(GetQuad(startPos, endPos, functionFormula.FormulaColor, functionFormula.FormulaWidth));
                    //这里把当前绘制的结束点设置为下一次绘制的起始点
                    startPos = endPos;
                }
            }

            #endregion
        }

        //利用Func委托，计算出每一个绘制点
        private Vector2 GetFormulaPoint(Func<float, float> formula, float xValue)
        {
            return new Vector2(xValue, formula(xValue / GraphBase.ScaleValue) * GraphBase.ScaleValue);
        }


        // 通过两个端点绘制矩形
        private UIVertex[] GetQuad(Vector2 startPos, Vector2 endPos, Color color0, float lineWidth = 2.0f)
        {
            float dis = Vector2.Distance(startPos, endPos);
            float y = lineWidth*0.5f*(endPos.x - startPos.x)/dis;
            float x = lineWidth*0.5f*(endPos.y - startPos.y)/dis;
            if (y <= 0) y = -y;
            else x = -x;
            UIVertex[] vertices=new UIVertex[4];
            vertices[0].position=new Vector3(startPos.x+x,startPos.y+y);
            vertices[1].position=new Vector3(endPos.x+x,endPos.y+y);
            vertices[2].position=new Vector3(endPos.x-x,endPos.y-y);
            vertices[3].position=new Vector3(startPos.x-x,startPos.y-y);
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].color = color0;
            }
            return vertices;
        }

        //通过四个顶点绘制矩形
        private UIVertex[] GetQuad(Vector2 first, Vector2 second, Vector2 third, Vector2 four, Color color0)
        {
            UIVertex[] vertices=new UIVertex[4];
            vertices[0] = GetUIVertex(first, color0);
            vertices[1] = GetUIVertex(second, color0);
            vertices[2] = GetUIVertex(third, color0);
            vertices[3] = GetUIVertex(four, color0);
            return vertices;
        }

        //构造UIVertex,UI顶点
        private UIVertex GetUIVertex(Vector2 point, Color color0)
        {
            UIVertex vertex = new UIVertex
            {
                position = point,
                color = color0,
                uv0 = new Vector2(0, 0)
            };
            return vertex;
        }

        private void GetImaglinaryLine(ref VertexHelper vh, Vector2 first, Vector2 second, Color color0,
            float imaginaryLength,float spaceingWidth,float lineWidth=2.0f)
        {
            if (first.y.Equals(second.y))//x轴
            {
                Vector2 indexSecond = first + new Vector2(imaginaryLength, 0);
                while (indexSecond.x<second.x)
                {
                    vh.AddUIVertexQuad(GetQuad(first,indexSecond,color0));
                    first = indexSecond + new Vector2(spaceingWidth, 0);
                    indexSecond = first + new Vector2(imaginaryLength, 0);
                    if (indexSecond.x>second.x)
                    {
                        indexSecond=new Vector2(second.x,indexSecond.y);
                        vh.AddUIVertexQuad(GetQuad(first,indexSecond,color0));
                    }
                }
            }
            if (first.x.Equals(second.y))//y轴
            {
                Vector2 indexSecond = first + new Vector2(0, imaginaryLength);
                while (indexSecond.y<second.y)
                {
                    vh.AddUIVertexQuad(GetQuad(first,indexSecond,color0));
                    first = indexSecond + new Vector2(0, spaceingWidth);
                    indexSecond = first + new Vector2(0, imaginaryLength);
                    if (indexSecond.y>second.y)
                    {
                        indexSecond=new Vector2(indexSecond.x,second.y);
                        vh.AddUIVertexQuad(GetQuad(first,indexSecond,color0));
                    }
                }
            }
        }

        //本地坐标转化为屏幕坐标绘制GUI文字
        private Vector2 local2Screen(Vector2 parentPos, Vector2 localPos)
        {
            Vector2 pos = localPos + parentPos;
            float xValue, yValue = 0;
            if (pos.x > 0)
                xValue = pos.x + Screen.width/2.0f;
            else
                xValue = Screen.width/2.0f - Mathf.Abs(pos.x);
            if (pos.y > 0)
                yValue = Screen.height/2.0f - pos.y;
            else
                yValue = Screen.height/2.0f + Mathf.Abs(pos.y);
            return new Vector2(xValue,yValue);
        }

        //递归计算
        private Vector2 getScreenPosition(Transform trans, ref Vector3 result)
        {
            if (null!=trans.parent&&null!=trans.parent.parent)
            {
                result += trans.parent.localPosition;
                getScreenPosition(trans.parent, ref result);
            }
            if (null!=trans.parent&&null==trans.parent.parent)
            {
                return result;
            }
            return result;
        }
    }
}