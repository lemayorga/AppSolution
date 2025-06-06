namespace SG.Domain.Base;

public abstract class BaseEntity<Tkey>
{
    public Tkey Id { get; protected set; } = default!;

    protected BaseEntity() { }

    protected BaseEntity(Tkey id)
    {
        Id = id;
    }

    public override bool Equals(object? obj)
        => obj is BaseEntity<Tkey> other && EqualityComparer<Tkey>.Default.Equals(Id, other.Id);

    public override int GetHashCode()
        => Id?.GetHashCode() ?? 0;

    public Tkey? GetDefaultKey()
        => default(Tkey); 
    
    public bool IsDefaultId()
      => EqualityComparer<Tkey>.Default.Equals(Id, default(Tkey));
    
    public void SetId(Tkey id)
        => Id = id;

    public void SetIdIfIsDefaultValue(Tkey id)
        =>  Id =  IsDefaultId() ? id : Id;
} 