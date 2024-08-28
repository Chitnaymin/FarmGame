using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkManager : MonoBehaviour {
    public void Facebook() {
        Application.OpenURL("https://m.facebook.com/jialian8/?locale2=zh_TW");
    }
    public void Website() {
        Application.OpenURL("http://www.jialian8.com/");
    }
    public void FBShareLink() { // no longer use
        //Application.OpenURL("https://www.facebook.com/sharer/sharer.php?u=https://play.google.com/store/apps/details?id=com.habby.archero");
        Application.OpenURL("line://msg/text/?https://play.google.com/store/apps/details?id=com.habby.archero");
        int Progress = AchievementManager.Instance().pdb.GetCurrentProgress((int)E_AchType.Share);
		AchievementManager.Instance().MajorAchievementUpdate(E_AchType.Share, Progress + 1);

		//Application.OpenURL("https://www.facebook.com/sharer/sharer.php?u=https://bit.ly/38aOYzg");
	}
    public void ExitTheGame() {
        Application.Quit();
    }
}
