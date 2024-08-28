using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


[System.Serializable]
public class Boundary {
    public float xMin, xMax, yMin, yMax;
}

public class Panning : MonoBehaviour {
    Vector3 touchStart;
    public float zoomOutMin = 1;
    public float zoomOutMax = 8;
    public Boundary[] boundary;
    StorageManager db;
    public int expansionData;

    private void Start() {
        db = new StorageManager();
        ExpanstionDataSet();
	}

    void Update() {

    }
    public void ExpanstionDataSet() {
        expansionData = ExpansionManager.Instance().GetExpensionLevel();
    }
    public void LateUpdate() {
        transform.position = new Vector3(
								Mathf.Clamp(transform.position.x, boundary[expansionData].xMin, boundary[expansionData].xMax),
								Mathf.Clamp(transform.position.y, boundary[expansionData].yMin, boundary[expansionData].yMax),
								-10f);
    }

    // dead extra function
    void zoom(float increment) {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
    }

}