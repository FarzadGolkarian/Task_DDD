namespace Task_DDD.Application.Dto.Users
{
    public record ChangeUserPasswordDto(string CurrentPassword,
                                        string NewPassword,
                                        string RePassword);
}
