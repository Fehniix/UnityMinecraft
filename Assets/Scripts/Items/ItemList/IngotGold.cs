public class IngotGold: Item
{
    public IngotGold()
    {
        this.itemName 				= "goldIngot";
		this.itemTextureName 		= "gold_ingot";
		this.placeable				= false;
		this.placeableOnlyOnTop 	= false;
		this.placeableOnOtherItems 	= false;
		this.hasGenericMesh			= true;
		this.LoadPrefab();
    }
}
