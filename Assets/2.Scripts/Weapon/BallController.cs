using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : Projectile
{
    Monster _monster;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster") || other.CompareTag("EnemyMonster"))
        {
            _monster = other.GetComponent<Monster>();
            _monster.HitBall();

            StartCoroutine(Co_CalculateCapture());

            _isShoot = false;
        }
    }

    IEnumerator Co_CalculateCapture()
    {
        float percentage = _monster._CapturePercentage;
        float random = Random.Range(0f, 100f);

        yield return new WaitForSeconds(3f);

        if (random <= percentage * 100)
        {
            _monster.Captured();
            UIManager._Instance.GetNewMonster(_monster._Type);
            Debug.Log("Capture Success");
        }
        else
        {
            _monster.EscapeBall();
            Debug.Log("Capture Fail");
        }
        EndShooting();
    }
}
