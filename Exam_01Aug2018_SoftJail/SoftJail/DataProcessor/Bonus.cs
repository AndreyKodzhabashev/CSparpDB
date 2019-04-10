namespace SoftJail.DataProcessor
{

    using Data;
    using System;

    public class Bonus
    {
        public static string ReleasePrisoner(SoftJailDbContext context, int prisonerId)
        {
            var prisoner = context.Prisoners.Find(prisonerId);

            if (prisoner != null && prisoner.ReleaseDate != null)
            {
                prisoner.ReleaseDate = DateTime.Now;
                prisoner.CellId = null;
                prisoner.Cell = null;

                context.Prisoners.Update(prisoner);
                context.SaveChanges();
                return $"Prisoner {prisoner.FullName} released";
            }

            return $"Prisoner {prisoner.FullName} is sentenced to life";
        }
    }
}
