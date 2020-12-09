using Vintagestory.API.Common;

[assembly: ModInfo("Farmland Drops Soil",
	Description = "Farmland has a change to drop soil depending on nutrient levels",
	Website     = "https://www.vintagestory.at/forums/topic/1209-farmland-drops-soil-v120/",
	Authors     = new []{ "copygirl" })]

namespace FarmlandDropsSoil
{
	public class FarmlandDropsSoilSystem : ModSystem
	{
		public override void Start(ICoreAPI api)
			=> api.RegisterBlockBehaviorClass("FarmlandDropsSoilBehavior", typeof(FarmlandDropsSoilBehavior));
	}
}
