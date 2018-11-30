using Assets.LFramework.Core;
using Assets.LFramework.Manager;
using Assets.LFramework.UI;
using Assets.LFramework.Utility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.LFramework.Components.Chart
{
    public class DrawingItem:UIBase,IPointerEnterHandler,IPointerExitHandler
    {
        public Transform Content;
        public Image DrawingImage;
        public ScrollRect ScrollView;
        private bool changeDrwaingScale;
        private float drawScale=1.0f;

        public override void InitOnAwake()
        {
            Util.AutoFindComponent(transform,this);
        }

        /// <summary>
        /// 初始化接口
        /// </summary>
        /// <param name="url">图纸的url</param>
        public void Init(string url)
        {
            WWWLoadManager loader = AppFacade.Instance().WWWLoadManager;
            loader.StartLoadRes(url,this, bytes =>
            {
                DrawingImage.sprite = Util.GetSprite(bytes);
            });
            Reset();            
        }

        private void Reset()
        {
            DrawingImage.transform.localScale = Vector3.one;
            drawScale = 1;
            ScrollView.horizontalNormalizedPosition = 0.5f;
            ScrollView.verticalNormalizedPosition = 0.5f;
        }

        protected override void Update()
        {
            if (changeDrwaingScale)
            {
                drawScale += Input.mouseScrollDelta.y*0.1f;
                drawScale = Mathf.Clamp(drawScale, 0.5f, 2.0f);
                DrawingImage.transform.localScale = Vector3.one*drawScale;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            changeDrwaingScale = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            changeDrwaingScale = false;
        }
    }
}