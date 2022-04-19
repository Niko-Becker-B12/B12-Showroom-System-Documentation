using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Showroom
{
    [System.Serializable]
    [Sirenix.OdinInspector.InlineEditor]
    [CreateAssetMenu(fileName ="Bullet Point Data", menuName ="Showroom/Bullet Point Data")]
    public class BulletPoint : ScriptableObject
    {

        [NaughtyAttributes.ResizableTextArea] public string bulletPointTitle;
        [NaughtyAttributes.ResizableTextArea] public string bulletPointSubTitle;
        [NaughtyAttributes.ResizableTextArea] public string bulletPointText;

    }

}