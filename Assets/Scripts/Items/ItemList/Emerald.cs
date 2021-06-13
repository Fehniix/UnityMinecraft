public class Emerald: Item
{
	public Emerald()
	{
		this.itemName 				= "emerald";
		this.itemTextureName 		= "emerald";
		this.placeable				= false;
		this.placeableOnlyOnTop 	= false;
		this.placeableOnOtherItems 	= false;
		this.hasGenericMesh			= true;
		this.LoadPrefab();
	}
}
