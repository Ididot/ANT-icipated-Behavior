using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace foodSize
{
    //Size class to handle all different instance of foodSource
    public class size
    {
        public int index = 0;
        public int maxPieces = 0;
        public int activePieces = 0;
        public float transformCoef = 1.0f;

        public size()
        {
            index = Random.Range(1, 4); ;

            switch (index)
            {
                case 1:
                    {
                        maxPieces = 5;
                        transformCoef = 0.5f;
                        break;
                    }

                case 2:
                    {
                        maxPieces = 10;
                        transformCoef = 1.0f;
                        break;
                    }
                case 3:
                    {
                        maxPieces = 20;
                        transformCoef = 1.5f;
                        break;
                    }
                default:
                    {
                        Debug.Log("Faulty Size, for food");
                        break;

                    }
            }
        }

    }

}