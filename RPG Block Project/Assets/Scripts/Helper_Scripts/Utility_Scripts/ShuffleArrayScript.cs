using System.Collections;

public static class ShuffleArrayScript
{ 
    public static T[] ShuffleArray<T>(T[] array, int seed)
    {
        System.Random r = new System.Random(seed);

        for (int i = 0; i < array.Length - 1; i++)
        {
            int randIndex = r.Next(i, array.Length);
            T tempIndex = array[randIndex];

            array[randIndex] = array[i];
            array[i] = tempIndex;
        }
        return array;
    }
}
