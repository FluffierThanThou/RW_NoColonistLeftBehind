using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Fluffy
{
    public class Building_CryptosleepCasket : RimWorld.Building_CryptosleepCasket
    {
        // logic for accepting pawn by bodysize
        // RimWorld.Building_CryptosleepCasket
        public static new Building_CryptosleepCasket FindCryptosleepCasketFor(Pawn p, Pawn traveler)
        {
            try {
                IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
                                                   where typeof(Fluffy.Building_CryptosleepCasket).IsAssignableFrom(def.thingClass)
                                                   select def;

                foreach (ThingDef current in enumerable)
                {
                    Predicate<Thing> validator = (Thing x) => ((Building_CryptosleepCasket)x).GetContainer().Count == 0 && ((Building_CryptosleepCasket)x).TryGetComp<CompAnimalCrypto>().props.maxSizeAllowance >= traveler.BodySize;
                    Building_CryptosleepCasket building_CryptosleepCasket = (Building_CryptosleepCasket)GenClosest.ClosestThingReachable(p.Position, ThingRequest.ForDef(current), PathEndMode.InteractionCell, TraverseParms.For(traveler, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, null, -1, false);

                    if (building_CryptosleepCasket != null)
                    {
                        return building_CryptosleepCasket;
                    }
                }
                return null;
            }
            finally
            {
#if DEBUG
                Log.Message("Something odd happened.");
#endif
            }
        }
    }
}
