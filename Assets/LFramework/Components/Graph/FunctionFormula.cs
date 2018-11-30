using System;
using UnityEngine;

namespace Assets.LFramework.Components.Graph
{
    /// <summary>
    /// 函数公式
    /// </summary>
    [Serializable]
    public class FunctionFormula
    {
        /// <summary>
        /// 函数表达式
        /// </summary>
        public Func<float, float> Formula;
        /// <summary>
        /// 函数图对应的线条颜色
        /// </summary>
        public Color FormulaColor;

        public float FormulaWidth;

        public FunctionFormula(){}

        public FunctionFormula(Func<float, float> formula, Color formulaColor, float width)
        {
            Formula = formula;
            FormulaColor = formulaColor;
            FormulaWidth = width;
        }      
    }
}