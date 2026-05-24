namespace Hackathon_segunda_chamada.DTOs
{
    public record CriarRequerimentoResultDto(
        bool Sucesso,
        int? Id,
        string Mensagem
    );
}
