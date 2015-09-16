using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;
using Verse.AI;
using RimWorld;

namespace Fluffy
{
    class JobDriver_CarryAnimalToCryptosleepCasket : JobDriver
    {
        protected override IEnumerable<Toil> MakeNewToils()
        {
#if DEBUG
            Log.Message("StartToils OK");
            Log.Message(Takee.ToString());
            Log.Message(pawn.ToString());
            Log.Message(DropPod.ToString());

#endif
            yield return Toils_Reserve.Reserve(TargetIndex.A, 1);
            yield return Toils_Reserve.Reserve(TargetIndex.B, 1);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell).FailOnDestroyedOrForbidden(TargetIndex.A).FailOnDespawnedOrForbidden(TargetIndex.B).FailOn(() => DropPod.GetContainer().Count > 0); //.FailOn(() => pawn.CanReach(Takee, PathEndMode.Touch, Danger.Deadly, false));
            yield return Toils_Haul.StartCarryThing(TargetIndex.A);
            yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.InteractionCell).FailOnDestroyed(TargetIndex.B);

            Toil prepare = new Toil();
            prepare.defaultCompleteMode = ToilCompleteMode.Delay;
            prepare.defaultDuration = 500;
            yield return prepare;

            Toil putInPod = new Toil();
            putInPod.initAction = delegate
            {
                pawn.carrier.GetContainer().TransferToContainer(Takee, DropPod.GetContainer(), 1);
            };
            putInPod.defaultCompleteMode = ToilCompleteMode.Instant;
            yield return putInPod;
            yield break;
        }

        private const TargetIndex TakeeInd = TargetIndex.A;

        protected Pawn Takee
        {
            get
            {
                return (Pawn)base.CurJob.GetTarget(TargetIndex.A).Thing;
            }
        }

        protected Building_CryptosleepCasket DropPod
        {
            get
            {
                return (Building_CryptosleepCasket)base.CurJob.targetB.Thing;
            }
        }
    }
}
