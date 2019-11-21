using System;
using System.Collections.Generic;
using JetBrains.Annotations;

public static class WeightedRandomizer
    {
        public static WeightedRandomizer<TR> From <TR>(Dictionary<TR, int> spawnRate)
        {
            return new WeightedRandomizer<TR>(spawnRate);
        }
    }
 
    public class WeightedRandomizer<T>
    {
        private static readonly Random Random = new Random();
        private readonly Dictionary<T, int> _weights;
        public WeightedRandomizer(Dictionary<T, int> weights)
        {
            _weights = weights;
        }

        public T TakeOne()
        {
            // Sorts the spawn rate list
            var sortedSpawnRate = Sort(_weights);
 
            // Sums all spawn rates
            int sum = 0;
            foreach (var spawn in _weights)
            {
                sum += spawn.Value;
            }
 
            // Randomizes a number from Zero to Sum
            int roll = Random.Next(0, sum);
 
            // Finds chosen item based on spawn rate
            T selected = sortedSpawnRate[sortedSpawnRate.Count - 1].Key;
            foreach (var spawn in sortedSpawnRate)
            {
                if (roll < spawn.Value)
                {
                    selected = spawn.Key;
                    break;
                }
                roll -= spawn.Value;
            }
 
            // Returns the selected item
            return selected;
        }
 
        private List<KeyValuePair<T, int>> Sort(Dictionary<T, int> weights)
        {
            var list = new List<KeyValuePair<T, int>>(weights);
 
            // Sorts the Spawn Rate List for randomization later
            list.Sort(
                delegate(KeyValuePair<T, int> firstPair,
                         KeyValuePair<T, int> nextPair)
                {
                    return firstPair.Value.CompareTo(nextPair.Value);
                }
             );
 
            return list;
        }
    }