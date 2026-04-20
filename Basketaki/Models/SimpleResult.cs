namespace Basketaki.Models
{
    public class SimpleResult
    {

        public bool Success { get; set; }
        public string? Message { get; set; }



        public static SimpleResult Ok(string? message = null)
        {

            return new SimpleResult { Success = true, Message = message };

        }

        public static SimpleResult Fail(string message)
        {

            return new SimpleResult { Success = false, Message = message };

        }

    }
}
