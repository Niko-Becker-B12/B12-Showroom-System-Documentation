using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThisOtherThing.UI.Shapes;
using UnityEngine.UI;
using DG.Tweening;

namespace Showroom
{

    public class ClickMeVFX : MonoBehaviour
    {

        RectTransform VFXEllipse;
        Ellipse VFXEllipseImage;

        private void OnEnable()
        {

            VFXEllipse = this.GetComponent<RectTransform>();
            VFXEllipseImage = this.GetComponent<Ellipse>();

            Invoke("StartEffect", Random.Range(5, 10));

        }


        void StartEffect()
        {

            float startTiming = Random.Range(ShowroomManager.Instance.clickMeVFXRandomTime.x, ShowroomManager.Instance.clickMeVFXRandomTime.y);

            VFXEllipse.DOSizeDelta(new Vector2(6f, 48f), 0.01f).OnComplete(
                () => 
                { 
                    VFXEllipseImage.ShapeProperties.OutlineColor.a = (byte)0f; 
                });


            VFXEllipse.DOSizeDelta(new Vector2(48f, 48f), 5f).SetDelay(startTiming).OnStart(EnableAlpha);

            float a = 255f;
            DOTween.To(() => a, x => a = x, 0, 1).SetDelay(4f + startTiming).OnUpdate(() => UpdateAlpha(a)).OnComplete(StartEffect);

        }

        void UpdateAlpha(float alpha)
        {


            VFXEllipseImage.ShapeProperties.OutlineColor.a = (byte)alpha;

        }

        void EnableAlpha()
        {

            VFXEllipseImage.ShapeProperties.OutlineColor.a = (byte)128f;

        }

    }

}