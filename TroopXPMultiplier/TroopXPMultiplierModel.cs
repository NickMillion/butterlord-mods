using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents.Map;
using TaleWorlds.Library;

namespace TroopXPMultiplier
{
    class TroopXPMultiplierModel : DefaultCombatXpModel
    {
        public override void GetXpFromHit(CharacterObject attackerTroop, CharacterObject attackedTroop, int damage, bool isFatal, bool isSimulated, out int xpAmount)
        {
            int num = attackedTroop.MaxHitPoints();
            xpAmount = MBMath.Round(0.4f * ((attackedTroop.GetPower() + 0.5f) * (float)(Math.Min(damage, num) + (isFatal ? num : 0)))) * Core.multiplier;
            if (isSimulated)
            {
                xpAmount *= 8 * Core.multiplier;
            }
        }
    }
}
