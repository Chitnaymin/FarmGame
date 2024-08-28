using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectComfirmation : MonoBehaviour {
    public static bool DIRECTBUILD = false;
    public static int DIRECTBLOCKID = 0;

    public static void SETDIRECTBUILD(bool directbuild, int blockid) {
        DIRECTBUILD = directbuild;
        DIRECTBLOCKID = blockid;
    }
}