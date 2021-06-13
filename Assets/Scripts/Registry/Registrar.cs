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
	}

	void RegisterItems()
	{
		Registry.RegisterItem<Torch>("torch");
		Registry.RegisterItem<Coal>("coal");
		Registry.RegisterItem<Diamond>("diamond");
		Registry.RegisterItem<Emerald>("emerald");
		Registry.RegisterItem<IngotIron>("ironIngot");
		Registry.RegisterItem<IngotGold>("goldIngot");
		Registry.RegisterItem<DiamondPickaxe>("diamondPickaxe");
	}

	void RegisterCraftingRecipes()
	{
		CraftingRecipeRegistry.RegisterRecipe(
			new CraftingRecipe(new string[3,3] {
				{"coal", null, null},
				{"stick", null, null},
				{null, null, null}
			}, 
			new CraftingResult("torch", 1)
		));

		CraftingRecipeRegistry.RegisterRecipe(
			new CraftingRecipe(new string[3,3] {
				{"log", null, null},
				{null, null, null},
				{null, null, null}
			}, 
			new CraftingResult("plank", 4)
		));

		CraftingRecipeRegistry.RegisterRecipe(
			new CraftingRecipe(new string[3,3] {
				{"cobblestone", "cobblestone", "cobblestone"},
				{"cobblestone", null, "cobblestone"},
				{"cobblestone", "cobblestone", "cobblestone"}
			}, 
			new CraftingResult("furnace", 1)
		));
	}
}
