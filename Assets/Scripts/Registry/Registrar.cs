using UnityEngine;

public class Registrar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		this.RegisterBlocks();
		this.RegisterItems();
		this.RegisterCraftingRecipes();
    }

	void RegisterBlocks()
	{
		Registry.RegisterItem<Air>("air");
        Registry.RegisterItem<Cobblestone>("cobblestone");
		Registry.RegisterItem<Stone>("stone");
		Registry.RegisterItem<Dirt>("dirt");
		Registry.RegisterItem<Grass>("grass");
		Registry.RegisterItem<Bedrock>("bedrock");
		Registry.RegisterItem<CraftingTable>("craftingTable");
		Registry.RegisterItem<Furnace>("furnace");
		Registry.RegisterItem<OreCoal>("oreCoal");
		Registry.RegisterItem<OreIron>("oreIron");
		Registry.RegisterItem<OreDiamond>("oreDiamond");
		Registry.RegisterItem<OreEmerald>("oreEmerald");
		Registry.RegisterItem<OreGold>("oreGold");
		Registry.RegisterItem<Log>("log");
		Registry.RegisterItem<Planks>("planks");
		Registry.RegisterItem<Leaves>("leaves");
	}

	void RegisterItems()
	{
		Registry.RegisterItem<Torch>("torch");
		Registry.RegisterItem<Coal>("coal");
		Registry.RegisterItem<Diamond>("diamond");
		Registry.RegisterItem<Emerald>("emerald");
		Registry.RegisterItem<IngotIron>("ironIngot");
		Registry.RegisterItem<IngotGold>("goldIngot");
		Registry.RegisterItem<WoodPickaxe>("woodPickaxe");
		Registry.RegisterItem<StonePickaxe>("stonePickaxe");
		Registry.RegisterItem<IronPickaxe>("ironPickaxe");
		Registry.RegisterItem<GoldPickaxe>("goldPickaxe");
		Registry.RegisterItem<DiamondPickaxe>("diamondPickaxe");
		Registry.RegisterItem<Stick>("stick");
	}

	void RegisterCraftingRecipes()
	{
		CraftingRecipeRegistry.RegisterRecipe(
			new CraftingRecipe(new string[3,3] {
				{"coal", null, null},
				{"stick", null, null},
				{null, null, null}
			}, 
			new CraftingResult("torch", 4)
		));

		CraftingRecipeRegistry.RegisterRecipe(
			new CraftingRecipe(new string[3,3] {
				{"planks", "planks", "planks"},
				{null, "stick", null},
				{null, "stick", null}
			}, 
			new CraftingResult("woodPickaxe", 1)
		));

		CraftingRecipeRegistry.RegisterRecipe(
			new CraftingRecipe(new string[3,3] {
				{"cobblestone", "cobblestone", "cobblestone"},
				{null, "stick", null},
				{null, "stick", null}
			}, 
			new CraftingResult("stonePickaxe", 1)
		));

		CraftingRecipeRegistry.RegisterRecipe(
			new CraftingRecipe(new string[3,3] {
				{"ironIngot", "ironIngot", "ironIngot"},
				{null, "stick", null},
				{null, "stick", null}
			}, 
			new CraftingResult("ironPickaxe", 1)
		));

		CraftingRecipeRegistry.RegisterRecipe(
			new CraftingRecipe(new string[3,3] {
				{"goldIngot", "goldIngot", "goldIngot"},
				{null, "stick", null},
				{null, "stick", null}
			}, 
			new CraftingResult("goldPickaxe", 1)
		));

		CraftingRecipeRegistry.RegisterRecipe(
			new CraftingRecipe(new string[3,3] {
				{"diamond", "diamond", "diamond"},
				{null, "stick", null},
				{null, "stick", null}
			}, 
			new CraftingResult("diamondPickaxe", 1)
		));

		CraftingRecipeRegistry.RegisterRecipe(
			new CraftingRecipe(new string[3,3] {
				{"log", null, null},
				{null, null, null},
				{null, null, null}
			}, 
			new CraftingResult("planks", 4)
		));

		CraftingRecipeRegistry.RegisterRecipe(
			new CraftingRecipe(new string[3,3] {
				{"cobblestone", "cobblestone", "cobblestone"},
				{"cobblestone", null, "cobblestone"},
				{"cobblestone", "cobblestone", "cobblestone"}
			}, 
			new CraftingResult("furnace", 1)
		));

		CraftingRecipeRegistry.RegisterRecipe(
			new CraftingRecipe(new string[3,3] {
				{"plank", "plank", null},
				{"plank", "plank", null},
				{null, null, null}
			}, 
			new CraftingResult("craftingTable", 1)
		));
	}
}
