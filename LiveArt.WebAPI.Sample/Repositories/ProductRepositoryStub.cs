using LiveArt.WebAPI.Sample.Domain;

namespace LiveArt.WebAPI.Sample.Repositories
{
    public class ProductRepositoryStub:IProductRepository
    {
        public Product Get(string id)
        {
            // this is stupid implementation
            // in production you can get it from DB, config/products.json e.t.c
            return null;
        }
    }
}