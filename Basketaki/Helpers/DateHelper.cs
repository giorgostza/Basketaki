namespace Basketaki.Helpers
{
    public static class DateHelper
    {
        public static int CalculateAge(DateOnly birthDate)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - birthDate.Year;

            if (birthDate > today.AddYears(-age))
            {

                age--;

            }


            return age;
        }
    }
}
