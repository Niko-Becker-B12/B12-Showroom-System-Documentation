using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

namespace Showroom
{

    public class ShowroomLoadingScreen : MonoBehaviour
    {

        public CanvasGroup loadingScreenCanvasGroup;

        public Camera loadingScreenCamera;

        public Slider loadingScreenLoadingBar;

        public string sceneToLoad;

        private void Start()
        {

            StartCoroutine(LoadScene());

        }

        float totalProgress = 0;

        IEnumerator LoadScene()
        {

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

            Showroom.ShowroomSSPDataHandler.Instance.StartDownloadingFairtouchData();

            while (!asyncLoad.isDone)
            {
                totalProgress = Mathf.Clamp01(asyncLoad.progress / .9f);

                loadingScreenLoadingBar.value = totalProgress;

                yield return null;
            }

            if (asyncLoad.isDone)//&& Showroom.ShowroomManager.Instance != null && Showroom.ShowroomSSPDataHandler.Instance.isFinished)
            {

                loadingScreenLoadingBar.value = 1f;

                loadingScreenCamera.gameObject.SetActive(false);

                Showroom.ShowroomSSPDataHandler.Instance.StartSettingData();

            }

        }


        public void StartFadingLoadingScreen()
        {

            loadingScreenCanvasGroup.DOFade(0f, 1f).SetDelay(.5f).OnComplete(DisableLoadingScreen);

        }

        void DisableLoadingScreen()
        {

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));

            loadingScreenCanvasGroup.gameObject.SetActive(false);

        }
    }

}