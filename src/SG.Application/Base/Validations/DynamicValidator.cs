using FluentValidation;
using FluentValidation.Results;

namespace SG.Application.Base.Validations;

public interface IDynamicValidator
{
    Task<ValidationResult> ValidateAsync(object model, CancellationToken cancellationToken = default);
}
public class DynamicValidator : IDynamicValidator
{
    private readonly IServiceProvider _serviceProvider;

    public DynamicValidator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<ValidationResult> ValidateAsync(object model, CancellationToken cancellationToken = default)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));

        var modelType = model.GetType();
        var validatorType = typeof(IValidator<>).MakeGenericType(modelType);

        if (_serviceProvider.GetService(validatorType) is not IValidator validator)
            return new ValidationResult(); 

        var context = new ValidationContext<object>(model);
        var result = await validator.ValidateAsync(context, cancellationToken);

        return result;
    }
}