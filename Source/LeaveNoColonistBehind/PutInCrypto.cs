using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;


namespace Fluffy
{
    public static class PutInCrypto
    {
        public static readonly DesignationDef designationDef = DefDatabase<DesignationDef>.GetNamed("Fluffy_PutInCrypto", true);
        public static readonly JobDef jobDef = DefDatabase<JobDef>.GetNamed("Fluffy_PutInCrypto", true);
        public static readonly ResearchProjectDef researchProjectDef = DefDatabase<ResearchProjectDef>.GetNamed("Stonecutting", true);
        public static readonly Texture2D icon = ContentFinder<Texture2D>.Get("UI/Designators/Hunt", true);
    }
}