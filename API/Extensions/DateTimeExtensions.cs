using System;

namespace API.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateTime dob)
        {
            var today = DateTime.Today;
            var age=today.Year-dob.Year; //eg. 2021-1997=23
            if (dob.Date>today.AddYears(-age))  age--; //check if they already had their birthday this year or not. If not, return age--
            //if 21 2 1997 > (3/1/2021)-23  -> 22
            return age;
        }
        

        
    }
}