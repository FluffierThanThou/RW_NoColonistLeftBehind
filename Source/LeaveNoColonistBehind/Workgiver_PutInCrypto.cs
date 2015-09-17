using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;


namespace Fluffy
{
    class Workgiver_PutInCrypto : WorkGiver_Scanner
    {
        public override PathEndMode PathEndMode
        {
            get
            {
                return PathEndMode.Touch;
            }
        }

        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {

            List<Thing> possibleTargets = new List<Thing>();
            if (Find.ListerBuildings.AllBuildingsColonistOfClass<Fluffy.Building_CryptosleepCasket>().ToList().NullOrEmpty())
            {
                return possibleTargets;
            }
            foreach (Designation curDesignation in Find.DesignationManager.DesignationsOfDef(PutInCrypto.designationDef))
            {
                Thing curThing = curDesignation.target.Thing;
                if ((curThing.Position.InBounds()) && (pawn.CanReserveAndReach(curDesignation.target, PathEndMode.Touch, Danger.None)))
                {
                    possibleTargets.Add(curThing);
                }
            }
            return possibleTargets.AsEnumerable<Thing>();
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t)
        {
            Pawn pawn2 = t as Pawn;
            if (pawn2 == null) return false;
            bool casket = Building_CryptosleepCasket.FindCryptosleepCasketFor(pawn2, pawn) != null;
            return pawn2 != null && casket && pawn2.RaceProps.Animal && pawn2.CasualInterruptibleNow() && pawn.CanReserveAndReach(pawn2, PathEndMode.Touch, Danger.Deadly);
        }

        public override Job JobOnThing(Pawn pawn, Thing thing)
        {
            Pawn pawn2 = thing as Pawn;
            Designation_PutInCrypto des = (Designation_PutInCrypto)Find.DesignationManager.DesignationOn(thing, PutInCrypto.designationDef);
            Building_CryptosleepCasket Crypto = (Building_CryptosleepCasket)des.target2;
            if (Crypto == null)
            {
                Crypto = Building_CryptosleepCasket.FindCryptosleepCasketFor(pawn2, pawn);
            }
            if (Crypto == null)
            {
                return null;
            }
            return new Job(PutInCrypto.jobDef, (Pawn)thing, Crypto)
            {
                maxNumToCarry = 1
            };
        }
    }
}
