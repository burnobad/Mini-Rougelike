using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GroundInfoUI : MonoBehaviour
{
    [SerializeField]
    private Color inactiveColor;

    [SerializeField]
    private Color activeColor;

    [SerializeField]
    private List<Image> groundImages;

    [SerializeField]
    private TMP_Text groundText;
    void Start()
    {
        GameEventsManager.UpdateLevelData += UpdateGroundInfo;
    }


    void UpdateGroundInfo(int _curLevel, int _curUpgradesPicked)
    {
        groundText.text = _curLevel.ToString();

        for(int i = 0; i < groundImages.Count; i++) 
        {
            if (i + 1 <= _curUpgradesPicked) 
            {
                groundImages[i].color = activeColor;
            }
            else
            {
                groundImages[i].color = inactiveColor;
            }
        }

    }
}
