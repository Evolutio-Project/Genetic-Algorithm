using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticPathfinder : MonoBehaviour
{
   public float creatureSpeed;
   public float pathMultiplier;
   int pathIndex = 0;
   public DNA dna;
   public bool hasFinished;
   bool hasBeenInitialized = false;
   Vector2 goal;
   Vector2 nextPoint;

    private void Awake()
    {
        InitCreature(new DNA(), Vector2.zero);
    }
    public void InitCreature(DNA newDna, Vector2 _goal)
    {
        dna = newDna;
        goal = _goal;
        nextPoint = transform.position;
        hasBeenInitialized = true;
    }

    private void Update()
    {
        if(hasBeenInitialized && !hasFinished)
        {
            if(pathIndex == dna.genes.Count)
            {
                End();
            }
            if((Vector2)transform.position == nextPoint)
            {
                nextPoint = (Vector2)transform.position + dna.genes[pathIndex];
                pathIndex++;
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position,nextPoint,creatureSpeed * Time.deltaTime);
            }
        }
    }
    void End()
    {
        hasFinished = true;
    }
    
    public float fitness
    {
         get
         {
             float dist = Vector2.Distance(transform.position,goal);
             if(dist == 0)
             {
                 dist = 0.0001f;
             }
             return 60/dist;
         }
    }
}
