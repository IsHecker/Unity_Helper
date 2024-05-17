namespace UnityHelper
{
    public class UniqueRandomNumber
    {
        private int remainingNumbers;
        private int lastNumber;
        private readonly System.Random random = new System.Random();

        public int GetRandomNumber(int min, int max)
        {
            this.remainingNumbers = max - min + 1;

            if (remainingNumbers == 0)
            {
                remainingNumbers = max - min + 1;
                lastNumber = -1; // Reset the last number to avoid immediate repetition.
            }

            int selectedNumber;

            do
            {
                selectedNumber = random.Next(min, max + 1);
            }
            while (selectedNumber == lastNumber);

            lastNumber = selectedNumber;
            remainingNumbers--;

            return selectedNumber;
        }
    }
}
