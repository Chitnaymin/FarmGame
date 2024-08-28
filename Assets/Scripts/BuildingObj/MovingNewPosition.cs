using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingNewPosition : MonoBehaviour {
    public GameObject canvas;

    private GameObject block;// parent gound object
    private bool byBuild = false;
    private bool showRed; // not allow build or move
    private Vector3 initialPosition;
    private Animator anim;
    private GameObject initalBlock;

    void Awake() {
        initialPosition = transform.position;
        anim = GetComponent<Animator>();
        anim.SetBool("UnableBuild", byBuild);
    }

    void Start() {
        initalBlock = block;
    }

    public void ActionMove(bool move) {
        if (move) {
            canvas.SetActive(true);
            GetComponent<SpriteRenderer>().sortingOrder = 100;
            GetComponent<Building>().BuildingDetailPanel.SetActive(false);
        } else {
            // hide them
            // not allow to move this item 
        }
    }

    // sprite render check
    private bool CheckingBlockAvailable(GameObject block) {
        if (GetComponent<Building>().GetCategory() == Shop_Building.Category.Build || !byBuild ) {
            if ((block.transform.childCount == 0)) {
                return false;
            }
            return true;
        } else {
            if (block.transform.childCount != 0) { 
                int requireID = 0;
                if (GetComponent<Building>().GetCategory() == Shop_Building.Category.Field) {
                    requireID = 1;
                } else if (GetComponent<Building>().GetCategory() == Shop_Building.Category.Tree) {
                    requireID = 2;
                } else if (GetComponent<Building>().GetCategory() == Shop_Building.Category.Pergola) {
                    requireID = 3;
                }

                if (requireID == block.transform.GetChild(0).GetComponent<Building>().GetBuildingID()) {
                    return false;
                }
                return true;
            }
            return true;
        }
    }

    public void Moving(GameObject block, bool byBuild) {
        if (DirectComfirmation.DIRECTBUILD) {
            StartCoroutine(directMove());
        } else {
            this.SetBlock(block);
            this.byBuild = byBuild;
            this.showRed = CheckingBlockAvailable(block);
            anim.SetBool("UnableBuild", showRed);
            transform.position = block.transform.position;
        }
    }

    IEnumerator directMove() {
        canvas.SetActive(false);
        yield return new WaitForEndOfFrame();
        this.byBuild = true;
        showRed = false;
        GameObject directBuildBlock = BuildManager.Instance().GetBlock(DirectComfirmation.DIRECTBLOCKID);
        SetBlock(directBuildBlock);
        transform.position = directBuildBlock.transform.position;
        DirectComfirmation.DIRECTBUILD = false;
        BtnBuildConfrim();
    }
    void CheckAndDestory() {
        if (GetComponent<Building>().GetCategory() != Shop_Building.Category.Build) {
            BuildManager.Instance().DeleteBuilding(block);
        }
        GetComponent<Building>().Build();
    }
    public void BtnBuildConfrim() {
        if (!showRed) {
            int oldBlockID = GetComponent<Building>().GetBlockID();
            GetComponent<Building>().SetBlockRef(block);
            if (byBuild) {
				GetComponent<Building>().BuyFromClick();
				if(GetComponent<Building>().GetCategory()==Shop_Building.Category.Build) 
				{
					if(GetComponent<Building>().GetBuildingID() == 4|| GetComponent<Building>().GetBuildingID() == 5|| GetComponent<Building>().GetBuildingID() == 6) {
						int Progress = AchievementManager.Instance().pdb.GetCurrentProgress((int)E_AchType.Building);
						AchievementManager.Instance().MajorAchievementUpdate(E_AchType.Building, Progress + 1);
					}
				}
                CheckAndDestory();
            } else {
                GetComponent<Building>().Move(oldBlockID);
            }
            initialPosition = transform.position;
            CompleteMove();
        }
        // if red not allow
    }

    public void BtnBuildCancel() {
        if (byBuild) {
            Destroy(gameObject);
            //cancel to disapper
        } else {
            transform.position = initialPosition;
            SetBlock(initalBlock);
            //cancel to previous position
        }
        CompleteMove();
    }

    public void CompleteMove() {
        byBuild = false;
        transform.SetParent(block.transform);
        initalBlock = block;
        canvas.SetActive(false);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = block.GetComponent<SpriteRenderer>().sortingOrder;
        GameManager.Instance().ForceIdle();
    }

    public GameObject Block() {
        return block;
    }

    public void SetBlock(GameObject value) {
        block = value;
    }

}
