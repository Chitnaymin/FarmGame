using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwardObj : MonoBehaviour
{
	private int id;
	private string keycode;
	private string serial;
	private int titleID;

	public AwardObj(int id, string keycode, string serial, int title) {
		this.id = id;
		this.keycode = keycode;
		this.serial = serial;
		this.titleID = title;
	}

	public int GetId() {
		return id;
	}

	public void SetId(int value) {
		id = value;
	}

	public string GetKeycode() {
		return keycode;
	}

	public void SetKeycode(string value) {
		keycode = value;
	}

	public string GetSerial() {
		return serial;
	}

	public void SetSerial(string value) {
		serial = value;
	}

	public int GetTitleID() {
		return titleID;
	}

	public void SetTitleID(int value) {
		titleID = value;
	}
}
