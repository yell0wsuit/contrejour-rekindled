using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;

namespace Default.Namespace
{
    public static class MarketUtils
    {
        public static void NavigateToMarket()
        {
            try
            {
                Guide.ShowMarketplace(PlayerIndex.One);
            }
            catch (Exception)
            {
            }
        }
    }
}
