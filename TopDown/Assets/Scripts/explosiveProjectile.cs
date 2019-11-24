using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosiveProjectile : MonoBehaviour
{
    float fuseTime = 3;
    float explosionRadius = 3;
    float explosionPower = 100;


    public void setStats(float radius, float force, float time)
    {
        explosionPower = force;
        explosionRadius = radius;
        fuseTime = time;

        StartCoroutine(explode(fuseTime));
    }

    IEnumerator explode(float time)
    {
        yield return new WaitForSeconds(time);

        explode();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        explode();
    }

    void explode()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, explosionRadius, Vector2.zero, 0);
        foreach (RaycastHit2D hit in hits)
        {
            if(hit.transform != transform)
            hit.rigidbody.AddForce(((hit.transform.position - transform.position).normalized * explosionPower) / Vector2.Distance(hit.transform.position,transform.position), ForceMode2D.Impulse);
        }

        Destroy(gameObject);
    }

}
