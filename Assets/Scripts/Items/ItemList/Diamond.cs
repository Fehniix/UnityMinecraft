public class Diamond: Item
{
    public Diamond()
    {
        this.itemName 				= "diamond";
		this.itemTextureName 		= "diamond";
		this.placeable				= false;
		this.placeableOnlyOnTop 	= false;
		this.placeableOnOtherItems 	= false;
		this.hasGenericMesh			= true;
		this.LoadPrefab();
    }
}
