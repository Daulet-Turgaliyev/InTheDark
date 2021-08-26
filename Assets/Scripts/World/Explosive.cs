using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace com.Acinus.InTheDark
{
	public class Explosive : MonoBehaviour
	{
		/*
				public PhotonView pv;
		
				[SerializeField] private float radius;
				[SerializeField] private float power;
				[SerializeField] private float damage;
				[SerializeField] private LayerMask layerMask;
		
				private bool isPlayer; // �����, ����������
		
				private void Start()
				{
					StartCoroutine(Timer());
				}
		
				IEnumerator Timer()
				{
					yield return new WaitForSeconds(1.3f);
					Explosion2D(gameObject.transform.position);
				}
				private void Explosion2D(Vector3 position)
				{
					Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius, layerMask);
		
					foreach (Collider2D hit in colliders)
					{
						if (hit.attachedRigidbody != null)
						{
							Vector3 direction = hit.transform.position - position;
							direction.z = 0;
							if (hit.gameObject.name == "Heart")
							{
								float dist = Vector2.Distance(gameObject.transform.position, hit.gameObject.transform.position);
								int dmg = Mathf.RoundToInt(damage / dist);
		
								hit.gameObject.GetComponent<PlayerAction>().Damage(dmg);
		
								hit.attachedRigidbody.AddForce(direction.normalized * power);
							}
						}
					}
					Destroy(gameObject);
				}
			*/
	}
}