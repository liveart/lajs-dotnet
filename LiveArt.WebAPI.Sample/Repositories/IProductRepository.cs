using LiveArt.WebAPI.Sample.Domain;

namespace LiveArt.WebAPI.Sample.Repositories
{
    public interface IProductRepository
    {
        Product Get(string id);
    }
}