using System;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
 private Player player;
 private float footStepTimer;
 private float footStepTimerMax=.1f;

 private void Awake()
 {
  player = GetComponent<Player>();
 }

 private void Update()
 {
  footStepTimer -= Time.deltaTime;
  if (footStepTimer<0f)
  {
   footStepTimer = footStepTimerMax;
   if (player.IsWalking())
   {
    float valume = 1f;
    SoundManager.Instance.PlayFootStepsSound(player.transform.position,valume);
   }
  }
 }
}
