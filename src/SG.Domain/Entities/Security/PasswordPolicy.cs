using SG.Domain.Base;

namespace SG.Domain.Entities.Security;

public class PasswordPolicy  : BaseEntity<int>
{ 
    public int MinimumDigits { get; set; }   // MinimoDigitos
    public bool RequiredLowercase { get; set; }   // MinusculaRequerida
    public bool RequiredUppercase { get; set; }   //MayusculaRequerida
    public bool RequiredCharacters { get; set; }   //CaracteresRequerido
    public string SpecialCharacters { get; set; } = string.Empty;  //CaracteresEspeciales
    public int TimeNoRepeat { get; set; }  // TiempoNoRepetir pass
    public char ChangeTimeType { get; set; }   // TipoTiempoCambio D= dias, M= meses, Y= anio
    public int PasswordChangeTime { get; set; }   // TiempoCambioContrasenia
    public int TemporaryPasswordChangeTime { get; set; }  //TiempoCambioContraseniaTemporal
}
