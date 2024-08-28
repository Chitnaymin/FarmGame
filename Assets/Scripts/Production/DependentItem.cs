using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DependentItem {

    int requireID;
    int requireCount;

    public DependentItem(int requireID, int requireCount) {
        this.requireID = requireID;
        this.requireCount = requireCount;
    }

    public int GetRequireID() {
        return requireID;
    }

    public void SetRequireID(int value) {
        requireID = value;
    }

    public int GetRequireCount() {
        return requireCount;
    }

    public void SetRequireCount(int value) {
        requireCount = value;
    }
}
