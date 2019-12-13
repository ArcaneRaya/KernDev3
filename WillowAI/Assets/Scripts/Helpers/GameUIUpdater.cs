using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameUIUpdater : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI fragmentField = null;
    [SerializeField] private Player player = null;
    [SerializeField] private FriendlyController friendlyController = null;
    [SerializeField] private GameObject hintPressE = null;
    [SerializeField] private GameObject hintPressQ = null;
    [SerializeField] private GameObject hintRobotHasFragment = null;

    public void Tick(float deltaTime) {
        fragmentField.text = player.FragmentsCollected.ToString();
        UpdateHints();
    }

    private void UpdateHints() {
        hintPressE.SetActive(player.HasFragmentWithinRange);
        hintRobotHasFragment.SetActive(friendlyController.FriendlyHasFragment);
    }
}
