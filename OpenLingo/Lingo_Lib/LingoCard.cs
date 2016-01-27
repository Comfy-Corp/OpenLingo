using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace LingoLib
{
    public class LingoCard
    {
        public int[] numbers = new int[25];

        public LingoCard(bool isEven, int[] mask)
        {
            numbers = getRandomLingoCard(true);
            numbers = sortLingoCard(numbers);
            numbers = applyLingoMask(numbers, mask);
        }

        public override string ToString()
        {
            string retVal = "";
            for (var i = 0; i < 25; ++i)
            {
                var number = (numbers[i] < 0) ? 0 : numbers[i];
                retVal += ""+number+"\t";
                if(i%5 == 4){
                    retVal+=("\n");
                }
            }
            return retVal;
        }

        public int[] getAllDrawableNumbers()
        {
            List<int> numbers = new List<int>();
            foreach (int number in numbers)
            {
                if (number < 0)
                {
                    numbers.Add(number);
                }
            }
            return numbers.ToArray();

        }

        static private int[] getRandomLingoCard(bool isEven)
        {
            int[] returnNumbers = new int[25];

            for (int i = 0; i < 25; i++)
            {
                int number = 0;
                var unique = false;
                Random random = new Random();
                while (!unique)
                {
                    number = ((random.Next() % 34) * 2);
                    number = (number == 0) ? 70 : number;
                    number = (isEven) ? number : number - 1;
                    unique = (returnNumbers.Contains(number)) ? false : true;
                }
                returnNumbers[i] = number;
            }
            return returnNumbers;
        }

        static public int[] generateLingoCardMask()
        {
            Random random = new Random();
            int randomInt = ((random.Next()) % 4);
            switch (randomInt)
            {
                case 0:
                    return new int[] { 1, 1, 1, -1, -1,
					1, -1, 1, 1, 1,
					-1, 1, 1, -1, 1,
					-1, 1, 1, 1, 1,
					1, 1, -1, 1, -1
				};
                case 1:
                    return new int[] {-1, 1, -1, 1, 1,
					1, 1, 1, -1, 1,
					1, -1, 1, 1, -1,
					1, 1, -1, 1, 1,
					-1, 1, 1, -1, 1
				};
                case 2:
                    return new int[] { 1, -1, 1, 1, 1,
					1, 1, -1, -1, 1,
					-1, 1, 1, 1, 1,
					1, 1, -1, 1, -1,
					1, -1, 1, 1, -1
				};
                case 3:
                    return new int[] {-1, 1, 1, -1, 1,
					1, 1, -1, 1, -1,
					1, -1, 1, 1, 1,
					1, 1, -1, 1, -1,
					-1, 1, 1, 1, 1
				};
                default:
                    return new int[] { 1, 1, 1, 1, -1,
					-1, 1, -1, 1, 1,
					1, 1, 1, -1, 1,
					-1, -1, 1, 1, 1,
					1, -1, 1, 1, -1
				};
            }
        }

        static private int[] applyLingoMask(int[] card, int[] mask)
        {

            int[] newCard = card;

            for (int i = 0; i < 25; i++)
            {
                newCard[i] = card[i] * mask[i];
            }
            return newCard;

        }

        static private int[] sortLingoCard(int[] card)
        {
            int[] newCard = new int[25];

            for (int i = 0; i <= 20; i += 5)
            {
                int[] sortArray = new int[5];
                for (int j = i; j < i+5; j++)
                {
                    sortArray[j%5] = card[(j%5)+i];
                }
                Array.Sort(sortArray); 
                sortArray.CopyTo(newCard, i);
            }
            return newCard;
        }


    }
}

