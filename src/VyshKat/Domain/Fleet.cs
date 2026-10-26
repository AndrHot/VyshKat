namespace VyshKat.Domain;

public sealed class Fleet
{
    private readonly List<Asset> _assets = new();

    public IReadOnlyList<Asset> Assets => _assets;

    public void Register(Asset asset)
    {
        if (asset.InventoryNumber != null)
            throw new InvalidOperationException($"«{asset.Name}» уже стоит на балансе.");
        asset.InventoryNumber = $"VK-{_assets.Count + 1:D4}";
        _assets.Add(asset);
    }
}
