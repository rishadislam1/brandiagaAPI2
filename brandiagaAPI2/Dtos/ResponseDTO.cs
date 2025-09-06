namespace brandiagaAPI2.Dtos
{
    public class ResponseDTO<T>
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ResponseDTO(string status, string message, T data)
        {
            Status = status;
            Message = message;
            Data = data;
        }

        // Static factory methods for common scenarios
        public static ResponseDTO<T> Success(T data, string message = "Operation successful")
        {
            return new ResponseDTO<T>("Success", message, data);
        }

        public static ResponseDTO<T> Error(string message)
        {
            return new ResponseDTO<T>("Error", message, default);
        }
    }
}
