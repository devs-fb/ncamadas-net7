namespace ModeloSimples.Service.API.Principal.Common;

public class ErrorResponse
{
    public List<string> Errors { get; set; }

    public ErrorResponse(List<string> errors)
    {
        Errors = errors;
    }
}
