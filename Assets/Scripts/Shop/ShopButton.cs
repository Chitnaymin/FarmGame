using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopButton : MonoBehaviour
{
	public GameObject categoryList;
	public Sprite neutral, hightlight;
	private Image sprite;

    void Awake()
    {
		sprite = GetComponent<Image>();
    }
 
	public void Click()
	{
		if (sprite.sprite == neutral)
		{
			sprite.sprite = hightlight;
			//this.transform.GetChild(0).GetComponent<Outline>().effectColor = new Color(199, 124, 7, 255);
			categoryList.SetActive(true);
		}
		else if(sprite.sprite == hightlight)
		{
			sprite.sprite = neutral;
			//this.transform.GetChild(0).GetComponent<Outline>().effectColor = new Color(94, 58, 1, 255);
			categoryList.SetActive(false);
		}
	}

	public void Refresh() {
		sprite.sprite = neutral;
		categoryList.SetActive(false);
	}
}
