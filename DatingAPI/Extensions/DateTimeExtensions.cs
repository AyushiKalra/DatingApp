namespace DatingAPI.Extensions
{
    public static class DateTimeExtensions
    {
        //The static keyword is very useful in c# when we need to use class, method, or property without creating an object instance
        public static int CalculateAge(this DateOnly dob){
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var age = today.Year -dob.Year;

            if(dob > today.AddYears(-age)) age--;
            return age;
        }        
    }
}