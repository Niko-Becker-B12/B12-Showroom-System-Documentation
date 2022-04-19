using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoTweenFix : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine ("ActiveDeactive_DoTween");
    }

    IEnumerator ActiveDeactive_DoTween () {
        GameObject DoTween = GameObject.Find ("[DOTween]");
        DoTween.SetActive (false);
        yield return new WaitForSeconds (0.1f);
        DoTween.SetActive (true);
    }
}
