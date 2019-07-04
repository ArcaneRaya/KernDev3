using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentController : BaseController {

    [SerializeField] private Fragment fragmentPrefab;
    [SerializeField] private List<FragmentLocation> potentialFragmentLocations = new List<FragmentLocation>();
    [SerializeField] private int fragmentSpawnAmount = 10;

    private List<Fragment> spawnedFragments = new List<Fragment>();

    protected override void OnInitialize(List<BaseController> controllers) {

    }

    protected override void OnSetup() {
        int fragmentsSpawned = 0;
        List<FragmentLocation> remainingLocations = potentialFragmentLocations;
        while (fragmentsSpawned < fragmentSpawnAmount && remainingLocations.Count > 0) {
            int randomIndex = Random.Range(0, remainingLocations.Count);
            FragmentLocation newLocation = remainingLocations[randomIndex];
            remainingLocations.Remove(newLocation);
            Fragment newFragment = Instantiate(fragmentPrefab.gameObject, newLocation.FragmentContainer).GetComponent<Fragment>();
            newFragment.transform.localPosition = Vector3.zero;
            newFragment.transform.localRotation = Quaternion.identity;
            spawnedFragments.Add(newFragment);
            fragmentsSpawned++;
        }
    }

    protected override void OnTick(float deltaTime) {

    }
}
