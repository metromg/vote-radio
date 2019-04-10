using System.Threading.Tasks;

namespace Radio.Infrastructure.Synchronization.Services
{
    public interface ISongImportService
    {
        Task ImportAsync();
    }
}
