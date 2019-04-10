namespace Ex03_Minion_Names
{
    public class Min
    {
        private static int rowNum = 1;

        public Min()
        {
            Num = rowNum++;
        }
        public int Num { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}