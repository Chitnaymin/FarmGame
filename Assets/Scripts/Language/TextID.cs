using UnityEngine;
using UnityEngine.UI;

public class TextID : MonoBehaviour {
    public int TextIDNumber;
    [HideInInspector]
    public string TextString;

    Text text;

    public TextID(int textIDNumber, string textString) {
        TextIDNumber = textIDNumber;
        TextString = textString;
    }

    // Start is called before the first frame update
    void Start() {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update() {

    }
    void SetText() {
        text.text = TextString;
    }
}
