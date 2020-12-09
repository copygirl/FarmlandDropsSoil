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

		public override void OnBlockBroken(
			IWorldAccessor world, BlockPos pos,
			IPlayer byPlayer, ref EnumHandling handling)
		{
			if ((byPlayer.WorldData.CurrentGameMode == EnumGameMode.Creative) ||
				!(world.BlockAccessor.GetBlockEntity(pos) is BlockEntityFarmland farmland))
				return;
			
			// Get the lowest nutrient value (out of N, P and K) relative
			// to the farmland's default nutrient levels (its fertility).
			var nutrients = farmland.Nutrients.Min() / farmland.OriginalFertility;
			
			// If this nutrient is below 95%, there is a chance the soil won't drop.
			if ((nutrients < 0.95) && (world.Rand.NextDouble() > nutrients))
				return;
			
			var fertility = this.block.FirstCodePart(2);
			var block = world.GetBlock(new AssetLocation("game", $"soil-{fertility}-none"));
			world.SpawnItemEntity(new ItemStack(block), new Vec3d(pos.X + 0.5, pos.Y + 0.5, pos.Z + 0.5), null);
		}
	}
}
