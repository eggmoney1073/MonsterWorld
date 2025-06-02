using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SLaser : MonoBehaviour
{
    string _targetTag;
    List<GameObject> _targets;

    public List<GameObject> _Target { get { return _targets; } }

    public void Init()
    {
        _targets = new List<GameObject>();
    }

    public void SetLaserTarget(string targetTag)
    {
        _targetTag = targetTag;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_targetTag))
            _targets.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_targetTag))
            _targets.Remove(other.gameObject);
    }
}
