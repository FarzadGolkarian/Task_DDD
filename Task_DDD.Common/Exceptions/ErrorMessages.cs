namespace Task_DDD.Common.Exceptions;

public static class ErrorMessages
{

    public const string ValidationEnumType = " {0} وجود ندارد";
    public const string FullNameIsInvalid = "نام الزامی می باشد";
    public const string EmailIsInvalid = "ایمیل الزامی می باشد";
    public const string FullNameMaxLength = "نام نمیتواند بیشتر از {0} کاراکتر باشد";
    public const string EmailIsInvalidFormat = "آدرس ایمیل {0} معتبر نمیباشد";
    public const string CurrentPasswordIsRequired = "لطفا رمز عبور فعلی خود را وارد کنید";
    public const string NewPasswordRequired = "لطفا رمز عبور جدید خود را وارد کنید";
    public const string ReNewPasswordRequired = "تکرار رمز عبور الزامی می باشد";
    public const string CurrentPasswordInvalid = "رمز عبور وارد شده با رمز عبور قبلی مطابقت ندارد";
    public const string NewPasswordAndRePasswordInvalid = "رمز عبور با تکرار آن برابر نیست";
    public const string PasswordIsRequired = "رمز عبور الزامی می باشد";
    public const string PasswordMaxLength = "کاراکتر رمز عبور نمیتواند بیشتر از {0} کاراکتر باشد";
    public const string PasswordMinLength = "کاراکتر رمز عبور نمیتواند کمتر از {0} کاراکتر باشد";
    public const string TitleIsRequired = " عنوان الزامی می باشد";
    public const string TitleMaxLength = "عنوان نمیتواند بیشتر از {0} کاراکتر باشد";
    public const string DescriptionMaxLength = "توضیحات نمیتواند بیشتر از {0} کاراکتر باشد";
    public const string GuidIsInvalidFormat = "شناسه معتبر نمیباشد";
    public const string UserNotFound = "اطلاعات کاربر موجود نمی باشد ";
    public const string UserNotFoundByID = "کاربر با شناسه {0} یافت نشد";
    public const string TryAgain = "لطفا مجددا وارد شوید";
    public const string TokenIsNotValid = "اطلاعات توکن معتبر نمی باشد";
    public const string AccessIsNotpermitted = "مجوز دسترسی برای شما مقدور نمی‌باشد";
    public const string AssignedToUserId = "اختصاص به کاربر الزامی میباشد ";
    public const string GuidNotValid = "شناسه معتبر نمی باشد ";
    public const string TicketNotFound = "تیکت یافت نشد ";


}