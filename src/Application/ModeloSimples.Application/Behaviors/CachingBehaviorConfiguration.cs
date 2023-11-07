namespace ModeloSimples.Application.Behaviors;

public class CachingBehaviorConfiguration
{
    public bool AtivarCaching { get; set; }
    public List<ComandosCaching> ComandosCaching { get; set; }
}

public class ComandosCaching
{
    public bool AtivarCaching { get; set; }
    public string Commando { get; set; }
    public int TempoDeCacheEmSegundos { get; set; } = 360;
    public int TempoDeCacheEmSegundosQueSeRenovamComOUso { get; set; } = 60;
}