namespace SG.Domain;

public interface IEntity
{
  public int Id { get; set; }
}

public abstract class Entity : IEntity
{
      public int Id { get; set; }
}


///Leer https://github.com/SimonHolmquist/Challengers/blob/main/src/Challengers.Domain/Common/Entity.cs