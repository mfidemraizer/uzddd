using System.Threading.Tasks;

namespace UserZoom.Shared.Data
{
    public interface IDataHandler<TObject>
    {
        Task OnAddAsync(TObject domainObject);
        Task OnUpdateAsync(TObject domainObject);
    }
}
