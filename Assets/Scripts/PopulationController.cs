using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationController : MonoBehaviour
{
   List<GeneticPathfinder> population = new List<GeneticPathfinder>();
    public int populationSize;
    public int geneomeLength;
    public float cutoff = .3f;
    public GameObject creaturePrefab;
    public Transform start;
    public Transform end;
    

    private void Start()
    {
        InitPopulation();
    }
    private void Update()
    {
        if(!HasActive())
        {
            NextGeneration();
        }
    }
    void InitPopulation()
    {
        for(int i=0; i< populationSize; i++)
        {
            GameObject go = Instantiate(creaturePrefab, start.position, Quaternion.identity);
            go.GetComponent<GeneticPathfinder>().InitCreature(new DNA(geneomeLength),end.position);
            population.Add(go.GetComponent<GeneticPathfinder>()); 
        }
    }

    void NextGeneration()
    {
        int survivorCut = Mathf.RoundToInt(populationSize * cutoff);
        List<GeneticPathfinder> survivors = new List<GeneticPathfinder>();
        print("survivor cut: " + survivorCut);
        //pick best creatures
        for(int i=0; i< survivorCut; i++)
        { 
            survivors.Add(GetFittest());
            if(i==0){
                print(survivors[i].fitness);
            }
        }

        //kill current population
        for (int i=0; i<population.Count; i++)
        {
            Destroy(population[i].gameObject);
        }
        population.Clear();

        //make new population
        while(population.Count < populationSize)
        {
            for(int i=0;i<survivors.Count; i++)
            {
                GameObject go = Instantiate(creaturePrefab, start.position, Quaternion.identity);
                go.GetComponent<GeneticPathfinder>().InitCreature(new DNA(survivors[Random.Range(0,(int)survivorCut)].dna), end.position);
                population.Add(go.GetComponent<GeneticPathfinder>());

                if(population.Count >= populationSize)
                {
                    break;
                }
            }
        }

        //delete old survivors list 
        for (int i=0; i<survivors.Count; i++)
        {
            Destroy(survivors[i].gameObject);
        }
    }
    GeneticPathfinder GetFittest()
    {
        float maxFitness = float.MinValue;
        int index = 0;
        for(int i=0; i< population.Count; i++)
        {
            if(population[i].fitness > maxFitness)
            {
                maxFitness = population[i].fitness;
                index = i;
            }
        }
        GeneticPathfinder fittest = population[index];
        population.Remove(fittest);
        return fittest;
    }
    bool HasActive()
    {
        for (int i=0; i< population.Count; i++)
        {
            if(population[i].hasFinished == false)
            {
                
                return true;
            }
        }
        print("done");
        return false;
    }
    

}
