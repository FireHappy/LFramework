using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.LFramework.Components.Chart    
{

    public class PieData
    {
        public float Percent;
        public Color Color;
        public PieData(){}

        public PieData(float percent, Color color0)
        {
            Percent = percent/100.0f;
            Color = color0;
        }

        public PieData(float percent)
        {
            Percent = percent/100.0f;
            // auto set color by percent
            Color = Color.white*Percent;
        }               
    }

    // Use this data struct to draw a Pie Graph
    [Serializable]
    public class Pies
    {
        public IList<PieData> PieDatas = new List<PieData>();
        public Pies() { }

        public Pies(IList<PieData> pieDatas)
        {
            PieDatas = pieDatas;
        }

        public void AddPieData(Pies piedatas)
        {
            foreach (PieData pieData in piedatas.PieDatas)
                PieDatas.Add(pieData);
        }
    }

    //GUI Text infomation for pie Graph
    [Serializable]
    public class PieText
    {
        public string Content = null;
        public Vector2 Position;
        public bool IsLeft = true;

        public PieText(string content, Vector2 position, bool isLeft)
        {
            Content = content;
            Position = position;
            IsLeft = isLeft;
        }
    }

    //core class
    public class PieChart : MaskableGraphic
    {
        [Header("Pie Bse Setting")]
        [Range(5,100)]public float PieRadius = 60.0f;
        [Range(0,120)]public float HollowWidth = 0.0f;
        [Range(0,15)]public float BoomDegree = 1.5f;
        [Range(20,200)]public float Smooth = 100;

        [Header("Percent Text Setting")] 
        public bool IsShowPercent = true;
        [Range(0.5f, 4)] public float BrokenLineWidth = 2;

        private Pies pieDate=new Pies();
        private List<PieText> pieTexts = null;
        private Vector3 realPosition;

        public void Inject(IList<float> percents, IList<Color> colors)
        {
            IList<PieData>pieDatas=new List<PieData>();
            for (int i = 0; i < percents.Count; i++)
            {
                pieDatas.Add(new PieData(percents[i],colors[i]));
            }
            Inject(pieDatas);
        }

        public void Inject(IList<float> percents)
        {
            IList<PieData>pieDatas=new List<PieData>();
            for (int i = 0; i < percents.Count; i++)
            {
                pieDatas.Add(new PieData(percents[i]));
            }
            Inject(pieDatas);
        }

        public void Inject(IList<PieData> pieDatas)
        {
            Inject(new Pies(pieDatas));
        }

        public void Inject(Pies pies)
        {
            pieDate.AddPieData(pies);
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            base.OnPopulateMesh(vh);
        }
    }
}