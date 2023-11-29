using System;
using System.Collections.Generic;

public class SortedList<T> where T : class
{
    private class WeightEntity : IComparable
    {
        public float weight;
        public T entity;

        public int CompareTo(object obj)
            => weight.CompareTo(((WeightEntity)obj).weight);

        public WeightEntity(float weight, T entity)
        {
            this.weight = weight;
            this.entity = entity;
        }

    }


    private List<WeightEntity> _list;
    private int _entityCount;
    private int _capacity;

    private const float INF = 100000f;

    public SortedList(int capacity)
    {
        _list = new List<WeightEntity>(capacity);

        for (int i = 0; i < capacity; i++)
            _list.Add(new WeightEntity(weight: INF, entity: null));

        _entityCount = 0;
        _capacity = capacity;
    }


    public void Add(float weight, T value)
    {
        _list[_entityCount].weight = weight;
        _list[_entityCount].entity = value;
        _entityCount++;
    }


    public T GetMin()
    {
        WeightEntity minEntity = _list[_entityCount];
        for (int i = 0; i < _capacity; i++)
            if (minEntity.weight > _list[i].weight) minEntity = _list[i];

        return minEntity.entity;
    }
        

    public T PopMin()
    {
        WeightEntity minEntity = _list[_entityCount];
        for (int i = 0; i < _capacity; i++)
            if (minEntity.weight > _list[i].weight) minEntity = _list[i];

        minEntity.weight = INF;

        _entityCount--;
        return minEntity.entity;
    }

    public bool isEmpty => _entityCount == 0;

    public void Clear()
    {
        for (int i = 0; i < _capacity; i++)
        {
            _list[i].weight = INF;
            _list[i].entity = null;
        }
        _entityCount = 0;

    }
    


    /*
    private int FindIndexForAddValue(float value)
    {

    }
    */



}
