namespace Task_DDD.Domain
{
    public class BaseEntity <T>  where T : struct
    {
        public T Id { get; protected set; }

        protected BaseEntity()
        {
            if (typeof(T) == typeof(Guid)) Id = (T)(object)Guid.NewGuid();

            else Id = default;
        }


    }
}
