using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipController : UnitySingleton<ToolTipController>
{
    [SerializeField]
    private RectTransform _playerItemToolTip;

    private const int fixedResWidth = 1920;
    private const int fixedResHeight = 1080;

    public void Open(string textToShow, Vector2 position, Vector2 offset, float _xPivot)
    {
        _playerItemToolTip.gameObject.SetActive(true);

        _playerItemToolTip.pivot = new Vector2(_xPivot, 1f);

        float ratioX = (float)Screen.width / fixedResWidth;
        float ratioY = (float)Screen.height / fixedResHeight;

        offset *= new Vector2(ratioX, ratioY);

        string pattern = @"(.{1," + 30 + @"})(\s+|$)";
        textToShow = Regex.Replace(textToShow, pattern, "$1" + Environment.NewLine);
        _playerItemToolTip.position = position + offset;
        _playerItemToolTip.GetComponentInChildren<TextMeshProUGUI>().text = textToShow;

        _playerItemToolTip.ForceUpdateRectTransforms();

        LayoutRebuilder.ForceRebuildLayoutImmediate(_playerItemToolTip);

    }

    public void Close()
    {
        _playerItemToolTip.gameObject.SetActive(false);
    }


}

