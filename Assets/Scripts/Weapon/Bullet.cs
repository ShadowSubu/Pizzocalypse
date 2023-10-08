using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int Damage = 50;
    private void Update()
    {
        transform.Translate(Vector3.forward * 20 * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
      //      other.GetComponent<ZombieClass>().TakeDamage(Damage);
        }
    }

    private void OnEnable()
    {
        //DestroySelf();
    }

    async void DestroySelf()
    {
        await Task.Delay(2000);
        Destroy(gameObject);
    }
}
