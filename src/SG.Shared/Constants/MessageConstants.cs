namespace SG.Shared.Constants;

public static class MESSAGE_CONSTANTS
{
    public const string NOT_ITEM_FOUND_DATABASE = "The record was not found in the database.";
    public const string VALIDATION_INVALID_CREDENTIALS = " Invalid credentials.";
    public const string VALIDATION_USER_DOESNT_EXIST = "User does not exist.";
    public const string VALIDATION_USER_IS_DISABLED = "The user is disabled.";
    public const string VALIDATION_USER_IS_BLOCKED = "The user is temporarily blocked.";
    public const string VALIDATION_USER_NAME_REGISTERED = "Username is already registered.";
    public const string VALIDATION_USER_EMAIL_REGISTERED = "Email is already registered.";
    public const string REFRESH_TOKEN_ERROR = "Eror refresh token.";
    public const string TOKEN_INVALID_OR_EXPIRED = "Invalid o expired token.";
    public const string VALIDATION_PASSWORD_REGEX = "The password must be at least 8 characters long, at least 1 uppercase letter, 1 lowercase letter, 1 number and 1 character.";
    public const string VALIDATION_CURRENT_PASSWORD_NOT_MATCH = "The current password does not match the one registered in the database.";

    public const string VALIDATION_PASWORD_IS_REQUIRED = "Password is required.";
    public const string VALIDATION_PASWORD_MIN_LENGTH = "Password must be at least 8 characters.";
    public const string VALIDATION_PASWORD_MUST_CONTAIN_UPPERCASE = "Password must contain at least one uppercase letter.";
    public const string VALIDATION_PASWORD_MUST_CONTAIN_LOWERCASE = "Password must contain at least one lowercase letter.";
    public const string VALIDATION_PASWORD_MUST_CONTAIN_DIGIT = "Password must contain at least one digit.";
    public const string VALIDATION_PASWORD_MUST_CHARACTER = "Password must contain at least one special character.";
}