namespace Admin.Models.Results
{
    public class SuccessDataResult<T>: Admin.Models.Results.DataResult<T>
    {
        public SuccessDataResult(T data, string message) : base(data, true, message)
        {
        }

        public SuccessDataResult(T data) : base(data, true)
        {
        }

        public SuccessDataResult(string message):base(default,true,message)
        {
        }

        public SuccessDataResult():base(default,true)
        {
        }
    }
}