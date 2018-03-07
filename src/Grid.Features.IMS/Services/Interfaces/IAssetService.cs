using Grid.Providers.Email;

namespace Grid.Features.IMS.Services.Interfaces
{
    public interface IAssetService
    {
        EmailContext ComposeEmailContextForAssetStateChanged(int assetId);
    }
}
