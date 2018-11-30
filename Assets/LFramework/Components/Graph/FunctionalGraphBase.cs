using System;
using UnityEngine;

namespace Assets.LFramework.Components.Graph
{
    [Serializable]
    public class FunctionalGraphBase
    {
        /// <summary>
        /// 是否显示刻度
        /// </summary>
        public bool ShowScale = false;
        /// <summary>
        /// 是否显示XY轴单位
        /// </summary>
        public bool ShowXYAxisUnit = true;
        /// <summary>
        /// X轴单位
        /// </summary>
        public string XAxisUnit = "X";
        /// <summary>
        /// Y轴单位
        /// </summary>
        public string YAxisUnit = "Y";
        /// <summary>
        /// 单位字体大小
        /// </summary>
        [Range(12, 30)]
        public int FontSize = 16;
        /// <summary>
        /// 字体颜色
        /// </summary>
        public Color FontColor = Color.black;
        /// <summary>
        /// XY轴刻度
        /// </summary>
        [Range(20f, 100f)]
        public float ScaleValue = 50f;
        /// <summary>
        /// 刻度的长度
        /// </summary>
        [Range(2, 30)]
        public float ScaleLenght = 5.0f;
        /// <summary>
        /// XY轴宽度
        /// </summary>
        [Range(2f, 20f)]
        public float XYAxisWidth = 2.0f;
        /// <summary>
        /// XY轴颜色
        /// </summary>
        public Color XYAxisColor = Color.gray;
        /// <summary>
        /// 网格Enum
        /// </summary>
        public enum E_MeshType
        {
            None,
            FullLine,
            ImaglinaryLine
        }
        /// <summary>
        /// 网格类型
        /// </summary>
        public E_MeshType MeshType = E_MeshType.None;
        /// <summary>
        /// 网格线段宽度
        /// </summary>
        [Range(1.0f, 10f)]
        public float MeshLineWidth = 2.0f;
        /// <summary>
        /// 网格颜色
        /// </summary>
        public Color MeshColor = Color.gray;
        /// <summary>
        /// 虚线的长度
        /// </summary>
        [Range(0.5f, 20)]
        public float ImaglinaryLineWidth = 8.0f;
        /// <summary>
        /// 虚线空格长度
        /// </summary>
        [Range(0.5f, 10f)]
        public float SpaceingWidth = 5.0f;
    }
}