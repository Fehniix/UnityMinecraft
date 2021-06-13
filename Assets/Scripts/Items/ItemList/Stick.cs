public class Stick: Item
{
    public Stick()
    {
        this.itemName 				= "stick";
		this.itemTextureName 		= "stick";
		this.placeable				= false;
		this.placeableOnlyOnTop 	= false;
		this.placeableOnOtherItems 	= false;
		this.hasGenericMesh			= true;
		this.burnable				= true;
		this.burnTime			 	= 100;
		this.LoadPrefab();
    }
}
