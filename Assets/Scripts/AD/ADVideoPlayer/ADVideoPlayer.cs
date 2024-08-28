using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADVideoPlayer : MonoBehaviour {

    UnityEngine.Video.VideoPlayer vio;

    private void Start() {
        vio = GetComponent<UnityEngine.Video.VideoPlayer>();
        vio.targetCamera = Camera.main;
        vio.Play();
        Controller.Instance().allowClick = false;
        StartCoroutine(VideoEnd((float)vio.length));
    }

    IEnumerator VideoEnd(float videoLength) {
        yield return new WaitForSeconds(videoLength);
        Controller.Instance().allowClick = true;
        GameManager.Instance().ForceIdle();
        Destroy(transform.parent.gameObject);
    }

    public int VideoLength() {
        return ((int)vio.length);
    }
}
