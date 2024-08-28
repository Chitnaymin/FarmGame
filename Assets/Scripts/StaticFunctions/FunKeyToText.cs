using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunKeyToText : MonoBehaviour {
	static string[] strKeyCode = { "levelTo30", "sold300times", "done30quests", "login30times", "sharedLinksToFriends" };
	static int[] intTextID = { 10214, 10215, 10216, 10217, 10218 };

	public static int KeyCodeToTextID(string key_code) {
		for (int i = 0; i < strKeyCode.Length; i++) {
			if (key_code == strKeyCode[i]) {
				return intTextID[i];
			}
		}
		return 0;
	}

}
