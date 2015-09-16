using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;
using RimWorld;

namespace Fluffy
{
    public class Designator_PutInCrypto : Designator
    {
        public override bool Visible
        {
            get
            {
                return DefDatabase<ResearchProjectDef>.GetNamed("ShipCryptosleep").IsFinished;
            }
        }

        private List<Pawn> justDesignated = new List<Pawn>();

        public override int DraggableDimensions
        {
            get
            {
                return 2;
            }
        }

        public Designator_PutInCrypto()
        {
            this.defaultLabel = "Put in cryptosleep casket";
            this.defaultDesc = "Put animal in cryptosleep casket. \nNo colonist left behind!";
            this.icon = ContentFinder<Texture2D>.Get("UI/Buttons/PutInCrypto", true);
            this.soundDragSustain = SoundDefOf.DesignateDragStandard;
            this.soundDragChanged = SoundDefOf.DesignateDragStandardChanged;
            this.useMouseIcon = true;
            this.soundSucceeded = SoundDefOf.DesignateHunt;
        }

        public override AcceptanceReport CanDesignateThing(Thing t)
        {
            Pawn pawn = t as Pawn;
            if (pawn != null && pawn.RaceProps.Animal && pawn.Faction == Faction.OfColony && Find.DesignationManager.DesignationOn(pawn, PutInCrypto.designationDef) == null)
            {
                return true;
            }
            return false;
        }

        public override AcceptanceReport CanDesignateCell(IntVec3 c)
        {
            if (!c.InBounds())
            {
                return false;
            }
            if (CryptoAnimalsInCell(c).Count() == 0)
            {
                return "Must designate animal of faction";
            }
            return true;
        }

        public override void DesignateSingleCell(IntVec3 loc)
        {
            foreach (Pawn current in this.CryptoAnimalsInCell(loc))
            {
                this.DesignateThing(current);
            }
        }

        public override void DesignateThing(Thing t)
        {
            Find.DesignationManager.RemoveAllDesignationsOn(t, false);
            Find.DesignationManager.AddDesignation(new Designation(t, PutInCrypto.designationDef));
            this.justDesignated.Add((Pawn)t);
        }

        public List<Thing> CryptoAnimalsInCell(IntVec3 loc)
        {
            List<Thing> list = new List<Thing>();
            if (loc.Fogged())
            {
                return list;
            }
            else
            {
                list.AddRange(loc.GetThingList().Where(t => CanDesignateThing(t).Accepted));
                return list;
            }
        }

        protected override void FinalizeDesignationSucceeded()
        {
            base.FinalizeDesignationSucceeded();
            foreach (Thing current in justDesignated)
            {
                if (Building_CryptosleepCasket.FindCryptosleepCasketFor(Find.ListerPawns.FreeColonistsSpawned.RandomElement(), (Pawn)current) == null)
                {
                    Messages.Message("No cryptosleep casket available for" + current.LabelCap, MessageSound.RejectInput);
                }
            }
            this.justDesignated.Clear();
        }
    }
}