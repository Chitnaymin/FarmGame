using System.Collections;
using UnityEngine;

public class ShareBTN : MonoBehaviour {

    public void ShareBTNClick() {
        StartCoroutine(TakeSSAndShare());
    }

    private IEnumerator TakeSSAndShare() {
        yield return new WaitForEndOfFrame();
        new NativeShare().SetSubject("每天酵一笑").SetText("https://play.google.com/store/apps/details?id=com.GameFactory.FarmGame08").Share();
        GUIUtility.systemCopyBuffer = "https://play.google.com/store/apps/details?id=com.GameFactory.FarmGame08";
        int Progress = AchievementManager.Instance().pdb.GetCurrentProgress((int)E_AchType.Share);
        AchievementManager.Instance().MajorAchievementUpdate(E_AchType.Share, Progress + 1);
    }
}
