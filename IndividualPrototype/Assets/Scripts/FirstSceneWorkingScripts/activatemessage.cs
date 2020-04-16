using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class activatemessage : MonoBehaviour
{
    [SerializeField] GameObject messagetoEnable, picturetoEnable;
    [SerializeField] string loadScene;
    loadscene newLoader;

    private void Awake()
    {
        if(this.gameObject.GetComponent<loadscene>()==null)
        {
            this.gameObject.AddComponent<loadscene>();
        }

        newLoader = this.gameObject.GetComponent<loadscene>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {

            messagetoEnable.SetActive(true);
            if(picturetoEnable!=null)
            {
                picturetoEnable.SetActive(true);
            }
            StartCoroutine(loadMainMenu());

        }
    }

    IEnumerator loadMainMenu()
    {
        yield return new WaitForSeconds(5f);

        newLoader.LoadScene(loadScene);
    }


}
