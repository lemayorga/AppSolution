
namespace SG.Application.Base.Responses;

/// <summary>
/// Represents the result of an operation, including success status, error messages, and a value of type T if the operation was successful.
/// </summary>
/// <typeparam name="T">The type of the value contained in the result.</typeparam>
public class OperationResult<T>
{
    /// <summary>
    /// Indicates whether the operation was successful. If true, the Value property contains the result of the operation. If false, the Errors property contains error messages describing why the operation failed.
    /// </summary>
    public bool IsSuccess  { get; set; }

    /// Indicates whether the operation failed. If true, the Errors property contains error messages describing why the operation failed. If false, the Value property contains the result of the operation.
    public bool IsFailed { get; set; }

   /// <summary>
   ///  An array of error messages describing why the operation failed. This property is populated when IsFailed is true. 
   /// If IsSuccess is true, this property should be null or empty.
   /// </summary>
    public string[]? Errors { get; set; }
 
    /// <summary>
    /// The value resulting from the operation if it was successful. This property is populated when IsSuccess is true. If IsFailed is true, this property should be null.
    /// </summary>
    public T? Value { get; set; }
}
 
