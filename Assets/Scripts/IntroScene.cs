using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    [SerializeField] private float delay1;
    [SerializeField] private float delay2;
    [SerializeField] private float delay3;
    [SerializeField] private float delay4;
    [SerializeField] private float delay5;
    [SerializeField] private GameObject clip;
    [SerializeField] private GameObject text1;
    [SerializeField] private GameObject text2;
    [SerializeField] private GameObject text3;
    [SerializeField] private GameObject text4;

    private void Start()
    {
        StartCoroutine(IntroSeq());
    }

    private IEnumerator IntroSeq()
    {
        //MusicManager.Instance.SetVolume(0f);
        MusicManager.Instance.PlayIntroMusic();
        yield return new WaitForSeconds(delay1);
        text1.gameObject.SetActive(true);
        //MusicManager.Instance.SetVolume(1f);
        

        yield return new WaitForSeconds(delay2);
        text1.gameObject.SetActive(false);
        text2.gameObject.SetActive(true);

        yield return new WaitForSeconds(delay3);
        text2.gameObject.SetActive(false);
        text3.gameObject.SetActive(true);

        yield return new WaitForSeconds(delay4);
        text3.gameObject.SetActive(false);
        text4.gameObject.SetActive(true);

        yield return new WaitForSeconds(delay5);
        text4.gameObject.SetActive(false);

        SceneManager.LoadScene("MainMenu 1");
    }
}
