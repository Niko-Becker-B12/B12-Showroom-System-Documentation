using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Showroom
{

    public class ShowroomLineEffectEditor : MonoBehaviour
    {

        public GameObject linePrefab;

        public List<Transform> lineTransforms = new List<Transform>();

        private List<Line> lineObjects = new List<Line>();

        public string animPath;

        [SerializeField] private Transform lineParent;

#if UNITY_EDITOR
        [Button()]
        public void GenerateLines()
        {
            if (GameObject.Find(this.transform.name + "/- Line Parent -"))
                lineParent = GameObject.Find(this.transform.name + "/- Line Parent -").transform;

            if (lineParent == null)
            {

                GameObject newTransformObj = new GameObject();
                lineParent = newTransformObj.transform;
                lineParent.parent = this.transform;

                lineParent.name = "- Line Parent -";
            }
            else
            {

                DestroyImmediate(lineParent.gameObject);

                GameObject newTransformObj = new GameObject();
                lineParent = newTransformObj.transform;
                lineParent.parent = this.transform;

                lineParent.name = "- Line Parent -";
            }


            lineObjects.Clear();


            foreach (Transform child in lineParent)
            {

                DestroyImmediate(child.gameObject);

            }

            for (int i = 0; i < lineTransforms.Count - 1; i++)
            {
                Debug.Log("Index: " + i);
                string[] s = lineTransforms[i].name.Split(char.Parse("_"));

                int currentCurveIndex = int.Parse(s[1]);

                string[] s2 = lineTransforms[i + 1].name.Split(char.Parse("_"));

                int nextCurveIndex = int.Parse(s2[1]);




                if (s[0] == "ST_" || s[0] == "ST")
                {
                    Debug.Log(currentCurveIndex + " | " + nextCurveIndex + " | S[2]: " + s[2]);

                    if (currentCurveIndex == nextCurveIndex || int.Parse(s[2]) == 1)
                    {
                        Debug.Log("Test");
                        GameObject newLineObj = (GameObject)PrefabUtility.InstantiatePrefab(linePrefab, lineParent);
                        Line newLine = newLineObj.GetComponent<Line>();

                        newLine.Start = lineTransforms[i].position;
                        newLine.End = lineTransforms[i + 1].position;

                        newLineObj.name = "LineObj_ST_" + i;

                        lineObjects.Add(newLine);
                    }
                    else
                    {
                        if (i < lineTransforms.Count - 1)
                            continue;
                    }

                }
                else
                {

                    GameObject newLineObj = (GameObject)PrefabUtility.InstantiatePrefab(linePrefab, lineParent);
                    Line newLine = newLineObj.GetComponent<Line>();

                    newLine.Start = lineTransforms[i].position;
                    newLine.End = lineTransforms[i + 1].position;

                    newLineObj.name = "LineObj_" + i;

                    lineObjects.Add(newLine);

                    i++;

                }


            }

            GenerateNewAnim();

        }

        void GenerateNewAnim()
        {

            AnimationClip clip = new AnimationClip();
            clip.frameRate = 60f;

            for (int i = 0; i < lineObjects.Count; i++)
            {

                string[] s = lineObjects[i].name.Split(char.Parse("_"));

                if (s[1] == "ST_" || s[1] == "ST")
                {

                    string path = AnimationUtility.CalculateTransformPath(lineObjects[i].transform, transform);
                    Debug.Log("Object Path: " + path);

                    Vector3 dir = lineObjects[i].Start - lineObjects[i].End;
                    Debug.Log("Direction: " + dir);

                    AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve(path, typeof(Line), "color.a"), AnimationCurve.Linear(.35f, 0, .4f, linePrefab.GetComponent<Line>().Color.a));

                }
                else
                {

                    string path = AnimationUtility.CalculateTransformPath(lineObjects[i].transform, transform);
                    Debug.Log("Object Path: " + path);

                    Vector3 dir = lineObjects[i].Start - lineObjects[i].End;
                    Debug.Log("Direction: " + dir);


                    //PosX in Seconds, Value, PosX2 in Seconds, Value2

                    AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve(path, typeof(Transform), "m_LocalPosition.x"), AnimationCurve.Linear(0, -dir.x, .4f, 0));
                    AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve(path, typeof(Transform), "m_LocalPosition.y"), AnimationCurve.Linear(0, -dir.y, .4f, 0));
                    AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve(path, typeof(Transform), "m_LocalPosition.z"), AnimationCurve.Linear(0, -dir.z, .4f, 0));

                    //Start gets animated, End stays the same -> Start starts at the End position

                    AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve(path, typeof(Line), "start.x"), AnimationCurve.Linear(0, lineObjects[i].End.x, .4f, lineObjects[i].Start.x));
                    AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve(path, typeof(Line), "start.y"), AnimationCurve.Linear(0, lineObjects[i].End.y, .4f, lineObjects[i].Start.y));
                    AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve(path, typeof(Line), "start.z"), AnimationCurve.Linear(0, lineObjects[i].End.z, .4f, lineObjects[i].Start.z));

                    AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve(path, typeof(Line), "end.x"), AnimationCurve.Linear(0, lineObjects[i].End.x, .4f, lineObjects[i].End.x));
                    AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve(path, typeof(Line), "end.y"), AnimationCurve.Linear(0, lineObjects[i].End.y, .4f, lineObjects[i].End.y));
                    AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve(path, typeof(Line), "end.z"), AnimationCurve.Linear(0, lineObjects[i].End.z, .4f, lineObjects[i].End.z));

                }

            }




            AnimationClip oldClip = AssetDatabase.LoadAssetAtPath(animPath, typeof(AnimationClip)) as AnimationClip;

            if (oldClip == null)
            {
                AssetDatabase.CreateAsset(clip, animPath + "Anim_" + this.transform.parent.name + "_Lines" + ".anim");
                AssetDatabase.Refresh();
            }
            else
            {
                AssetDatabase.DeleteAsset(animPath + "Anim_" + this.transform.parent.name + "_Lines" + ".anim");
                AssetDatabase.Refresh();
                AssetDatabase.CreateAsset(clip, animPath + "Anim_" + this.transform.parent.name + "_Lines" + ".anim");
            }



        }
#endif


    }

}