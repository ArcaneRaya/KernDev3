using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentController : BaseController {

    [SerializeField] private Fragment fragmentPrefab = null;
    [SerializeField] private int fragmentSpawnAmount = 10;
    private List<FragmentLocation> potentialFragmentLocations = new List<FragmentLocation>();

    private List<Fragment> spawnedFragments = new List<Fragment>();

    public List<Fragment> GetFragmentsInRange(Vector3 position, float range) {
        List<Fragment> fragmentsInRange = new List<Fragment>();
        foreach (Fragment fragment in spawnedFragments) {
            if ((fragment.transform.position - position).sqrMagnitude < range * range) {
                fragmentsInRange.Add(fragment);
            }
        }
        return fragmentsInRange;
    }

    protected override void OnInitialize() {
        potentialFragmentLocations = new List<FragmentLocation>(FindObjectsOfType<FragmentLocation>());
    }

    protected override void OnSetup() {
        int fragmentsSpawned = 0;
        List<FragmentLocation> remainingLocations = potentialFragmentLocations;
        while (fragmentsSpawned < fragmentSpawnAmount && remainingLocations.Count > 0) {
            int randomIndex = UnityEngine.Random.Range(0, remainingLocations.Count);
            FragmentLocation newLocation = remainingLocations[randomIndex];
            remainingLocations.Remove(newLocation);
            Fragment newFragment = Instantiate(fragmentPrefab.gameObject, newLocation.FragmentContainer).GetComponent<Fragment>();
            newFragment.transform.localPosition = Vector3.zero;
            newFragment.transform.localRotation = Quaternion.identity;
            newFragment.OnPickedUpAction += OnFragmentPickup;
            spawnedFragments.Add(newFragment);
            fragmentsSpawned++;
        }
    }

    protected override void OnTick(float deltaTime) {

    }

    protected override void OnTerminate() {

    }

    private void OnFragmentPickup(Fragment fragment) {
        spawnedFragments.Remove(fragment);
    }
}
