using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace FarmlandDropsSoil
{
	public class FarmlandDropsSoilBehavior : BlockBehavior
	{
		public FarmlandDropsSoilBehavior(Block block)
			: base(block) {  }

		public override ItemStack[] GetDrops(
			IWorldAccessor world, BlockPos pos, IPlayer byPlayer,
			ref float dropChanceMultiplier, ref EnumHandling handling)
		{
			if ((byPlayer?.WorldData.CurrentGameMode == EnumGameMode.Creative) ||
			    (world.BlockAccessor.GetBlockEntity(pos) is not BlockEntityFarmland farmland))
				return new ItemStack[0];
			
			// Prevent other behaviors from running after this one.
			handling = EnumHandling.PreventSubsequent;
			
			// Divide each type of nutrient (N, P and K) by its original, "full"
			// fertility to get a value from 0 to 1. Then pick the lowest one.
			var nutrients = farmland.Nutrients.Zip(farmland.OriginalFertility,
				(current, original) => current / original).Min();
			
			// If this nutrient is below 95%, there is a chance the soil won't drop.
			if ((nutrients < 0.95) && (world.Rand.NextDouble() > nutrients))
				return new ItemStack[0];
			
			var fertility = this.block.FirstCodePart(2);
			var block = world.GetBlock(new AssetLocation("game", $"soil-{fertility}-none"));
			return new ItemStack[]{ new(block) };
		}
	}
}
