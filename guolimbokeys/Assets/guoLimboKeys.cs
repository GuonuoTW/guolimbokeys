using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using rd = UnityEngine.Random;
using math = ExMath;

public class guoLimboKeys : MonoBehaviour {

   public KMBombInfo Bomb;
   public KMAudio Audio;
   //Register
   public KMSelectable[] bt;
   public GameObject[] pos;
   int[] curpos={0,1,2,3,4,5,6,7};
   bool hasScrambled=false;
   int scrambleStepsLeft=30;
   int moveDuration=1;
   string[] possibleSwaps={"TopBottomCW","TopBottomCCW","TopBottomCWCCW","TopBottomCCWCW","TopBottomDiagonal","TopBottomSwap","AllRowUp","AllRowDown","AllColumnLeftRight","AllCW","AllCCW","180AllRowUp","180AllRowDown"};
   //Dont touch
   static int ModuleIdCounter = 1;
   int ModuleId;
   private bool ModuleSolved;

   void Awake () { //Avoid doing calculations in here regarding edgework. Just use this for setting up buttons for simplicity.
      ModuleId = ModuleIdCounter++;
      GetComponent<KMBombModule>().OnActivate += Activate;
   /*
      for (int i=0;i<8;i++) {
         curpos[i]=bt[i];
      }
   */
      for (int i=0;i<8;i++) {
         int temp=i;
         bt[temp].OnInteract += delegate () { buttonPress(bt[temp]); return false; };
      }
      //utton.OnInteract += delegate () { buttonPress(); return false; };

   }

   void OnDestroy () { //Shit you need to do when the bomb ends
      
   }

   void Activate () { //Shit that should happen when the bomb arrives (factory)/Lights turn on

   }

   void Start () { //Shit that you calculate, usually a majority if not all of the module
      //begin scramble
      int r;
      string logString="";
      /*
      while (scrambleStepsLeft > 0) {
         //r=rd.Range(0,logString.size);
         r=0;
         if ((r==9 || r==10) && scrambleStepsLeft < 2) continue; 
         logString+=possibleSwaps[r]+", ";
         scrambleStepsLeft-=((r==9 || r==10) ? 2 : 1);
      }
      */


      //
      Debug.LogFormat("[Limbo Keys #{0}] Scramble Sequence is {1}", ModuleId, logString);
   }

   void Update () { //Shit that happens at any point after initialization

   }

   void Solve () {
      GetComponent<KMBombModule>().HandlePass();
   }

   void Strike () {
      GetComponent<KMBombModule>().HandleStrike();
   }

   void buttonPress(KMSelectable obj) {
      Debug.LogFormat("pressed");
      Solve();
      for (int i=0;i<8;i++) {
         if (obj==bt[i]) {
            scrambleAnimation(i);
         }
      }
      //Button.AddInteractionPunch();
      //GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Button.transform);
      //scrambleAnimation(0);
   }

   IEnumerator LerpMove(int idx, int target) {
      float dtime=0;
      int localidx=curpos[idx];
      int loctarget=target;
      while (dtime<moveDuration) {
         bt[localidx].transform.position = Vector3.Lerp(bt[localidx].transform.position, pos[loctarget].transform.position, dtime/moveDuration );
         dtime += Time.deltaTime;
         yield return null;
      }

      bt[localidx].transform.position = pos[loctarget].transform.position;
      curpos[loctarget]=localidx;
   }
   void scrambleAnimation(int r) {
      int temp,temp2;
      switch(r) {
         case 0: //TopBottomCW
            temp=curpos[2]; //top
            StartCoroutine(LerpMove(0,1));
            StartCoroutine(LerpMove(1,3));
            StartCoroutine(LerpMove(3,2));
            StartCoroutine(LerpMove(2,0));
            curpos[0]=temp;
            //bottom
            temp=curpos[6]; //top
            StartCoroutine(LerpMove(4,5));
            StartCoroutine(LerpMove(5,7));
            StartCoroutine(LerpMove(7,6));
            StartCoroutine(LerpMove(6,4));
            curpos[4]=temp;
            break;
         case 1: //TopBottomCCW
            temp=curpos[0]; //top
            StartCoroutine(LerpMove(1,0));
            StartCoroutine(LerpMove(3,1));
            StartCoroutine(LerpMove(2,3));
            StartCoroutine(LerpMove(0,2));
            curpos[2]=temp;
            //bottom
            temp=curpos[4]; //top
            StartCoroutine(LerpMove(5,4));
            StartCoroutine(LerpMove(7,5));
            StartCoroutine(LerpMove(6,7));
            StartCoroutine(LerpMove(4,6));
            curpos[6]=temp;
            break;
         case 2: //TopBottomCWCCW
            temp=curpos[0]; //top
            StartCoroutine(LerpMove(1,0));
            StartCoroutine(LerpMove(3,1));
            StartCoroutine(LerpMove(2,3));
            StartCoroutine(LerpMove(0,2));
            curpos[2]=temp;
            //bottom
            temp=curpos[4]; //top
            StartCoroutine(LerpMove(5,4));
            StartCoroutine(LerpMove(7,5));
            StartCoroutine(LerpMove(6,7));
            StartCoroutine(LerpMove(4,6));
            curpos[6]=temp;
            break;
         case 3: //TopBottomCCWCW
            temp=curpos[0]; //top
            StartCoroutine(LerpMove(1,0));
            StartCoroutine(LerpMove(3,1));
            StartCoroutine(LerpMove(2,3));
            StartCoroutine(LerpMove(0,2));
            curpos[2]=temp;
            //bottom
            temp=curpos[6]; //top
            StartCoroutine(LerpMove(4,5));
            StartCoroutine(LerpMove(5,7));
            StartCoroutine(LerpMove(7,6));
            StartCoroutine(LerpMove(6,4));
            curpos[4]=temp;
            break;
         case 4: //TopBottomDiagonal
            temp=curpos[3]; //top
            temp2=curpos[1];
            StartCoroutine(LerpMove(0,3));
            StartCoroutine(LerpMove(3,0));
            StartCoroutine(LerpMove(2,1));
            StartCoroutine(LerpMove(1,2));
            curpos[0]=temp;
            curpos[2]=temp2;
            //bottom
            temp=curpos[7]; //top
            temp2=curpos[6];
            StartCoroutine(LerpMove(4,7));
            StartCoroutine(LerpMove(7,4));
            StartCoroutine(LerpMove(5,6));
            StartCoroutine(LerpMove(6,5));
            curpos[4]=temp;
            curpos[5]=temp2;
            break;
         default:
            return;
      }
   }



#pragma warning disable 414
   private readonly string TwitchHelpMessage = @"Use !{0} to do something.";
#pragma warning restore 414

   IEnumerator ProcessTwitchCommand (string Command) {
      yield return null;
   }

   IEnumerator TwitchHandleForcedSolve () {
      yield return null;
   }
}
